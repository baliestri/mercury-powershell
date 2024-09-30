// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.Storage.Options.Abstractions;

/// <summary>
///   Defines a contract for configuring the storage name.
/// </summary>
public interface IConfigureStorageName {
  /// <summary>
  ///   Sets the name of the storage.
  /// </summary>
  /// <param name="name">The name of the storage.</param>
  /// <returns>The next builder.</returns>
  IConfigureStorageFlags WithName(string name);
}
