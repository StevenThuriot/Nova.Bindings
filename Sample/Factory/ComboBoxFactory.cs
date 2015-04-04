using System.Collections.Generic;
using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;

namespace Sample.Factory
{
    public class ComboBoxFactory : DefinitionFactory
    {
        protected override IDefinition CreateDefinition(string id)
        {
            if (id != Constants.Country) return null;

            var label = "Definition for " + id; //Retrieve from some sort of database.
            
            var items = new List<string>
            {
                "Austria",
                "Belgium",
                "Norway",
                "The Netherlands"
            };

            return new ComboBoxDefinition(id, label, items);
        }
    }
}