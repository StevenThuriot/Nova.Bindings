using System;
using System.Diagnostics.Contracts;

namespace Nova.Bindings.Metadata
{
    [ContractClass(typeof(SettingsManagerContract))]
    public interface ISettingsManager
    {
        IDefinition GetDefinition(string id);
    }

    [ContractClassFor(typeof(ISettingsManager))]
    public abstract class SettingsManagerContract : ISettingsManager
    {
        public IDefinition GetDefinition(string id)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(id), "The definition id can't be null, empty or whitespace.");
            Contract.Ensures(Contract.Result<IDefinition>() != null, "This method is required to supply a valid result.");

            return default (IDefinition);
        }
    }
}