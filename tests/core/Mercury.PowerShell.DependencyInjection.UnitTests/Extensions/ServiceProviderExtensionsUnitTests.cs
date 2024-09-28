// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using JetBrains.Annotations;
using Mercury.PowerShell.DependencyInjection.Extensions;
using Mercury.PowerShell.DependencyInjection.UnitTests.Cmdlets;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Extensions;

[TestSubject(typeof(ServiceProviderExtensions))]
public class ServiceProviderExtensionsUnitTests {
  [Fact]
  public void BindCmdletInjections_ShouldThrow_WhenServiceProviderIsNull() {
    // Arrange
    var serviceProvider = default(IServiceProvider);

    // Assert
    Should.Throw<ArgumentNullException>(act);
    return;

    // Act
    void act() => serviceProvider!.BindCmdletInjections(new TestServiceSampleCmdlet());
  }

  [Fact]
  public void BindCmdletInjections_ShouldThrow_WhenCmdletIsNull() {
    // Arrange
    var serviceProvider = new ServiceCollection().BuildServiceProvider();
    var cmdlet = default(TestServiceSampleCmdlet);

    // Assert
    Should.Throw<ArgumentNullException>(act);
    return;

    // Act
    void act() => serviceProvider.BindCmdletInjections(cmdlet!);
  }
}
