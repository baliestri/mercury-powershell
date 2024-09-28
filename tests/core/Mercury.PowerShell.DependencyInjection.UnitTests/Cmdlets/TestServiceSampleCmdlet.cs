// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.DependencyInjection.Attributes;
using Mercury.PowerShell.DependencyInjection.UnitTests.Services;
using Shouldly;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Cmdlets;

[Cmdlet(VerbsDiagnostic.Test, "ServiceSample")]
public sealed class TestServiceSampleCmdlet : PSAsyncCmdlet {
  [Inject]
  internal TestServiceWithoutInterface _serviceWithoutInterface;

  [Inject(Required = true)]
  internal ITestServiceWithInterface ServiceWithInterface { get; } = default!;

  [Inject]
  internal ITestServiceThroughPipeline ServiceThroughPipeline { get; } = default!;

  public override async Task ProcessRecordAsync(CancellationToken cancellationToken = default) {
    cancellationToken.ShouldNotBe(default, "The cancellation token should not be default.");
    cancellationToken.IsCancellationRequested.ShouldBeFalse("The cancellation token should not be requested.");

    await base.ProcessRecordAsync(cancellationToken);
  }

  public void TriggerBeginProcessing() => BeginProcessing();
  public void TriggerProcessRecord() => ProcessRecord();
  public void TriggerEndProcessing() => EndProcessing();
}
