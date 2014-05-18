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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Nova.Bindings.Base;
using Nova.Bindings.Listeners;

namespace Nova.Bindings
{
    public class LabelForExtension : ResolvableNovaMarkUpExtension
    {
        public LabelForExtension(PropertyPath path)
            : base(path, ResolveMode.Once)
        {
        }

        protected override object ProvideValue(IServiceProvider serviceProvider, IProvideValueTarget target)
        {
            var textBlock = target.TargetObject as TextBlock;

            if (textBlock != null)
            {
                var listener = new TextBlockDataContextListener(Path, ResolveMode);
                listener.AttachTo(textBlock);

                textBlock.SetResourceReference(FrameworkElement.StyleProperty, "NovaTextBlock");

                return textBlock.Text;
            }

            var label = target.TargetObject as ContentControl; //Label is a ContentControl
            if (label != null)
            {
                var listener = new ContentControlDataContextListener(Path, ResolveMode);
                listener.AttachTo(label);

                return label.Content;
            }

            throw new NotSupportedException("{0} is not supported yet when using the LabelFor extension.");
        }
    }
}