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
  private readonly IStorageOptions _options;

  public StorageProvider(IStorageOptions options) {
    ArgumentException.ThrowIfNullOrEmpty(options.Name, nameof(options.Name));

    _options = options;
  }

  /// <inheritdoc />
  public SQLiteAsyncConnection Connection { get; private set; } = default!;

  public async Task InitializeAsync() {
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var tables = assemblies
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => type.GetCustomAttribute<StorageTable>() is not null);

    Connection = new SQLiteAsyncConnection(_options.FilePath(), _options.OpenFlags);

    var tableUnion = tables.Union(_options.Tables);
    var uniqueTables = tableUnion.Distinct().ToArray();

    await Connection.CreateTablesAsync(CreateFlags.AllImplicit, uniqueTables);
  }
}
