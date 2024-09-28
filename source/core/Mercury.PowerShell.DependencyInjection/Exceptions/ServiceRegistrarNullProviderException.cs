// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.DependencyInjection.Exceptions;

public sealed class ServiceRegistrarNullProviderException()
  : Exception("The service provider is not set. Call the BuildAndApply method in ServiceProviderBuilder before using the service provider.");
