// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Reflection;
using Mercury.PowerShell.Storage.Abstractions;
using Mercury.PowerShell.Storage.Attributes;
using Mercury.PowerShell.Storage.Extensions;
using Mercury.PowerShell.Storage.Options.Abstractions;
using SQLite;

namespace Mercury.PowerShell.Storage.Internal;

internal sealed class StorageProvider : IStorageProvider {
  public StorageProvider(IStorageOptions options) {
    ArgumentException.ThrowIfNullOrEmpty(options.Name, nameof(options.Name));

    Options = options;
  }

  /// <inheritdoc />
  public SQLiteAsyncConnection Connection { get; private set; } = default!;

  /// <inheritdoc />
  public IStorageOptions Options { get; }

  public async Task InitializeAsync() {
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var tables = assemblies
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => type.GetCustomAttribute<StorageTable>() is not null);

    Connection = new SQLiteAsyncConnection(Options.FilePath(), Options.OpenFlags);

    var tableUnion = tables.Union(Options.Tables);
    var uniqueTables = tableUnion.Distinct().ToArray();

    await Connection.CreateTablesAsync(CreateFlags.AllImplicit, uniqueTables);
  }
}
