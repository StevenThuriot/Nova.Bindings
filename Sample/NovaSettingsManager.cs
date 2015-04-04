using System.Collections.Generic;
using Nova.Bindings.DefaultDefinitions;
using Nova.Bindings.Metadata;
using Sample.Factory;

namespace Sample
{
    public class NovaSettingsManager : ISettingsManager
    {
        private readonly Dictionary<string, IDefinition> _definitions;
        private readonly DefinitionFactory _factory;

        public NovaSettingsManager()
        {
            _definitions = new Dictionary<string, IDefinition>();

            //TODO;
            _factory = new ComboBoxFactory(); //Good starter since it's a special case.
            _factory.SetSuccessor<RadioButtonFactory>()
                .SetSuccessor<CheckBoxFactory>()
                .SetSuccessor<DatePickerFactory>()
                //.......... 
                .SetSuccessor<TextBoxFactory>(); //Decent Fallback in case nothing matches.
        }

        public IDefinition GetDefinition(string id)
        {
            IDefinition definition;

            if (_definitions.TryGetValue(id, out definition))
            {
                return definition;
            }

            definition = _factory.Create(id);
            _definitions.Add(id, definition);
            return definition;
        }
    }
}