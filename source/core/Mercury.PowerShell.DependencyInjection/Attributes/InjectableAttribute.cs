// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.Extensions.DependencyInjection;

namespace Mercury.PowerShell.DependencyInjection.Attributes;

/// <summary>
///   Marks a class as a service to be registered by the dependency injection container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class InjectableAttribute : Attribute {
  /// <summary>
  ///   The lifetime of the service.
  /// </summary>
  public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Scoped;

  /// <summary>
  ///   The interface type of the service. If not provided, the own type will be used.
  /// </summary>
  public Type? ServiceType { get; init; }
}
