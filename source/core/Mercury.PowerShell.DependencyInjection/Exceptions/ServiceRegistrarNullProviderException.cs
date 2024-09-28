// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.DependencyInjection.Exceptions;

/// <summary>
///   Represents an exception thrown when the service provider is not set.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ServiceRegistrarNullProviderException()
  : Exception("The service provider is not set. Call the BuildAndApply method in ServiceProviderBuilder before using the service provider.");
