// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.DependencyInjection.Attributes;
using Mercury.PowerShell.DependencyInjection.UnitTests.Services;

namespace Mercury.PowerShell.DependencyInjection.UnitTests.Cmdlets;

[Cmdlet(VerbsDiagnostic.Test, "ServiceProblemSample")]
public sealed class TestServiceProblemSampleCmdlet : PSAsyncCmdlet {
  [Inject(Required = true)]
  internal TestServiceNotInjected _serviceNotInjected;

  public void TriggerBeginProcessing() => BeginProcessing();
}
