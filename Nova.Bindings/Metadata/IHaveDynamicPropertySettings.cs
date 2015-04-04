using System;
using System.Diagnostics.Contracts;

namespace Nova.Bindings.Metadata
{
    [ContractClass(typeof (DynamicPropertySettingsContract))]
    public interface IHaveDynamicPropertySettings
    {
        string ProvideDynamicSettings(string field);
    }


    [ContractClassFor(typeof (IHaveDynamicPropertySettings))]
    public abstract class DynamicPropertySettingsContract : IHaveDynamicPropertySettings
    {
        public string ProvideDynamicSettings(string field)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(field), "The property name can't be null, empty or whitespace.");
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()), "Dynamic settings need to return a valid result");

            return default(string);
        }
    }
}