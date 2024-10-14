// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.ArgumentCompleters.Attributes;
using Mercury.PowerShell.Hooks.ComplexTypes;
using Mercury.PowerShell.Hooks.Enums;
using Mercury.PowerShell.Hooks.Exceptions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to get a proxy hook.
/// </summary>
[OutputType(typeof(HookStore), typeof(HookItem))]
[Cmdlet(VerbsCommon.Get, "ProxyHook")]
public sealed class GetProxyHookCmdlet : PSCmdlet {
  /// <summary>
  ///   The type of the hook.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required HookType Type { get; init; }

  /// <summary>
  ///   The identifier of the hook.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [HookStoreIdentifierCompleter]
  public string? Identifier { get; init; }

  /// <inheritdoc />
  protected override void ProcessRecord() {
    var hookVariableName = Type switch {
      HookType.ChangeWorkingDirectory => HookVariableNames.ChangeWorkingDirectory,
      HookType.PrePrompt => HookVariableNames.PrePrompt,
      var _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, "The hook type is not valid.")
    };

    var hookVariable = SessionState.PSVariable.Get(hookVariableName);

    if (hookVariable?.Value is not HookStore hookStore) {
      return;
    }

    if (!string.IsNullOrWhiteSpace(Identifier)) {
      if (hookStore.Items.Select(item => item.Identifier).Contains(Identifier)) {
        var item = hookStore.Items.FirstOrDefault(item => item.Identifier == Identifier);

        WriteObject(item);
        return;
      }

      WriteError(HookStoreIdentifierNotFoundException.ToRecord(Type, Identifier));
      return;
    }

    WriteObject(hookStore);
  }
}
