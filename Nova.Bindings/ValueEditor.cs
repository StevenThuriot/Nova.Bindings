﻿#region License
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Nova.Bindings.Metadata;

namespace Nova.Bindings
{
    [TemplatePart(Name = ValueEditorPart)]
    public class ValueEditor : Control, IHaveSettings
    {
        private const string ValueEditorPart = "PART_ValueEditor";


        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            "Settings",
            typeof (IDefinition),
            typeof (ValueEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.NotDataBindable, OnSettingsChanged));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof (object),
            typeof (ValueEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.Register("Command", typeof (ICommand), typeof (ValueEditor));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof (object), typeof (ValueEditor));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public IDefinition Settings
        {
            get { return (IDefinition) GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var editor = (ValueEditor) dependencyObject;

            var command = editor.Command;
            if (command == null) return;

            var parameter = editor.CommandParameter;
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        private static void OnSettingsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var settings = (IDefinition) e.NewValue;
            var editor = (ValueEditor)dependencyObject;

            Contract.Ensures(EnsureTemplate(settings), "Template does not match settings.");

            if (settings == null)
            {
                editor.Template = null;
            }
            else
            {
                editor.Template = (ControlTemplate) editor.FindResource(settings.Editor);
            }
        }

        private static bool EnsureTemplate(IDefinition settings)
        {
            if (settings == null) return true;
            
            switch (settings.Editor)
            {
                case Definitions.ValueComboBoxEditor:
                    return settings is IComboBoxDefinition;

                case Definitions.ValueRadioButtonEditor:
                    return settings is IRadioButtonDefinition;

                default:
                    return true;
            }
        }


        /// <summary>
        /// StartUp event handler for applications to use.
        /// Merges the ValueEditor's resource dictionaries into the application's resource dictionary.
        /// </summary>
        /// <param name="sender">The application</param>
        /// <param name="startupEventArgs">Start up event arguments. These aren't used.</param>
        public static void Initialize(object sender, StartupEventArgs startupEventArgs)
        {
            var app = sender as Application;
            if (app == null) return;

            app.Startup -= Initialize;

            var resourceLocator = new Uri("pack://application:,,,/Nova.Bindings;component/ValueEditor.xaml", UriKind.RelativeOrAbsolute);
            var dictionary = (ResourceDictionary)Application.LoadComponent(resourceLocator);
            app.Resources.MergedDictionaries.Add(dictionary);
        }

        public static class Definitions
        {
            public const string ValueTextEditor = "ValueTextEditor";
            public const string ValueCheckBoxEditor = "ValueCheckBoxEditor";
            public const string ValueRadioButtonEditor = "ValueRadioButtonEditor";
            public const string ValueComboBoxEditor = "ValueComboBoxEditor";
        }
    }
}