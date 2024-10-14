# Mercury.PowerShell.Hooks

Inspired by the [z-shell][1] hooks, this module tries to add hook functionality to PowerShell. It is not a perfect solution, but it is a start.

## About

Currently, the module supports the following hooks:

- [x] `PrePrompt` **¹**
- [x] `ChangeWorkingDirectory` **²**
- [ ] `AddToHistory` **³**
- [ ] `Periodic` **³**
- [ ] ~~`PreExecution`~~ **⁴**

**1.** This one was implemented using a cmdlet proxy for `Out-Default` cmdlet. Firstly, an implementation was made that updated the terminal's
`prompt` function,however, this created a bug when using PSReadLine's autocomplete, so I opted for this alternative.

**2.** This one was implemented using a cmdlet proxy for `Set-Location`, `Push-Location`, and `Pop-Location` cmdlets. It is triggered when using the
already defined aliases for these cmdlets.

**3.** Need to investigate more about how to implement these hooks.

**4.** For now, this one was not possible to implement. Maybe if we implement a custom `PSHost`, but I need to investigate more about it.

**PS**: Those hooks does not work when using through a `.ps1` script, but can work if you use it like `Mercury.PowerShell.Hooks\Set-Location` or any
other hook.

## Installation

```powershell
Install-Module -Name Mercury.PowerShell.Hooks
```

## Usage

See the [Documentation][3] for more information.

**PS**: The `Identifier` parameter has argument completion, so you can use the `Tab` key to complete the identifier.

## License

This project is licensed under the MIT License - see the [LICENSE][2] file for details.

[1]: https://www.zsh.org/

[2]: ../../../LICENSE.md

[3]: Documentation/Mercury.PowerShell.Hooks.md
