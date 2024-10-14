// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;
using Mercury.PowerShell.Hooks.Cmdlets;
using Mercury.PowerShell.Hooks.ComplexTypes;
using Mercury.PowerShell.Hooks.Enums;

namespace Mercury.PowerShell.Hooks.ArgumentCompleters;

internal sealed class HookStoreIdentifierArgumentCompleter : IArgumentCompleter {
  /// <inheritdoc />
  public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst,
  IDictionary fakeBoundParameters) {
    if (!fakeBoundParameters.Contains("Type")) {
      return [];
    }

    var currentType = fakeBoundParameters["Type"] as string;

    if (string.IsNullOrEmpty(currentType)) {
      return [];
    }

    if (!Enum.TryParse<HookType>(currentType, true, out var enumCurrentType)) {
      return [];
    }

    HookVariableNames currentTypeName = enumCurrentType;
    var variable = RegisterProxyHookCmdlet._hookVariables[currentTypeName];

    return variable.Value is not HookStore hookStore
      ? []
      : hookStore.Items.Select(item => new CompletionResult(item.Identifier));
  }
}
