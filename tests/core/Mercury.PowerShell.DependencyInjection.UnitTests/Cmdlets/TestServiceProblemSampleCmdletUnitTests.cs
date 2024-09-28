// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using JetBrains.Annotations;
using Shouldly;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Cmdlets;

[TestSubject(typeof(TestServiceProblemSampleCmdlet))]
public sealed class TestServiceProblemSampleCmdletUnitTests {
  [Fact]
  public void BeginProcessing_ShouldThrow_WhenServiceNotInjected() {
    // Arrange
    var cmdlet = new TestServiceProblemSampleCmdlet();

    // Assert
    Should.Throw<AggregateException>(act);
    return;

    // Act
    void act() => cmdlet.TriggerBeginProcessing();
  }
}
