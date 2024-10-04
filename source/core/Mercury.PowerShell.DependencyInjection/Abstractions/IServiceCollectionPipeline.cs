// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.Extensions.DependencyInjection;

namespace Mercury.PowerShell.DependencyInjection.Abstractions;

/// <summary>
///   Blueprint for classes that register service collection pipelines in the DI registrar.
/// </summary>
public interface IServiceCollectionPipeline {
  /// <summary>
  ///   Registers the pipelines in the service collection.
  /// </summary>
  /// <param name="serviceCollection">The service collection to register the pipelines.</param>
  void Register(IServiceCollection serviceCollection);
}
