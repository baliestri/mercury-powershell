// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;

namespace Mercury.PowerShell.Hooks.ArgumentCompleters.Attributes;

/// <summary>
///   Attribute to complete the HookStore identifier.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class HookStoreIdentifierCompleterAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  /// <inheritdoc />
  public IArgumentCompleter Create()
    => new HookStoreIdentifierArgumentCompleter();
}
