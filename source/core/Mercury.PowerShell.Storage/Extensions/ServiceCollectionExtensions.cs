// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Storage.Abstractions;
using Mercury.PowerShell.Storage.Internal;
using Mercury.PowerShell.Storage.Options;
using Mercury.PowerShell.Storage.Options.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.PowerShell.Storage.Extensions;

/// <summary>
///   Extensions for the service collection.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions {
  /// <summary>
  ///   Adds the storage to the service collection.
  /// </summary>
  /// <param name="serviceCollection">The service collection.</param>
  /// <param name="configure">The options.</param>
  /// <returns>The service collection itself.</returns>
  public static IServiceCollection AddStorage(this IServiceCollection serviceCollection, Action<IStorageOptions> configure) {
    var options = StorageOptions.Empty;
    configure.Invoke(options);

    var provider = new StorageProvider(options);

    Task.Run(async () => await provider.InitializeAsync()).Wait();

    serviceCollection.AddSingleton<IStorageProvider>(provider);
    serviceCollection.AddTransient(typeof(IReadOnlyRepository<>), typeof(Repository<>));
    serviceCollection.AddTransient(typeof(IRepository<>), typeof(Repository<>));

    return serviceCollection;
  }
}
