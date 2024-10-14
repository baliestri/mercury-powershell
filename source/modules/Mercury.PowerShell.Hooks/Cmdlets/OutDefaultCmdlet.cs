// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.ComplexTypes;
using Mercury.PowerShell.Hooks.Enums;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Proxy cmdlet to the <c>Out-Default</c> cmdlet.
/// </summary>
[Cmdlet(VerbsData.Out, "Default", HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096486", RemotingCapability = RemotingCapability.None)]
public sealed class OutDefaultCmdlet : PSCmdlet, IDisposable {
  private const string TARGET_COMMAND = "Microsoft.PowerShell.Core\\Out-Default";

  /// <summary>
  ///   Determines whether the output should be sent to PowerShell's transcription services.
  /// </summary>
  [Parameter]
  public SwitchParameter Transcript { get; init; } = true;

  /// <summary>
  ///   Accepts input to the cmdlet.
  /// </summary>
  [Parameter(ValueFromPipeline = true)]
  public PSObject InputObject { get; init; } = default!;

  private SteppablePipeline? SteppablePipeline { get; set; }

  /// <inheritdoc />
  public void Dispose()
    => SteppablePipeline?.Dispose();

  private void OnEndProcessing() {
    var hookVariable = SessionState.PSVariable.Get(HookVariableNames.PrePrompt);

    if (hookVariable?.Value is not HookStore hookStore) {
      return;
    }

    Parallel.ForEach(hookStore.Items, new ParallelOptions {
      MaxDegreeOfParallelism = 4
    }, item => item.Action.Invoke());
  }

  /// <inheritdoc />
  protected override void BeginProcessing() {
    if (MyInvocation.BoundParameters.TryGetValue("OutBuffer", out var _)) {
      MyInvocation.BoundParameters["OutBuffer"] = 1;
    }

    var commandInfo = InvokeCommand.GetCommand(TARGET_COMMAND, CommandTypes.Cmdlet);
    var scriptBlock = ScriptBlock.Create("& $CommandInfo @BoundParameters @UnboundArguments");
    SessionState.PSVariable.Set("CommandInfo", commandInfo);
    SessionState.PSVariable.Set("UnboundArguments", MyInvocation.UnboundArguments);
    SessionState.PSVariable.Set("BoundParameters", MyInvocation.BoundParameters);

    SteppablePipeline = scriptBlock.GetSteppablePipeline(MyInvocation.CommandOrigin);

    SteppablePipeline.Begin(this);
  }

  /// <inheritdoc />
  protected override void ProcessRecord()
    => SteppablePipeline?.Process(InputObject);

  /// <inheritdoc />
  protected override void EndProcessing() {
    SteppablePipeline?.End();
    OnEndProcessing();
  }

  /// <inheritdoc />
  protected override void StopProcessing()
    => Dispose();
}
