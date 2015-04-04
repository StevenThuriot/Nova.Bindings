using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Nova.Bindings.Base;
using Nova.Bindings.Listeners;
using Nova.Bindings.Metadata;

namespace Nova.Bindings
{
    public sealed class ValueBindingExtension : NovaMarkUpExtension
    { 
        public ValueBindingExtension(PropertyPath path) : base(path)
        {
            Mode = BindingMode.Default;
        }

        public BindingMode Mode { get; set; }
        public IValueConverter Converter { get; set; }

        protected override object ProvideValue(IServiceProvider serviceProvider, IProvideValueTarget target)
        {
            if (!(target.TargetObject is IHaveSettings))
                throw new NotSupportedException("The target has to implement IHaveSettings");

            var targetObject = (FrameworkElement) target.TargetObject;

            var listener = new ControlDataContextListener(Path);
            listener.AttachTo(targetObject);

            var binding = new Binding
            {
                Mode = Mode,
                Path = Path,
                Converter = Converter
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}