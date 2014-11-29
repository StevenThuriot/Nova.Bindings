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
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Nova.Bindings.Metadata;

namespace Nova.Bindings.DefaultDefinitions
{
    [DebuggerDisplay("ID: {Id} ~ Label: {Label} ~ Editor: {Editor} ~ Items: {ItemsSource}")]
    public class ComboBoxDefinition : Definition, IComboBoxDefinition
    {
        public ComboBoxDefinition(string id, string label, IEnumerable items)
            : base(id, label, ValueEditor.Definitions.ValueComboBoxEditor)
        {
            Contract.Requires<ArgumentNullException>(items != null, "Items can't be null.");

            ItemsSource = items;
        }

        public IEnumerable ItemsSource { get; private set; }
    }
}