Function Set-TfsRegistryValue
{
    [CmdletBinding()]
    [OutputType([Microsoft.TeamFoundation.Framework.Client.RegistryEntry])]
    Param
    (
        [Parameter(Position=0, Mandatory=$true, ValueFromPipelineByPropertyName='Path')]
        [string]
        $Path,

        [Parameter(Position=1, Mandatory=$true)]
        [object]
        $Value,

        [Parameter(Mandatory=$true)]
        [ValidateSet('Server', 'Collection', 'Project')]
        $Scope,

        [Parameter()]
        [object]
        $Target
    )

    Process
    {
        Write-Verbose "Set-TfsRegistryValue: Setting value '$Value' to '$Path', scoped to '$Scope'"
        
        switch ($Scope)
        {
            'Server' {
                $srv = Get-TfsConfigurationServer -Server $Target
                $regsvc = $srv.GetService([type]'Microsoft.TeamFoundation.Framework.Client.ITeamFoundationRegistry')
            }
            'Collection' {
                $tpc = Get-TfsTeamProjectCollection -Collection $Target
                $regsvc = $tpc.GetService([type]'Microsoft.TeamFoundation.Framework.Client.ITeamFoundationRegistry')
            }
            'Project' {
                throw 'Not implemented.'
            }
        }

        $regsvc.SetValue($Path, $Value)
    }
}