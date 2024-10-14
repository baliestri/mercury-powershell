// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.ComplexTypes;
using Mercury.PowerShell.Hooks.Enums;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to register a proxy hook.
/// </summary>
[OutputType(typeof(HookItem))]
[Cmdlet(VerbsLifecycle.Register, "ProxyHook")]
public sealed class RegisterProxyHookCmdlet : PSCmdlet {
  internal static readonly Dictionary<string, PSVariable> _hookVariables = new();

  /// <summary>
  ///   The type of the hook.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required HookType Type { get; init; }

  /// <summary>
  ///   The unique identifier of the hook.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required string Identifier { get; init; }

  /// <summary>
  ///   The action to be executed when the hook is triggered.
  /// </summary>
  [Parameter(Position = 2, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required ScriptBlock Action { get; init; }

  /// <summary>
  ///   Passes an object representing the hook to the pipeline. By default, this cmdlet does not generate any output.
  /// </summary>
  [Parameter(Position = 3, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public SwitchParameter PassThru { get; init; }

  /// <inheritdoc />
  protected override void BeginProcessing() {
    var availableHooks = new[] {
      HookVariableNames.ChangeWorkingDirectory,
      HookVariableNames.PrePrompt
    };

    foreach (var availableHook in availableHooks) {
      var hooksVariable = SessionState.PSVariable.Get(availableHook) ??
                          new PSVariable(availableHook, HookStore.Empty(availableHook), ScopedItemOptions.Private);

      if (_hookVariables.TryGetValue(availableHook, out var existingVariable)) {
        if (existingVariable.Value is not HookStore existingVariableStore ||
            hooksVariable.Value is not HookStore hooksVariableStore) {
          throw new InvalidOperationException("The hook store is not valid.");
        }

        if (existingVariableStore != hooksVariableStore) {
          _hookVariables.Remove(availableHook);
        }
      }

      _hookVariables.TryAdd(availableHook, hooksVariable);
    }
  }

  /// <inheritdoc />
  protected override void ProcessRecord() {
    HookVariableNames entryName = Type;

    if (!_hookVariables.TryGetValue(entryName, out var entry)) {
      throw new InvalidOperationException("The hook store is not valid.");
    }

    if (entry.Value is not HookStore hookStore) {
      throw new InvalidOperationException("The hook store is not valid.");
    }

    var hookItem = HookItem.Create(Identifier, Action);

    if (!hookStore.Items.Add(hookItem)) {
      WriteObject($"The hook with identifier '{Identifier}' already exists in the '{Type}' hook store.");
    }

    if (PassThru.IsPresent) {
      WriteObject(hookItem);
    }
  }

  /// <inheritdoc />
  protected override void EndProcessing() {
    foreach (var variable in _hookVariables.Values) {
      SessionState.PSVariable.Set(variable);
    }
  }
}
