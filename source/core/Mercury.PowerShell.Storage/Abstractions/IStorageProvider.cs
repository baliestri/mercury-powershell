// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using SQLite;

namespace Mercury.PowerShell.Storage.Abstractions;

/// <summary>
///   Provides an easy way to access the storage provider.
/// </summary>
public interface IStorageProvider {
  /// <summary>
  ///   The connection to the SQLite database.
  /// </summary>
  SQLiteAsyncConnection Connection { get; }
}
