using Nova.Bindings;
using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;

namespace Sample.Factory
{
    public class CheckBoxFactory : DefinitionFactory
    {
        protected override IDefinition CreateDefinition(string id)
        {
            if (id != Constants.Married) return null;

            var label = "Definition for " + id; //Retrieve from some sort of database.
            return new Definition(id, label, ValueEditor.Definitions.ValueCheckBoxEditor);
        }
    }
}