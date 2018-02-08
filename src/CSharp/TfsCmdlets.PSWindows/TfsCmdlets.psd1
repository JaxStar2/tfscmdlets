@{
    Author = 'Igor Abade V. Leite'
    CompanyName = 'Igor Abade V. Leite'
    Copyright = '(c) 2014 Igor Abade V. Leite. All rights reserved.'
    Description = 'TfsCmdlets - PowerShell Cmdlets for TFS and VSTS'
    RootModule = 'TfsCmdlets.PSWindows.dll'
    GUID = 'bd4390dc-a8ad-4bce-8d69-f53ccf8e4163'
    HelpInfoURI = 'https://github.com/igoravl/tfscmdlets/wiki/'
    ModuleVersion = '1.1'
    PowerShellVersion = '5.0'
    TypesToProcess = "TfsCmdlets.Types.ps1xml"
    FormatsToProcess = "TfsCmdlets.Format.ps1xml"
    ScriptsToProcess = 'Startup.ps1'
    #FileList = @(${FileList})

    PrivateData = @{ 
        Tags = @('TfsCmdlets', 'TFS', 'VSTS', 'PowerShell')
        Branch = '${BranchName}'
        Commit = '${Commit}'
        Build = '${BuildName}'
        PreRelease = '${PreRelease}'
        LicenseUri = 'https://raw.githubusercontent.com/igoravl/tfscmdlets/master/LICENSE.md'
        ProjectUri = 'https://github.com/igoravl/tfscmdlets/'
        IconUri = 'https://raw.githubusercontent.com/igoravl/tfscmdlets/master/TfsCmdlets/resources/TfsCmdlets_Icon_32.png'
        ReleaseNotes = 'See https://github.com/igoravl/tfscmdlets/wiki/ReleaseNotes' 
        TfsClientVersion = '${TfsOmNugetVersion}'
    }
}