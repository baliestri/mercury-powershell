// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.DependencyInjection.Exceptions;

namespace Mercury.PowerShell.DependencyInjection;

internal static class ServiceRegistrar {
  /// <summary>
  ///   Determines whether the service provider is locked.
  /// </summary>
  public static bool IsLocked { get; private set; }

  /// <summary>
  ///   The service provider.
  /// </summary>
  /// <remarks>When <see cref="ServiceRegistrar.IsLocked" /> is <c>true</c>, this property is not <c>null</c>.</remarks>
  [MemberNotNullWhen(true, nameof(IsLocked))]
  public static IServiceProvider? ServiceProvider { get; private set; }

  /// <summary>
  ///   Sets the service provider.
  /// </summary>
  /// <param name="serviceProvider">The service provider.</param>
  /// <exception cref="InvalidOperationException">When the service provider is already locked.</exception>
  public static void SetServiceProvider(IServiceProvider serviceProvider) {
    LockedServiceRegistrarException.ThrowIf(IsLocked);

    ServiceProvider = serviceProvider;
    IsLocked = true;
  }
}
