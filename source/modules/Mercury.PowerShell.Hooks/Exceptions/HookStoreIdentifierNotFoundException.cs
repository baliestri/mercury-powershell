// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.Enums;

namespace Mercury.PowerShell.Hooks.Exceptions;

/// <summary>
///   Exception thrown when a hook identifier is not found in the hook store.
/// </summary>
/// <param name="storeType">The type of the hook store.</param>
/// <param name="identifier">The identifier of the hook.</param>
public sealed class HookStoreIdentifierNotFoundException(HookType storeType, string identifier)
  : Exception($"The hook with identifier '{identifier}' was not found in the '{storeType}' hook store.") {
  private const string ERROR_ID = "HookStoreIdentifierNotFound";

  /// <summary>
  ///   Creates an <see cref="ErrorRecord" /> from the exception.
  /// </summary>
  /// <param name="storeType">The type of the hook store.</param>
  /// <param name="identifier">The identifier of the hook.</param>
  /// <returns>The <see cref="ErrorRecord" /> created from the exception.</returns>
  public static ErrorRecord ToRecord(HookType storeType, string identifier) {
    var exception = new HookStoreIdentifierNotFoundException(storeType, identifier);

    return new ErrorRecord(exception, ERROR_ID, ErrorCategory.ObjectNotFound, identifier);
  }
}
