[DscResource]
class TfsTeamProjectCollection
{
    [DscProperty(Key = $true)]
    [string] $Name

    [DscProperty(Mandatory = true)]
    public Ensure Ensure { get; set; } = Ensure.Present;

    [DscProperty(Mandatory = true)]
    public Uri ServerUrl { get; set; }

    [DscProperty]
    public string Description { get; set; }

    [DscProperty()]
    public string DatabaseServer { get; set; }

    [DscProperty]
    public string DatabaseName { get; set; }

    [DscProperty]
    public string ConnectionString { get; set; }

    [DscProperty]
    public bool Default { get; set; }

    [DscProperty]
    public bool UseExistingDatabase { get; set; }

    [DscProperty]
    public ServiceHostStatus InitialState { get; set; } = ServiceHostStatus.Started;

    [DscProperty(NotConfigurable = true)]
    public Uri CollectionUrl { get; set; }

    public TfsTeamProjectCollection Get()
    {
        return this;
    }

    public bool Test()
    {
        return false;
    }

    public void Set()
    {

    }
}
