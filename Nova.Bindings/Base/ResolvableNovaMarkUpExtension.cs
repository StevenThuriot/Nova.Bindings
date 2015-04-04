using System.Windows;

namespace Nova.Bindings.Base
{
    public abstract class ResolvableNovaMarkUpExtension : NovaMarkUpExtension
    {
        protected ResolvableNovaMarkUpExtension(PropertyPath path)
            : this(path, ResolveMode.OnDataContextChange)
        {
        }

        protected ResolvableNovaMarkUpExtension(PropertyPath path, ResolveMode mode)
            : base(path)
        {
            ResolveMode = mode;
        }

        public ResolveMode ResolveMode { get; set; }
    }
}