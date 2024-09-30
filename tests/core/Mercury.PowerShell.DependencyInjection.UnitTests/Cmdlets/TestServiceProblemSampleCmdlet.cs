// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.DependencyInjection.Attributes;
using Mercury.PowerShell.DependencyInjection.UnitTests.Services;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Cmdlets;

[Cmdlet(VerbsDiagnostic.Test, "ServiceProblemSample")]
public sealed class TestServiceProblemSampleCmdlet : PSAsyncCmdlet {
  [Inject(Required = true)]
#pragma warning disable CS0414 // Field is assigned but its value is never used
  internal TestServiceNotInjected _serviceNotInjected = default!;
#pragma warning restore CS0414 // Field is assigned but its value is never used

  public void TriggerBeginProcessing() => BeginProcessing();
}
