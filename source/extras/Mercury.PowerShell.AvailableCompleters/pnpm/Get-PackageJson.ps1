# Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
# See the LICENSE file in the project root for full license information.

[CmdletBinding()]
[OutputType([string[]])]
param(
  [Parameter(
    Mandatory = $true,
    Position = 0,
    ValueFromPipeline = $true,
    ValueFromPipelineByPropertyName = $true,
    ParameterSetName = "Binaries"
  )]
  [switch] $Binaries,
  [Parameter(
    Mandatory = $true,
    Position = 0,
    ValueFromPipeline = $true,
    ValueFromPipelineByPropertyName = $true,
    ParameterSetName = "Scripts"
  )]
  [switch] $Scripts
)

begin {
  $CWD = Get-Location
  [string[]]$Output = @()
}

process {
  if ($Binaries) {
    $NodeModulesBinariesPath = Join-Path $CWD "node_modules" ".bin"

    if (-not (Test-Path $NodeModulesBinariesPath -PathType Container)) {
      return $Output
    }

    $NodeModulesBinariesPath = Resolve-Path $NodeModulesBinariesPath

    Get-ChildItem $NodeModulesBinariesPath | Where-Object {
      $_.Name -notmatch "^*\."
    } | ForEach-Object {
      $Output += "{$($_.Name)}:$($_.Name)"
    }
  }

  if ($Scripts) {
    $PackageJSONPath = Join-Path $CWD "package.json"

    if (-not (Test-Path $PackageJSONPath -PathType Leaf)) {
      return $Output
    }

    $PackageJSONPath = Resolve-Path $PackageJSONPath
    $PackageJSON = Get-Content $PackageJSONPath | ConvertFrom-Json

    if ((-not $PackageJSON) -or (-not $PackageJSON.scripts)) {
      return $Output
    }

    $PackageJSONScripts = $PackageJSON.scripts | ConvertTo-Json | ConvertFrom-Json -AsHashtable
    $PackageJSONScripts.Keys | ForEach-Object {
      $Output += "{$($_)}:$($PackageJSONScripts[$_])"
    }
  }
}

end {
  return $Output | Sort-Object -Unique
}
