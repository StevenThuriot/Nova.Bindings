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
using Nova.Bindings.Metadata;

namespace Nova.Bindings.DefaultDefinitions
{
    public abstract class DefinitionFactory
    {
        protected DefinitionFactory _successor;

        public DefinitionFactory SetSuccessor(DefinitionFactory successor)
        {
            return _successor = successor;
        }

        public DefinitionFactory SetSuccessor<T>()
            where T : DefinitionFactory, new()
        {
            return _successor = new T();
        }

        public IDefinition Create(string id)
        {
            var definition = CreateDefinition(id);

            if (definition != null)
                return definition;

            if (_successor != null)
            {
                definition = _successor.Create(id);

                if (definition != null)
                    return definition;
            }

            throw new NotSupportedException(id);
        }

        protected abstract IDefinition CreateDefinition(string id);
    }
}
