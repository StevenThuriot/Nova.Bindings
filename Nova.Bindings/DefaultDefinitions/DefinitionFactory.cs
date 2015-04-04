using System;
using Nova.Bindings.Metadata;

namespace Nova.Bindings.DefaultDefinitions
{
    public abstract class DefinitionFactory
    {
        protected DefinitionFactory _successor;

        public DefinitionFactory SetSuccessor(DefinitionFactory successor)
        {
            return _successor = successor;
        }

        public DefinitionFactory SetSuccessor<T>()
            where T : DefinitionFactory, new()
        {
            return _successor = new T();
        }

        public IDefinition Create(string id)
        {
            var definition = CreateDefinition(id);

            if (definition != null)
                return definition;

            if (_successor != null)
            {
                definition = _successor.Create(id);

                if (definition != null)
                    return definition;
            }

            throw new NotSupportedException(id);
        }

        protected abstract IDefinition CreateDefinition(string id);
    }
}
