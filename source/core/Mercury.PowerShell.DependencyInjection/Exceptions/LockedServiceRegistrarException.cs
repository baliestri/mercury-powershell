// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.DependencyInjection.Exceptions;

/// <summary>
///   Represents an exception thrown when the service provider is already locked.
/// </summary>
public sealed class LockedServiceRegistrarException() : Exception("There was an attempt to set the service provider when it was already locked.") {
  /// <summary>
  ///   Throws an exception if the condition is <c>true</c>.
  /// </summary>
  /// <param name="condition">The condition to be evaluated.</param>
  /// <exception cref="LockedServiceRegistrarException">Thrown when the condition is <c>true</c>.</exception>
  public static void ThrowIf(bool condition) {
    if (condition) {
      throw new LockedServiceRegistrarException();
    }
  }
}
