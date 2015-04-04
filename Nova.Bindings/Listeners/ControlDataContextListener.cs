using System.Windows;
using Nova.Bindings.Base;
using Nova.Bindings.Metadata;
using Nova.Bindings.PropertyWalkers;

namespace Nova.Bindings.Listeners
{
    class ControlDataContextListener : DataContextListener<FrameworkElement>
    {
        public ControlDataContextListener(PropertyPath path) 
            : base(path)
        {
        }

        protected override void OnDataContextChanged(FrameworkElement element, object dataContext)
        {
            IDefinition definition = null;

            if (dataContext != null)
            {
                var manager = element.FindSettingsManager();

                var id = PropertyWalker.GetDefinitionId(dataContext, Path);
                
                definition = manager.GetDefinition(id);
            }

            ((IHaveSettings)element).Settings = definition;
        }

        protected override void OnDataContextDisconnected(FrameworkElement element)
        {
            ((IHaveSettings)element).Settings = null;
        }
    }
}
