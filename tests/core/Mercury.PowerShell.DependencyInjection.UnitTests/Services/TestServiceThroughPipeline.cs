// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Services;

public interface ITestServiceThroughPipeline {
  void DoSomething();
}

[ExcludeFromCodeCoverage]
public sealed class TestServiceThroughPipeline : ITestServiceThroughPipeline {
  public void DoSomething()
    => Console.WriteLine("Doing something...");
}
