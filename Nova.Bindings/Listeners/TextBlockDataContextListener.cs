using System.Windows;
using System.Windows.Controls;
using Nova.Bindings.PropertyWalkers;

namespace Nova.Bindings.Listeners
{
    class TextBlockDataContextListener : ResolvableDataContextListener<TextBlock>
    {
        private readonly bool _appendColon;

        public TextBlockDataContextListener(PropertyPath path, ResolveMode resolveMode, bool appendColon) 
            : base(path, resolveMode)
        {
            _appendColon = appendColon;
        }

        protected override bool OnResolvableDataContextChanged(TextBlock element, object dataContext)
        {
            if (dataContext == null)
            {
                element.Text = "";
                return false;
            }

            var manager = element.FindSettingsManager();

            var id = PropertyWalker.GetDefinitionId(dataContext, Path);
            var definition = manager.GetDefinition(id);

            var label = definition.Label;

            if (_appendColon)
                label += ":";

            element.Text = label;
            return true;
        }
    }
}