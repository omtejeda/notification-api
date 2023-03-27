using System;
namespace NotificationService.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousAttribute : Attribute
    {
        public AllowAnonymousAttribute() {}
    }
}