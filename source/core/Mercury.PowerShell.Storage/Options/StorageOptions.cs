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
  internal StorageOptions() { }

  /// <inheritdoc />
  public required string Name { get; init; }

  /// <inheritdoc />
  public SQLiteOpenFlags OpenFlags { get; init; }

  /// <inheritdoc />
  public IEnumerable<Type> Tables { get; init; } = [];

  /// <summary>
  ///   Configures the storage options.
  /// </summary>
  /// <returns>The builder.</returns>
  public static IConfigureStorageName Configure()
    => new ConfigureStorageOptions();
}
