namespace Aban360.Api.Tools
{
    using System.Reflection;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Aban360.Api.Controllers.V1;

    public static class ControllerInspector
    {
        public static void ExportControllerActions(string fileName= "ControllersAndActions.csv")
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Find all controller types
            var controllers = assembly.GetTypes()
                .Where(t => typeof(BaseController).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();

            var lines = new List<string> { "DirectoryPath, ControllerName, ActionName" };

            foreach (var controller in controllers)
            {
                // Get controller directory path (namespace → approximate path)
                var namespacePath = controller.Namespace?.Replace('.', Path.DirectorySeparatorChar) ?? "";

                // Get controller name (without "Controller" suffix)
                var controllerName = controller.Name.Replace("Controller", "");

                // Find public actions
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)))
                    .Select(m => m.Name)
                    .ToList();

                foreach (var action in actions)
                {
                    lines.Add($"{namespacePath}, {controllerName}, {action}");
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
