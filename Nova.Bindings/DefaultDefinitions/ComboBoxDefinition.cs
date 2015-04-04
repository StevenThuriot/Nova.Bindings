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