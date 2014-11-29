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
