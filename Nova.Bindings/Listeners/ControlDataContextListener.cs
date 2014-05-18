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
