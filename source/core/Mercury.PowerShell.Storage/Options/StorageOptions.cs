// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Storage.Options.Abstractions;
using SQLite;

namespace Mercury.PowerShell.Storage.Options;

/// <summary>
///   Options for storage.
/// </summary>
public sealed class StorageOptions : IStorageOptions {
  internal static readonly StorageOptions Empty = new() {
    Name = string.Empty
  };

  /// <inheritdoc />
  public required string Name { get; set; }

  /// <inheritdoc />
  public SQLiteOpenFlags OpenFlags { get; set; }

  /// <inheritdoc />
  public IEnumerable<Type> Tables { get; set; } = [];
}
