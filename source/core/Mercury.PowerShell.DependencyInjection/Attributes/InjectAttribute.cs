// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.DependencyInjection.Attributes;

/// <summary>
///   Marks a field or property as a dependency to be injected by the dependency injection container.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class InjectAttribute : Attribute {
  /// <summary>
  ///   Whether the dependency is required.
  /// </summary>
  /// <remarks>When <c>true</c>, the dependency is required, and it will throw an exception if it is not found.</remarks>
  public bool Required { get; init; } = true;
}
