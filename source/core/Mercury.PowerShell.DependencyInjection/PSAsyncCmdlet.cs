// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.DependencyInjection.Exceptions;
using Mercury.PowerShell.DependencyInjection.Extensions;
using Mercury.PowerShell.DependencyInjection.Utilities;

namespace Mercury.PowerShell.DependencyInjection;

/// <summary>
///   A base class for cmdlets that need to perform asynchronous operations.
/// </summary>
// ReSharper disable once InconsistentNaming
public abstract class PSAsyncCmdlet : PSCmdlet, IDisposable {
  private readonly CancellationTokenSource _cancellationTokenSource;
  private readonly IServiceProvider _serviceProvider;

  private bool _isDisposed;

  /// <summary>
  ///   Initializes a new instance of the <see cref="PSAsyncCmdlet" /> class.
  /// </summary>
  protected PSAsyncCmdlet() {
    _cancellationTokenSource = new CancellationTokenSource();

    if (!ServiceRegistrar.IsLocked) {
      ServiceProviderBuilder
        .CreateBuilder()
        .AddAllInjectablesFromAssemblies()
        .AddAllPipelinesFromAssemblies()
        .BuildAndApply();
    }

    _serviceProvider = ServiceRegistrar.ServiceProvider ?? throw new ServiceRegistrarNullProviderException();
  }

  /// <summary>
  ///   A <see cref="CancellationToken" /> that can be used to observe cancellation requests.
  /// </summary>
  protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

  /// <summary>
  ///   Method to be called when the cmdlet is initialized.
  /// </summary>
  /// <remarks>
  ///   Override this method to add custom initialization logic.
  /// </remarks>
  protected virtual Action<PSAsyncCmdlet, IServiceProvider> OnInitialize { get; } = (cmdlet, serviceProvider)
    => serviceProvider.BindCmdletInjections(cmdlet);

  /// <inheritdoc cref="IDisposable.Dispose" />
  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  ///   When overridden in the derived class, performs initialization of command execution.
  /// </summary>
  /// <remarks>
  ///   Default implementation in the base class just returns a completed task.
  /// </remarks>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  public virtual async Task BeginProcessingAsync(CancellationToken cancellationToken)
    => await Task.CompletedTask;

  /// <summary>
  ///   When overridden in the derived class, performs execution of the command.
  /// </summary>
  /// <remarks>
  ///   Default implementation in the base class just returns a completed task.
  /// </remarks>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  public virtual async Task ProcessRecordAsync(CancellationToken cancellationToken)
    => await Task.CompletedTask;

  /// <summary>
  ///   When overridden in the derived class, performs cleanup after the command execution.
  /// </summary>
  /// <remarks>
  ///   Default implementation in the base class just returns a completed task.
  /// </remarks>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  public virtual async Task EndProcessingAsync(CancellationToken cancellationToken)
    => await Task.CompletedTask;

  /// <inheritdoc />
  protected sealed override void BeginProcessing() {
    OnInitialize(this, _serviceProvider);

    ThreadAffinitiveSynchronizationContext.RunSynchronized(async () => await BeginProcessingAsync(CancellationToken));
  }

  /// <inheritdoc />
  protected sealed override void ProcessRecord()
    => ThreadAffinitiveSynchronizationContext.RunSynchronized(async () => await ProcessRecordAsync(CancellationToken));

  /// <inheritdoc />
  protected sealed override void EndProcessing() {
    ThreadAffinitiveSynchronizationContext.RunSynchronized(async () => await EndProcessingAsync(CancellationToken));

    if (!_cancellationTokenSource.IsCancellationRequested) {
      _cancellationTokenSource.Cancel();
    }

    _cancellationTokenSource.Dispose();

    base.EndProcessing();
  }

  /// <summary>
  ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
  /// </summary>
  /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
  protected virtual void Dispose(bool isDisposing) {
    if (_isDisposed) {
      return;
    }

    if (isDisposing) {
      _cancellationTokenSource.Dispose();
    }

    _isDisposed = true;
  }
}
