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
