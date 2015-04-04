using System.Collections;

namespace Nova.Bindings.Metadata
{
    public interface IComboBoxDefinition : IDefinition
    {
        IEnumerable ItemsSource { get; }
    }
}