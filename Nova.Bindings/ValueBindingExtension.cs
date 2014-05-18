#region License
//  
// Copyright 2014 Steven Thuriot
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
#endregion

using System;
using System.Diagnostics.Contracts;
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