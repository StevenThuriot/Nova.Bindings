using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Data;

namespace Nova.Bindings.Base
{
    internal abstract class DataContextListener<T>
        where T : FrameworkElement
    {
        private readonly PropertyPath _path;

        protected DataContextListener(PropertyPath path)
        {
            Contract.Requires<ArgumentNullException>(path != null, "The supplied path can't be null.");
            Contract.Requires<ArgumentNullException>(path.Path != ".", "The supplied path can't '.'.");

            _path = path;
        }

        protected PropertyPath Path
        {
            get { return _path; }
        }

        public void AttachTo(T element)
        {
            Contract.Requires<ArgumentNullException>(element != null, "Can't attach to null framework elements.");

            element.DataContextChanged += OnDataContextChangedInternal;

            var datacontext = element.GetDatacontext();
            if (datacontext == null) return;

            //Trigger on attach in case we already have a datacontext.
            var args = new DependencyPropertyChangedEventArgs(FrameworkElement.DataContextProperty, null, datacontext);
            OnDataContextChangedInternal(element, args);
        }

        public void DetachFrom(T element)
        {
            Contract.Requires<ArgumentNullException>(element != null, "Can't attach to null framework elements.");

            element.DataContextChanged -= OnDataContextChangedInternal;
        }

        protected abstract void OnDataContextChanged(T element, object dataContext);
        protected virtual void OnDataContextDisconnected(T element) { }

        private void OnDataContextChangedInternal(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var element = (T)sender;
            var dataContext = dependencyPropertyChangedEventArgs.NewValue;

            if (dataContext == BindingOperations.DisconnectedSource)
            {
                OnDataContextDisconnected(element);
            }
            else
            {
                OnDataContextChanged(element, dataContext);
            }
        }
    }
}
