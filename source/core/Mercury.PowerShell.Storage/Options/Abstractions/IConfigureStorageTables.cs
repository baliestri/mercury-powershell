// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.Storage.Options.Abstractions;

public interface IConfigureStorageTables : IConfigureStorageOptions {
  /// <summary>
  ///   Sets the tables to create in the storage.
  /// </summary>
  /// <param name="tables">The tables to create.</param>
  /// <returns>The next builder.</returns>
  IConfigureStorageOptions WithTables(IEnumerable<Type> tables);

  /// <summary>
  ///   Sets the tables to create in the storage.
  /// </summary>
  /// <param name="tables">The tables to create.</param>
  /// <returns>The next builder.</returns>
  IConfigureStorageOptions WithTables(params Type[] tables);
}
