// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.DependencyInjection.Attributes;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Services;

internal interface ITestServiceWithInterface {
  void DoSomething();
}

[ExcludeFromCodeCoverage]
[Injectable(ServiceType = typeof(ITestServiceWithInterface))]
internal sealed class TestServiceWithInterface : ITestServiceWithInterface {
  public void DoSomething()
    => Console.WriteLine("Doing something...");
}
