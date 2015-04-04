using System.Windows;
using Nova.Bindings.Base;

namespace Nova.Bindings.Listeners
{
    abstract class ResolvableDataContextListener<T> : DataContextListener<T> 
        where T : FrameworkElement
    {
        private readonly ResolveMode _resolveMode;

        protected ResolvableDataContextListener(PropertyPath path, ResolveMode resolveMode) 
            : base(path)
        {
            _resolveMode = resolveMode;
        }
        protected override void OnDataContextChanged(T element, object dataContext)
        {
            if (OnResolvableDataContextChanged(element, dataContext))
            {
                if (_resolveMode == ResolveMode.Once) DetachFrom(element);
            }
        }

        protected abstract bool OnResolvableDataContextChanged(T element, object dataContext);
    }
}