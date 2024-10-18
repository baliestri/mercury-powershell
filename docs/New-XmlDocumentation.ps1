[CmdletBinding()]
param (
  [ArgumentCompleter({
      $ChildDirectories = Get-ChildItem -Directory -Path $PSScriptRoot
      $ChildDirectories.Name
    })]
  [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
  [string]
  $Language,
  [ArgumentCompleter({
      $ModulesPath = Split-Path -Path $PSScriptRoot | Join-Path -ChildPath "source", "modules"

      $Modules = Get-ChildItem -Directory -Path $ModulesPath
      $Modules.Name
    })]
  [Parameter(Position = 1, Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
  [string]
  $Module,
  [Parameter(Position = 2, Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
  [string]
  $OutputDirectory
)

begin {
  if (-not (Test-Path -Path $OutputDirectory)) {
    Write-Error -Message "The output directory does not exist." -Category ObjectNotFound -TargetObject $OutputDirectory -ErrorId "OutputDirectoryNotFound"
    exit 1
  }
}

process {
  $PlatyPSModuleIsInstalled = Get-Module -Name "PlatyPS" -ListAvailable -ErrorAction SilentlyContinue

  if (-not $PlatyPSModuleIsInstalled) {
    Install-Module -Name "PlatyPS" -Scope CurrentUser -Force -AllowClobber
  }

  Import-Module -Name "PlatyPS" -Force -ErrorAction Stop

  $DocumentationDirectory = Join-Path -Path $PSScriptRoot -ChildPath $Language -AdditionalChildPath $Module
  $ModulePsd1 = Join-Path -Path $OutputDirectory -ChildPath "$Module.psd1"

  Import-Module $ModulePsd1 -ErrorAction Stop

  New-ExternalHelp -Path $DocumentationDirectory -OutputPath $OutputDirectory -Force -Encoding ([System.Text.Encoding]::UTF8)

  Remove-Module -Name $Module -Force -ErrorAction SilentlyContinue
}
