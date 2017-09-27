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

        [object]
        $Filter,

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
                throw 'Not implemented.'
            }
        }

        if ($Recurse)
        {
            if($Path -match '\*')
            {
                throw 'Invalid usage. When doing recursive searches, wildcards are not supported in the Path argument. Use the Filter argument instead to limit the results to the desired pattern.'
            }

            Write-Verbose "Get-TfsRegistryValue: Adding '/*' to '$Path' to perform recursive searches"

            $pathToTraverse = ($Path + '/*') -replace '/+', '/'
        }
        elseif ($Path -eq '/')
        {
            # Avoid using '/', otherwise it will traverse the whole registry tree
            $pathToTraverse = '/*'
        }
        else
        {
            $pathToTraverse = $Path    
        }

        $entries = $regsvc.ReadEntries($pathToTraverse, $true)

        if($entries.Count -eq 0)
        {
            Write-Verbose "No items found matching '$Path'. Exiting."
            return
        }

        Write-Verbose "Get-TfsRegistryValue: $($entries.Count) entries found"
        
        foreach($e in $entries)
        {
            Write-Verbose "Get-TfsRegistryValue: Processing item '$($e.Path)', with filter '$Filter'"

            if ($e.Value -ne $null)
            {
                if ((($Filter -is [string]) -and ($e.Path -notlike $Filter)) -or 
                    (($Filter -is [scriptblock]) -and (-not ($e.Path | ForEach-Object $Filter))))
                {
                    Write-Verbose "Get-TfsRegistryValue: '$($e.Path)' does not match filter '$Filter'. Skipping."
                }
                else
                {
                    Write-Verbose "Get-TfsRegistryValue: Outputting '$($e.Path)'"
                    Write-Output $e
                }
                continue
            }

            if ($Recurse -and ($e.Path -ne $Path))
            {
                Write-Verbose "Get-TfsRegistryValue: Recursing path $($e.Path)/*"
                Get-TfsRegistryValue -Path "$($e.Path)" -Scope $Scope -Target $Target -Filter $Filter -Recurse
            }
        }
    }
}