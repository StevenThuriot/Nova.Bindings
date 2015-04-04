using Nova.Bindings;
using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;

namespace Sample.Factory
{
    class DatePickerFactory : DefinitionFactory
    {
        protected override IDefinition CreateDefinition(string id)
        {
            if (id != Constants.DayOfBirth) return null;

            var label = "Definition for " + id; //Retrieve from some sort of database.
            return new Definition(id, label, ValueEditor.Definitions.ValueDatePickerEditor);
        }
    }
}
