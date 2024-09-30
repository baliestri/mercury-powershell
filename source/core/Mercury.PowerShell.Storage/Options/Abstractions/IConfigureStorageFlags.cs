// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using SQLite;

namespace Mercury.PowerShell.Storage.Options.Abstractions;

/// <summary>
///   Defines a contract for configuring the storage flags.
/// </summary>
public interface IConfigureStorageFlags : IConfigureStorageTables {
  /// <summary>
  ///   Sets the flags to use when opening the connection to the storage.
  /// </summary>
  /// <param name="flags">The flags to use.</param>
  /// <returns>The next builder.</returns>
  /// <remarks>
  ///   If not used, the default flags are <see cref="SQLiteOpenFlags.Create" />, <see cref="SQLiteOpenFlags.ReadWrite" />, and
  ///   <see cref="SQLiteOpenFlags.SharedCache" />.
  /// </remarks>
  IConfigureStorageTables WithFlags(SQLiteOpenFlags flags);
}
