// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.ArgumentCompleter.Manifest;

/// <summary>
///   Represents an option node in the manifest.
/// </summary>
/// <param name="Aliases">The aliases of the option.</param>
/// <param name="Description">The description of the option.</param>
/// <param name="CompletionValue">The completion value of the option.</param>
public readonly record struct ManifestOptionNode(IEnumerable<string> Aliases, string? Description, string? CompletionValue);
