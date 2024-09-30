// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Storage.Options.Abstractions;
using SQLite;

namespace Mercury.PowerShell.Storage.Options;

/// <summary>
///   Options for storage.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class StorageOptions : IStorageOptions {
  internal static readonly StorageOptions Empty = new() {
    Name = string.Empty,
    OpenFlags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache
  };

  /// <inheritdoc />
  public required string Name { get; set; }

  /// <inheritdoc />
  public SQLiteOpenFlags OpenFlags { get; set; }

  /// <inheritdoc />
  public IEnumerable<Type> Tables { get; set; } = [];

  internal static IStorageOptions WithName(string name)
    => new StorageOptions {
      Name = name,
      OpenFlags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache
    };

  internal static IStorageOptions WithNameAndTables(string name, IEnumerable<Type> tables)
    => new StorageOptions {
      Name = name,
      OpenFlags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache,
      Tables = tables
    };
}
