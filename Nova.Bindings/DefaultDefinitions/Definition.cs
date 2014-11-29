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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Nova.Bindings.Metadata;

namespace Nova.Bindings.DefaultDefinitions
{
    [DebuggerDisplay("ID: {Id} ~ Label: {Label} ~ Editor: {Editor}")]
    public class Definition : IDefinition
    {
        public Definition(string id, string label, string editor = ValueEditor.Definitions.ValueTextEditor)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(id), "Id can't be null, empty or whitespace.");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(editor), "Editor can't be null, empty or whitespace.");

            Id = id;
            Label = label;
            Editor = editor;
        }

        public string Id { get; private set; }
        public string Label { get; private set; }
        public string Editor { get; private set; }
    }
}
