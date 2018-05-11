# Initialize Shell

if ($Host.UI.RawUI.WindowTitle -eq "Team Foundation Server Shell")
{
    # SetConsoleColors
    $Host.UI.RawUI.BackgroundColor = "DarkMagenta"
    $Host.UI.RawUI.ForegroundColor = "White"
    Clear-Host

    # ShowBanner
    $module = Test-ModuleManifest -Path (Join-Path $PSScriptRoot 'TfsCmdlets.psd1')
    Write-Host "TfsCmdlets: $($module.Description)"
    Write-Host "Version $($module.PrivateData.Build)"
    Write-Host ""

    @'
Function Prompt
{
    Process
    {
        $tfsPrompt = ''
        if ($global:TfsServerConnection)
        {
            $tfsPrompt = $global:TfsServerConnection.Name
            if ($global:TfsTpcConnection)
            {
                $tfsPrompt += "/$($global:TfsTpcConnection.Name)"
            }
            if ($global:TfsProjectConnection)
            {
                $tfsPrompt += "/$($global:TfsProjectConnection.Name)"
            }
            if ($global:TfsTeamConnection)
            {
                $tfsPrompt += "/$($global:TfsTeamConnection.Name)"
            }
            $tfsPrompt = "[$tfsPrompt] "
        }
        "TFS ${tfsPrompt}$($executionContext.SessionState.Path.CurrentLocation)$('>' * ($nestedPromptLevel + 1)) "
    }
}
'@ | Invoke-Expression

}
