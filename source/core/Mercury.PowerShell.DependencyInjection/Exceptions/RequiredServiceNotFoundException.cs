// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.DependencyInjection.Exceptions;

/// <summary>
///   Represents an exception thrown when a required service is not found.
/// </summary>
/// <param name="serviceType">The type of the service that was not found.</param>
public sealed class RequiredServiceNotFoundException(Type serviceType) : Exception($"Could not find service for {serviceType.Name}.") {
  /// <summary>
  ///   Throws an exception if the condition is <c>true</c>.
  /// </summary>
  /// <param name="condition">The condition to be evaluated.</param>
  /// <param name="serviceType">The type of the service that was not found.</param>
  /// <exception cref="RequiredServiceNotFoundException">Thrown when the condition is <c>true</c>.</exception>
  public static void ThrowIf(bool condition, Type serviceType) {
    if (condition) {
      throw new RequiredServiceNotFoundException(serviceType);
    }
  }
}
