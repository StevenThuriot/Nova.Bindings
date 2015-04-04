using System;
using System.Diagnostics.Contracts;

namespace Nova.Bindings.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingsAttribute : Attribute
    {
        public SettingsAttribute(string id)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(id), "The supplied id can't be null, empty or whitespace.");

            Id = id;
        }

        public string Id { get; private set; }
    }
}
