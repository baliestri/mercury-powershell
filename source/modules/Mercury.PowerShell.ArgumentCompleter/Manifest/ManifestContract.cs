// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.ArgumentCompleter.Manifest;

/// <summary>
///   Represents the manifest contract.
/// </summary>
public sealed class ManifestContract {
  /// <summary>
  ///   Gets or sets the version of the manifest.
  /// </summary>
  public required string Version { get; init; }

  /// <summary>
  ///   Gets or sets the name of the manifest.
  /// </summary>
  public required string Name { get; init; }

  /// <summary>
  ///   Gets or sets the description of the manifest.
  /// </summary>
  public string? Description { get; init; }

  /// <summary>
  ///   Gets or sets the entry points of the manifest.
  /// </summary>
  public required IEnumerable<string> EntryPoints { get; init; }

  /// <summary>
  ///   Gets or sets the definition tree of the manifest.
  /// </summary>
  public required ManifestDefinitionTree DefinitionTree { get; init; }
}
