Function Get-TfsRegistryValue
{
    [CmdletBinding()]
    [OutputType([Microsoft.TeamFoundation.Framework.Client.RegistryEntry])]
    Param
    (
        [Parameter(Position=0, Mandatory=$true)]
        [SupportsWildcards()]
        [string]
        $Path,

        [Parameter(Mandatory=$true)]
        [ValidateSet('Server', 'Collection', 'Project')]
        $Scope,

        [switch]
        $Recurse,

        [switch]
        $IncludeContainers,

        [Parameter()]
        [object]
        $Target
    )

    Process
    {
        Write-Verbose "Get-TfsRegistryValue: Getting value from '$Path', scoped to '$Scope'"

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

            }
        }

        if ($Recurse -and (-not $Path.EndsWith('/*')))
        {
            Write-Verbose "Get-TfsRegistryValue: Changing starting path '$Path' to perform recursive searches"

            $Path = ($Path + '/*') -replace '//', '/'

            Write-Verbose "Get-TfsRegistryValue: New path is '$Path'"
        }

        $entries = $regsvc.ReadEntries($Path, $true)

        if (($entries.Count -gt 1) -and (-not $Path.EndsWith('/*')))
        {
            Write-Verbose "Get-TfsRegistryValue: Changing starting path '$Path' to enumerate container"
            
            $Path = ($Path + '/*') -replace '//', '/'

            Write-Verbose "Get-TfsRegistryValue: New path is '$Path'"

            $entries = $regsvc.ReadEntries($Path, $true)
        }
        
        Write-Verbose "Get-TfsRegistryValue: $($entries.count) entries found"
        
        $entries | Select-Object -Skip 1 | ForEach-Object { 

            Write-Verbose "Get-TfsRegistryValue: Processing item '$($_.Path)'"

            if (($_.Value -ne $null) -or $IncludeContainers)
            {
                Write-Verbose "Get-TfsRegistryValue: Outputting '$($_.Path)'"
                Write-Output $_ 
            }

            if ($Recurse)
            {
                Write-Verbose "Get-TfsRegistryValue: Recursing path $($_.Path)/*"
                Get-TfsRegistryValue -Path "$($_.Path)/*" -Scope $Scope -Target $Target -Recurse -IncludeContainers:$IncludeContainers
            }
        }
    }
}