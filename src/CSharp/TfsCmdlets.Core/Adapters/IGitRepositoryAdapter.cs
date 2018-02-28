namespace TfsCmdlets.Core.Adapters
{
    public interface IGitRepositoryAdapter: IAdapter
    {
        string Name { get; }
        object Id { get; }
    }
}