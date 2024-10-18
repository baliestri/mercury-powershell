@{
  GUID                 = "9081d461-19a6-4a03-bd21-0469a370dfe1"

  RootModule           = "Mercury.PowerShell.Hooks.dll"
  ModuleVersion        = "0.1.0.0"

  Author               = "Bruno Sales"
  Copyright            = "(c) Bruno Sales. All rights reserved."

  PowerShellVersion    = "7.0"
  CompatiblePSEditions = @("Core")

  FunctionsToExport    = @()
  CmdletsToExport      = @(
    "Get-ProxyHook",
    "Register-ProxyHook",
    "Unregister-ProxyHook",
    "Out-Default",
    "Pop-Location",
    "Push-Location",
    "Set-Location"
  )
  VariablesToExport    = @()
  AliasesToExport      = @()

  PrivateData          = @{
    PSData = @{
      Tags         = @("Mercury", "PowerShell", "Hooks")
      LicenseUri   = "https://github.com/baliestri/mercury-powershell/blob/main/LICENSE"
      ProjectUri   = "https://github.com/baliestri/mercury-powershell"
      IconUri      = ""
      ReleaseNotes = @"
"@
    }
  }
}
