// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.ArgumentCompleter.Manifest;

/// <summary>
///   Represents the definition tree of the manifest.
/// </summary>
/// <param name="GlobalOptions">The global options of the manifest.</param>
/// <param name="Commands">The commands of the manifest.</param>
/// <param name="ChildCommands">The child commands of the manifest.</param>
public readonly record struct ManifestDefinitionTree(
  IEnumerable<ManifestOptionNode> GlobalOptions,
  IEnumerable<ManifestCommandNode> Commands,
  Dictionary<string, ManifestChildCommandNode> ChildCommands
);
