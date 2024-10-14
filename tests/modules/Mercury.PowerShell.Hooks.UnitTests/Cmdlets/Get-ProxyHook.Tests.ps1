Import-Module "Pester"


Describe "Get-ProxyHook" {
  BeforeAll {
    Import-Module "$PSScriptRoot/../Mercury.PowerShell.Hooks.psd1"
  }

  It "Should return the proxy hook" {
    $createdHook = Register-ProxyHook -Type PrePrompt -Identifier "TestHook" -Action {
      Write-Host "The current location is: $PWD"
    } -PassThru

    $hook = Get-ProxyHook -Type PrePrompt -Identifier "TestHook"
    $hook | Should -BeOfType Mercury.PowerShell.Hooks.ComplexTypes.HookItem
    $hook.Identifier | Should -Be $createdHook.Identifier
    $hook.Type | Should -Be $createdHook.Type
    $hook | Should -Be $createdHook
  }

  It "Should return message if the hook does not exist" {
    $hook = Get-ProxyHook -Type PrePrompt -Identifier "NonExistentHook"
    $hook | Should -Be "The hook with identifier 'NonExistentHook' was not found in the 'PrePrompt' hook store."
  }
}
