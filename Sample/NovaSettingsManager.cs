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

using System.Collections.Generic;
using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;
using Sample.Factory;

namespace Sample
{
    public class NovaSettingsManager : ISettingsManager
    {
        private readonly Dictionary<string, IDefinition> _definitions;
        private readonly DefinitionFactory _factory;

        public NovaSettingsManager()
        {
            _definitions = new Dictionary<string, IDefinition>();

            //TODO;
            _factory = new ComboBoxFactory(); //Good starter since it's a special case.
            _factory.SetSuccessor<RadioButtonFactory>()
                .SetSuccessor<CheckBoxFactory>()
                .SetSuccessor<DatePickerFactory>()
                //.......... 
                .SetSuccessor<TextBoxFactory>(); //Decent Fallback in case nothing matches.
        }

        public IDefinition GetDefinition(string id)
        {
            IDefinition definition;

            if (_definitions.TryGetValue(id, out definition))
            {
                return definition;
            }

            definition = _factory.Create(id);
            _definitions.Add(id, definition);
            return definition;
        }
    }
}