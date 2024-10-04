// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.DependencyInjection.Abstractions;
using Mercury.PowerShell.DependencyInjection.UnitTests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Pipelines;

public sealed class SomethingServicePipeline : IServiceCollectionPipeline {
  public void Register(IServiceCollection serviceCollection)
    => serviceCollection.AddTransient<ITestServiceThroughPipeline, TestServiceThroughPipeline>();
}
