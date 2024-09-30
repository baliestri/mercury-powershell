// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using SQLite;

namespace Mercury.PowerShell.Storage.Options.Abstractions;

/// <summary>
///   Blueprint for storage options.
/// </summary>
public interface IStorageOptions {
  /// <summary>
  ///   The name of the storage.
  /// </summary>
  string Name { get; set; }

  /// <summary>
  ///   The flags to use when opening the storage.
  /// </summary>
  SQLiteOpenFlags OpenFlags { get; set; }

  /// <summary>
  ///   The tables to create in the storage.
  /// </summary>
  IEnumerable<Type> Tables { get; set; }
}
