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

namespace Nova.Bindings.Metadata
{
    [ContractClass(typeof (DynamicPropertySettingsContract))]
    public interface IHaveDynamicPropertySettings
    {
        string ProvideDynamicSettings(string field);
    }


    [ContractClassFor(typeof (IHaveDynamicPropertySettings))]
    public abstract class DynamicPropertySettingsContract : IHaveDynamicPropertySettings
    {
        public string ProvideDynamicSettings(string field)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(field), "The property name can't be null, empty or whitespace.");
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()), "Dynamic settings need to return a valid result");

            return default(string);
        }
    }
}