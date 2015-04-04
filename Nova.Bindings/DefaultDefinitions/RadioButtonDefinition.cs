using System.Diagnostics;
using Nova.Bindings.Metadata;

namespace Nova.Bindings.DefaultDefinitions
{
    [DebuggerDisplay("ID: {Id} ~ Label: {Label} ~ Editor: {Editor} ~ Group: {GroupName}")]
    public class RadioButtonDefinition : Definition, IRadioButtonDefinition
    {
        public RadioButtonDefinition(string id, string label, string groupName)
            : base(id, label, ValueEditor.Definitions.ValueRadioButtonEditor)
        {
            GroupName = groupName;
        }

        public string GroupName { get; private set; }
    }
}