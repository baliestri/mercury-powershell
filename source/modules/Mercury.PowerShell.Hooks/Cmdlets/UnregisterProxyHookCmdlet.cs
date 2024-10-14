// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.ArgumentCompleters.Attributes;
using Mercury.PowerShell.Hooks.ComplexTypes;
using Mercury.PowerShell.Hooks.Enums;
using Mercury.PowerShell.Hooks.Exceptions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to unregister a proxy hook.
/// </summary>
[OutputType(typeof(HookItem))]
[Cmdlet(VerbsLifecycle.Unregister, "ProxyHook")]
public sealed class UnregisterProxyHookCmdlet : PSCmdlet {
  /// <summary>
  ///   The type of the hook.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required HookType Type { get; init; }

  /// <summary>
  ///   The unique identifier of the hook.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  [HookStoreIdentifierCompleter]
  public required string Identifier { get; init; }

  /// <summary>
  ///   Passes an object representing the hook to the pipeline. By default, this cmdlet does not generate any output.
  /// </summary>
  [Parameter]
  public SwitchParameter PassThru { get; init; }

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

    var item = hookStore.Items.FirstOrDefault(item => item.Identifier == Identifier);

    if (!hookStore.Items.Remove(item) &&
        PassThru.IsPresent) {
      WriteError(HookStoreIdentifierNotFoundException.ToRecord(Type, Identifier));

      return;
    }

    if (PassThru.IsPresent) {
      WriteObject(item);
    }
  }
}
