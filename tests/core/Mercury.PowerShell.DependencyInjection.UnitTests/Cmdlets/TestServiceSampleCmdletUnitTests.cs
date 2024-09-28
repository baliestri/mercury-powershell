// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using JetBrains.Annotations;
using Shouldly;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Cmdlets;

[TestSubject(typeof(TestServiceSampleCmdlet))]
public sealed class TestServiceSampleCmdletUnitTests {
  [Fact]
  public void BeginProcessing_ShouldResolveDependencies_WhenInvoked() {
    // Arrange
    var cmdlet = new TestServiceSampleCmdlet();

    // Act
    cmdlet.TriggerBeginProcessing();

    // Assert
    cmdlet._serviceWithoutInterface.ShouldNotBeNull();
    cmdlet.ServiceWithInterface.ShouldNotBeNull();
    cmdlet.ServiceThroughPipeline.ShouldNotBeNull();

    // Dispose
    cmdlet.Dispose();
  }

  [Fact]
  public void ProcessRecord_ShouldCallProcessRecordAsync_WithAValidCancellationToken_WhenInvoked() {
    // Arrange
    var cmdlet = new TestServiceSampleCmdlet();

    // Act
    cmdlet.TriggerBeginProcessing();
    cmdlet.TriggerProcessRecord();
    cmdlet.TriggerEndProcessing();

    // Assert
    // Being asserted in the ProcessRecordAsync method override
  }
}
