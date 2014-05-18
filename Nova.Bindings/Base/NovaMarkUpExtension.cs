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
using System.Windows.Markup;

namespace Nova.Bindings.Base
{
    public abstract class NovaMarkUpExtension : MarkupExtension
    {
        private readonly PropertyPath _path;

        protected NovaMarkUpExtension(PropertyPath path)
        {
            Contract.Requires<ArgumentNullException>(path != null, "The supplied path can't be null.");
            Contract.Requires<ArgumentNullException>(path.Path != ".", "The supplied path can't '.'.");

            _path = path;
        }

        protected PropertyPath Path
        {
            get { return _path; }
        }

        public override sealed object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget) serviceProvider.GetService(typeof (IProvideValueTarget));

            //The markup expression is evaluated when it is encountered by the XAML parser : 
            //      in that case, when the template is parsed. But at this time, 
            //      the control is not created yet, so the ProvideValue method can’t access it...

            //When a markup extension is evaluated inside a template, 
            //the TargetObject is actually an instance of System.Windows.SharedDp, an internal WPF class.

            //For the markup extension to be able to access its target, 
            //it has to be evaluated when the template is applied : we need to defer its evaluation until this time.

            //It’s actually pretty simple, we just need to return the markup extension itself from ProvideValue:
            //this way, it will be evaluated again when the actual target control is created.

            // http://www.thomaslevesque.com/2009/08/23/wpf-markup-extensions-and-templates/
            if (target == null || target.TargetObject.GetType().FullName == "System.Windows.SharedDp")
                return this;


            return ProvideValue(serviceProvider, target);
        }

        protected abstract object ProvideValue(IServiceProvider serviceProvider, IProvideValueTarget target);
    }
}