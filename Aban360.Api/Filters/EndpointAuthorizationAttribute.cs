using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Filters
{
    /// <summary>Keeps an existing permission key when actions move to a consolidated controller.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EndpointAuthorizationAttribute : TypeFilterAttribute
    {
        public string ControllerName { get; }

        public EndpointAuthorizationAttribute(string controllerName)
            : base(typeof(EndpointAuthorizationFilter))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(controllerName);
            ControllerName = controllerName;
            Arguments = new object[] { controllerName };
        }
    }
}
