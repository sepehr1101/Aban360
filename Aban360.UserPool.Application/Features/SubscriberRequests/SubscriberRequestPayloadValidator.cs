using System.Text.Json;
using System.Text.RegularExpressions;

namespace Aban360.UserPool.Application.Features.SubscriberRequests;

public sealed class SubscriberRequestPayloadValidator
{
    private static readonly HashSet<string> Keywords = [
        "type", "properties", "required", "additionalProperties", "items", "enum",
        "minLength", "maxLength", "pattern", "minimum", "maximum", "minItems", "maxItems",
        "title", "description"];
    private static readonly HashSet<string> Types = ["object", "array", "string", "integer", "number", "boolean", "null"];

    public void Validate(string schemaJson, JsonElement payload)
    {
        using var schema = ParseSchema(schemaJson);
        CheckSchema(schema.RootElement, 0);
        if (schema.RootElement.GetProperty("type").GetString() != "object")
            throw ConfigurationError();
        var errors = new List<string>();
        CheckValue(schema.RootElement, payload, "$", errors, 0);
        if (errors.Count != 0) throw new SubscriberRequestPayloadException(errors);
    }

    private static JsonDocument ParseSchema(string json)
    {
        try { return JsonDocument.Parse(json, new JsonDocumentOptions { MaxDepth = 32 }); }
        catch (JsonException) { throw ConfigurationError(); }
    }

    private static SubscriberRequestException ConfigurationError() =>
        new("قالب اطلاعات این خدمت معتبر نیست؛ با پشتیبانی تماس بگیرید.");

