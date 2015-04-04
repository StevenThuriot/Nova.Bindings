using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;

namespace Sample.Factory
{
    public class TextBoxFactory : DefinitionFactory
    {
        protected override IDefinition CreateDefinition(string id)
        {
            //Since this is our catch all, don't bother checking the id and just return our definition.

            var label = "Definition for " + id; //Retrieve from some sort of database.
            return new Definition(id, label);
        }
    }
}