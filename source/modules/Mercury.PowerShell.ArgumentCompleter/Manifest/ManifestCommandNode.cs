// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.ArgumentCompleter.Manifest;

/// <summary>
///   Represents a command node in the manifest.
/// </summary>
/// <param name="Aliases">The aliases of the command.</param>
/// <param name="Description">The description of the command.</param>
/// <param name="Options">The options of the command.</param>
public readonly record struct ManifestCommandNode(IEnumerable<string> Aliases, string? Description, IEnumerable<ManifestOptionNode>? Options);
