// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.Storage.Options.Abstractions;

public interface IConfigureStorageOptions {
  /// <summary>
  ///   Finishes the building process.
  /// </summary>
  /// <returns>The storage options.</returns>
  IStorageOptions Done();
}
