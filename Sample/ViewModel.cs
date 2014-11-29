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
using System.Net.Mime;
using Nova.Bindings.Metadata;

namespace Sample
{
    public class ViewModel : IHaveSettingsManager,
        IHaveDynamicPropertySettings
    {
        public ISettingsManager SettingsManager { get; private set; }

        public ViewModel()
        {
            SettingsManager = new NovaSettingsManager();

            TextField = "Random Text";
            RadioButtonField1 = false;
            RadioButtonField2 = true;
            CheckBoxField = true;
            ComboBoxField = "Item 2";
            DatePickerField = DateTime.Now;
            DynamicField = "Dynamic Text";
            RandomField = "I have identity issues";
        }

        [Settings(Constants.Name)]
        public string TextField { get; set; }
        
        [Settings(Constants.Male)]
        public bool RadioButtonField1 { get; set; }

        [Settings(Constants.Female)]
        public bool RadioButtonField2 { get; set; }

        [Settings(Constants.Married)]
        public bool CheckBoxField { get; set; }

        [Settings(Constants.Country)]
        public string ComboBoxField { get; set; }

        [Settings(Constants.DayOfBirth)]
        public DateTime DatePickerField { get; set; }
        
        [DynamicSettings]
        public string DynamicField { get; set; }

        [Settings("Unknown")] //Catch-All
        public string RandomField { get; set; }




        public string ProvideDynamicSettings(string field)
        {
            if (field == "DynamicField")
                return Constants.DynamicName;

            return null; //TODO; Throw exception or return catch all.
        }
    }
}
