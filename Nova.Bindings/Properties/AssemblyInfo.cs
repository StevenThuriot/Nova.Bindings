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
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: AssemblyCopyright("Thuriot.be")]
[assembly: AssemblyDescription("Model first design of WPF views through binding.")]
[assembly: AssemblyCompany("Steven Thuriot")]
[assembly: NeutralResourcesLanguage("en")]

[assembly: AssemblyTitle("Nova.Bindings")]
[assembly: AssemblyProduct("Nova.Bindings")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("da04f51d-6608-43ff-ac7f-f0002f797731")]

[assembly: XmlnsDefinition("http://nova.thuriot.be/Bindings", "Nova.Bindings")]
[assembly: XmlnsPrefix("http://nova.thuriot.be/Bindings", "NovaBindings")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Nova.Bindings")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]