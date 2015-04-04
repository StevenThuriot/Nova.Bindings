using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;

namespace Sample.Factory
{
    public class RadioButtonFactory : DefinitionFactory
    {
        protected override IDefinition CreateDefinition(string id)
        {
            if (id != Constants.Male && id != Constants.Female) return null;


            var label = "Definition for " + id; //Retrieve from some sort of database.
            const string groupname = "gender"; //Also retrieve;

            return new RadioButtonDefinition(id, label, groupname);
        }
    }
}