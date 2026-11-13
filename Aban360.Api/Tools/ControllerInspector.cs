namespace Aban360.Api.Tools
{
    using System.Reflection;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using Aban360.Api.Controllers.V1;
    using Aban360.Api.Filters;

    public static class ControllerInspector
    {
        public static void ExportControllerActions(string fileName = "ControllersAndActions.csv")
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Find all controller types
            var controllers = assembly.GetTypes()
                .Where(t => !t.IsAbstract &&
                    (typeof(BaseController).IsAssignableFrom(t) ||
                     (typeof(ControllerBase).IsAssignableFrom(t) && t.IsDefined(typeof(ApiControllerAttribute), true))))
                .ToList();

            var lines = new List<string> { "DirectoryPath, ControllerName, ActionName, Route" };

            foreach (var controller in controllers)
            {
                // Get controller directory path (namespace → approximate path)
                var namespacePath = controller.Namespace?.Replace('.', Path.DirectorySeparatorChar) ?? "";

                // Get controller name (without "Controller" suffix)
                var controllerName = controller.Name.Replace("Controller", "");

                // Get controller Route
                var controllerRoute = controller?.GetCustomAttribute<RouteAttribute>()?.Template ?? "";

                // Find public actions
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)) &&
                        (typeof(BaseController).IsAssignableFrom(controller) || m.IsDefined(typeof(AuthorizeAttribute), true)))
                    //.Select(m => m.Name)
                    .ToList();

                foreach (var action in actions)
                {
                    var actionRoute = action?.GetCustomAttribute<RouteAttribute>()?.Template
                        ?? action?.GetCustomAttributes<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>().Select(a => a.Template).FirstOrDefault(t => t != null) ?? "";
                    var permissionControllerName = action?.GetCustomAttribute<EndpointAuthorizationAttribute>()?.ControllerName ?? controllerName;
                    var fullRoute = actionRoute.StartsWith("~/") ? actionRoute.Substring(2)
                        : actionRoute.StartsWith("/") ? actionRoute.Substring(1)
                        : string.IsNullOrEmpty(controllerRoute) ? actionRoute : $"{controllerRoute}/{actionRoute}";
                    var actionName = action?.GetCustomAttribute<ActionNameAttribute>()?.Name ?? action.Name;
                    lines.Add($"{namespacePath}, {permissionControllerName}, {actionName}, {fullRoute}");
                }
            }
            var baseDir = AppContext.BaseDirectory; // bin/Debug/netX.X/...
            var excelDir = Path.Combine(baseDir, "AppData", "Excels");

            if (!Directory.Exists(excelDir))
                Directory.CreateDirectory(excelDir);

            var outputPath = Path.Combine(excelDir, fileName);

            File.WriteAllLines(outputPath, lines);
        }
    }
}
