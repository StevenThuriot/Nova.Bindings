namespace Nova.Bindings.Metadata
{
    public interface IDefinition
    {
        string Id { get; }
        string Label { get; }
        string Editor { get; }
    }
}