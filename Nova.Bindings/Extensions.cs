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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Nova.Bindings.Metadata;

namespace Nova.Bindings
{
    static class Extensions
    {
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        public static T GetAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            Contract.Requires<ArgumentNullException>(member != null, "The supplied member info can't be null.");

            return (T) member.GetCustomAttributes(typeof (T), true).FirstOrDefault();
        }


        public static ISettingsManager FindSettingsManager(this DependencyObject dependencyObject)
        {
            Contract.Requires<ArgumentNullException>(dependencyObject != null, "The supplied dependency object can't be null.");

            while (dependencyObject != null)
            {
                var datacontext = dependencyObject.GetDatacontext();

#if INFRAGISTICS
                DependencyObject parent = null;
#endif
                if (datacontext != null)
                {

                    var viewModel = datacontext as IHaveSettingsManager;

                    if (viewModel != null) return viewModel.SettingsManager;
#if INFRAGISTICS
                    var datarecord = datacontext as Infragistics.Windows.DataPresenter.DataRecord;

                    if (datarecord != null)
                    {
                        parent = datarecord.DataPresenter;//Infragistics DataGrid --> Shortcut
                    }
#endif
                }

                dependencyObject = 
#if INFRAGISTICS
                    parent ??
#endif
                    VisualTreeHelper.GetParent(dependencyObject);
            }

            throw new NotSupportedException("Unable to find SettingsManager.");
        }

        public static object GetDatacontext(this DependencyObject dependencyObject)
        {
            Contract.Requires<ArgumentNullException>(dependencyObject != null, "The supplied dependency object can't be null.");

            var dataContext = dependencyObject.GetValue(FrameworkElement.DataContextProperty);

            if (dataContext == null)
                return null;

            if (dataContext == BindingOperations.DisconnectedSource)
                //Virtualized item that just got disconnected. Ignore this.
                //Pre-4.5 solutions need to use:
                //  if (dataContext.ToString().Equals("{DisconnectedItem}", StringComparison.OrdinalIgnoreCase))
                return null;

            return dataContext;
        }
    }
}
