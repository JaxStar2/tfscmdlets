namespace TfsCmdlets.Core.Adapters
{
    public interface ITeamFoundationIdentityAdapter: IAdapter
    {
        string DisplayName { get; }
        string UniqueName { get; }
        bool IsActive { get; }
        bool IsContainer { get; }
        object Descriptor { get; }
    }
}