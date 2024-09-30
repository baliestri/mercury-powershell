// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Storage.Options.Abstractions;
using SQLite;

namespace Mercury.PowerShell.Storage.Options;

[ExcludeFromCodeCoverage]
internal sealed class ConfigureStorageOptions : IConfigureStorageName, IConfigureStorageFlags {
  private SQLiteOpenFlags _flags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache;
  private string _name = string.Empty;
  private IEnumerable<Type> _tables = [];

  /// <inheritdoc />
  public IConfigureStorageTables WithFlags(SQLiteOpenFlags flags) {
    _flags = flags;
    return this;
  }

  /// <inheritdoc />
  public IStorageOptions Done()
    => new StorageOptions {
      Name = _name,
      OpenFlags = _flags,
      Tables = _tables
    };

  /// <inheritdoc />
  public IConfigureStorageOptions WithTables(IEnumerable<Type> tables) {
    _tables = tables;
    return this;
  }

  /// <inheritdoc />
  public IConfigureStorageOptions WithTables(params Type[] tables) {
    _tables = tables;
    return this;
  }

  /// <inheritdoc />
  public IConfigureStorageFlags WithName(string name) {
    _name = name;
    return this;
  }
}
