using System.Windows;
using System.Windows.Controls;
using Nova.Bindings.PropertyWalkers;

namespace Nova.Bindings.Listeners
{
    class ContentControlDataContextListener : ResolvableDataContextListener<ContentControl>
    {
        private readonly bool _appendColon;

        public ContentControlDataContextListener(PropertyPath path, ResolveMode resolveMode, bool appendColon) 
            : base(path, resolveMode)
        {
            _appendColon = appendColon;
        }

        protected override bool OnResolvableDataContextChanged(ContentControl element, object dataContext)
        {
            if (dataContext == null)
            {
                element.Content = "";
                return false;
            }

            var manager = element.FindSettingsManager();

            var id = PropertyWalker.GetDefinitionId(dataContext, Path);
            var definition = manager.GetDefinition(id);

            var label = definition.Label;

            if (_appendColon)
                label += ":";

            element.Content = label;
            return true;
        }
    }
}
