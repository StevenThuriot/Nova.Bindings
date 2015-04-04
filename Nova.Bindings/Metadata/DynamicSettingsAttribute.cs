using System;

namespace Nova.Bindings.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DynamicSettingsAttribute : Attribute
    {
    }
}
