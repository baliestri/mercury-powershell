// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Reflection;
using Mercury.PowerShell.DependencyInjection.Abstractions;
using Mercury.PowerShell.DependencyInjection.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.PowerShell.DependencyInjection;

internal sealed class ServiceProviderBuilder {
  private readonly IServiceCollection _serviceCollection;

  private ServiceProviderBuilder()
    => _serviceCollection = new ServiceCollection();

  public static ServiceProviderBuilder CreateBuilder()
    => new();

  public ServiceProviderBuilder AddAllInjectablesFromAssemblies() {
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var injectables = assemblies
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => type.GetCustomAttribute<InjectableAttribute>() is not null);

    foreach (var injectable in injectables) {
      var injectableAttribute = injectable.GetCustomAttribute<InjectableAttribute>()!;
      var serviceType = injectableAttribute.ServiceType ?? injectable;

      _serviceCollection.Add(new ServiceDescriptor(serviceType, injectable, injectableAttribute.Lifetime));
    }

    return this;
  }

  public ServiceProviderBuilder AddAllPipelinesFromAssemblies() {
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var pipelines = assemblies
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IServicePipelines).IsAssignableFrom(type));

    foreach (var pipeline in pipelines) {
      var instance = Activator.CreateInstance(pipeline) as IServicePipelines;
      instance?.Register(_serviceCollection);
    }

    return this;
  }

  public void BuildAndApply() {
    var serviceProvider = _serviceCollection.BuildServiceProvider();

    ServiceRegistrar.SetServiceProvider(serviceProvider);
  }
}
