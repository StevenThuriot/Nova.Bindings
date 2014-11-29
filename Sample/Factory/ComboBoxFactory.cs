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

namespace Sample.Factory
{
    public class ComboBoxFactory : DefinitionFactory
    {
        protected override IDefinition CreateDefinition(string id)
        {
            if (id != Constants.Country) return null;

            var label = "Definition for " + id; //Retrieve from some sort of database.
            
            var items = new List<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4"
            };

            return new ComboBoxDefinition(id, label, items);
        }
    }
}