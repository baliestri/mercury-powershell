// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.DependencyInjection.Attributes;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Services;

[ExcludeFromCodeCoverage]
[Injectable]
internal sealed class TestServiceWithoutInterface {
  public void DoSomething()
    => Console.WriteLine("Doing something...");
}
