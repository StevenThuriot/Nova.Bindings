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