    private static void CheckSchema(JsonElement schema, int depth)
    {
        if (depth > 16 || schema.ValueKind != JsonValueKind.Object ||
            !schema.TryGetProperty("type", out var type) || type.ValueKind != JsonValueKind.String ||
            !Types.Contains(type.GetString()!)) throw ConfigurationError();
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in schema.EnumerateObject())
        {
            if (!names.Add(property.Name) || !Keywords.Contains(property.Name)) throw ConfigurationError();
            switch (property.Name)
            {
                case "properties":
                    if (type.GetString() != "object" || property.Value.ValueKind != JsonValueKind.Object) throw ConfigurationError();
                    var fields = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var field in property.Value.EnumerateObject())
                    {
                        if (!fields.Add(field.Name)) throw ConfigurationError();
                        CheckSchema(field.Value, depth + 1);
                    }
                    break;
                case "required":
                    if (type.GetString() != "object" || property.Value.ValueKind != JsonValueKind.Array) throw ConfigurationError();
                    foreach (var field in property.Value.EnumerateArray())
                        if (field.ValueKind != JsonValueKind.String ||
                            !schema.TryGetProperty("properties", out var props) || !props.TryGetProperty(field.GetString()!, out _))
                            throw ConfigurationError();
                    break;
                case "additionalProperties":
                    if (property.Value.ValueKind != JsonValueKind.False) throw ConfigurationError();
                    break;
                case "items": CheckSchema(property.Value, depth + 1); break;
                case "enum":
                    if (property.Value.ValueKind != JsonValueKind.Array || property.Value.GetArrayLength() == 0 ||
                        property.Value.EnumerateArray().Any(x => x.ValueKind is JsonValueKind.Object or JsonValueKind.Array))
                        throw ConfigurationError();
                    break;
                case "minLength": case "maxLength": case "minItems": case "maxItems":
                    if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt32(out var length) || length < 0) throw ConfigurationError();
                    break;
                case "minimum": case "maximum":
                    if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetDecimal(out _)) throw ConfigurationError();
                    break;
                case "pattern":
                    if (property.Value.ValueKind != JsonValueKind.String) throw ConfigurationError();
                    try { _ = new Regex(property.Value.GetString()!, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100)); }
                    catch (ArgumentException) { throw ConfigurationError(); }
                    break;
            }
        }
        if (type.GetString() == "object" && (!schema.TryGetProperty("properties", out _) ||
            !schema.TryGetProperty("additionalProperties", out var additional) || additional.ValueKind != JsonValueKind.False))
            throw ConfigurationError();
        if (type.GetString() == "array" && !schema.TryGetProperty("items", out _)) throw ConfigurationError();
    }

    private static void CheckValue(JsonElement schema, JsonElement value, string path, List<string> errors, int depth)
    {
        if (errors.Count >= 20) return;
        if (depth > 16) { errors.Add($"{path}: عمق اطلاعات بیش از حد مجاز است"); return; }
        var type = schema.GetProperty("type").GetString();
        bool matches = type switch
        {
            "object" => value.ValueKind == JsonValueKind.Object,
            "array" => value.ValueKind == JsonValueKind.Array,
            "string" => value.ValueKind == JsonValueKind.String,
            "integer" => value.ValueKind == JsonValueKind.Number && value.TryGetDecimal(out var n) && decimal.Truncate(n) == n,
            "number" => value.ValueKind == JsonValueKind.Number && value.TryGetDecimal(out _),
            "boolean" => value.ValueKind is JsonValueKind.True or JsonValueKind.False,
            "null" => value.ValueKind == JsonValueKind.Null,
            _ => false
        };
        if (!matches) { errors.Add($"{path}: نوع مقدار باید {type} باشد"); return; }
        if (schema.TryGetProperty("enum", out var allowed) && !allowed.EnumerateArray().Any(x => ScalarEquals(x, value)))
            errors.Add($"{path}: مقدار در فهرست مقادیر مجاز نیست");
        switch (type)
        {
            case "object":
                var properties = schema.GetProperty("properties");
                if (schema.TryGetProperty("required", out var required))
                    foreach (var field in required.EnumerateArray())
                        if (!value.TryGetProperty(field.GetString()!, out _)) errors.Add($"{path}.{field.GetString()}: الزامی است");
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var field in value.EnumerateObject())
                {
                    if (!seen.Add(field.Name)) errors.Add($"{path}.{field.Name}: فیلد تکراری است");
                    else if (!properties.TryGetProperty(field.Name, out var child)) errors.Add($"{path}.{field.Name}: فیلد مجاز نیست");
                    else CheckValue(child, field.Value, path + "." + field.Name, errors, depth + 1);
                }
                break;
            case "array":
                CheckRange(schema, value.GetArrayLength(), "minItems", "maxItems", path, errors);
                int index = 0;
                foreach (var item in value.EnumerateArray()) CheckValue(schema.GetProperty("items"), item, $"{path}[{index++}]", errors, depth + 1);
                break;
            case "string":
                var text = value.GetString()!;
                CheckRange(schema, text.EnumerateRunes().Count(), "minLength", "maxLength", path, errors);
                if (schema.TryGetProperty("pattern", out var pattern))
                {
                    try
                    {
                        if (!Regex.IsMatch(text, pattern.GetString()!, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100)))
                            errors.Add($"{path}: قالب مقدار معتبر نیست");
                    }
                    catch (RegexMatchTimeoutException) { errors.Add($"{path}: بررسی قالب مقدار ممکن نشد"); }
                }
                break;
            case "integer": case "number":
                CheckRange(schema, value.GetDecimal(), "minimum", "maximum", path, errors);
                break;
        }
    }

    private static bool ScalarEquals(JsonElement a, JsonElement b) => a.ValueKind == b.ValueKind &&
        (a.ValueKind == JsonValueKind.String ? a.GetString() == b.GetString() :
         a.ValueKind == JsonValueKind.Number ? a.TryGetDecimal(out var x) && b.TryGetDecimal(out var y) && x == y :
         a.GetRawText() == b.GetRawText());

    private static void CheckRange(JsonElement schema, decimal value, string minimum, string maximum, string path, List<string> errors)
    {
        if (schema.TryGetProperty(minimum, out var min) && value < min.GetDecimal()) errors.Add($"{path}: کمتر از حد مجاز است");
        if (schema.TryGetProperty(maximum, out var max) && value > max.GetDecimal()) errors.Add($"{path}: بیشتر از حد مجاز است");
    }
}
