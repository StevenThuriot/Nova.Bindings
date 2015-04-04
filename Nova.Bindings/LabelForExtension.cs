using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Nova.Bindings.Base;
using Nova.Bindings.Listeners;

namespace Nova.Bindings
{
    public sealed class LabelForExtension : ResolvableNovaMarkUpExtension
    {
        public bool AppendColon { get; set; }

        public LabelForExtension(PropertyPath path)
            : base(path, ResolveMode.Once)
        {
            AppendColon = true;
        }

        protected override object ProvideValue(IServiceProvider serviceProvider, IProvideValueTarget target)
        {
            var textBlock = target.TargetObject as TextBlock;

            if (textBlock != null)
            {
                var listener = new TextBlockDataContextListener(Path, ResolveMode, AppendColon);
                listener.AttachTo(textBlock);

                textBlock.SetResourceReference(FrameworkElement.StyleProperty, "NovaTextBlock");

                return textBlock.Text;
            }

            var label = target.TargetObject as ContentControl; //Label is a ContentControl
            if (label != null)
            {
                var listener = new ContentControlDataContextListener(Path, ResolveMode, AppendColon);
                listener.AttachTo(label);

                return label.Content;
            }

            throw new NotSupportedException("{0} is not supported yet when using the LabelFor extension.");
        }
    }
}