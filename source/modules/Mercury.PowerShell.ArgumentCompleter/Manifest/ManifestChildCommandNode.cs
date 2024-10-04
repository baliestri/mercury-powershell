// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Text.Json.Serialization;
using Mercury.PowerShell.ArgumentCompleter.Manifest.Converters;
using OneOf;

namespace Mercury.PowerShell.ArgumentCompleter.Manifest;

/// <summary>
///   Represents a child command node in the manifest.
/// </summary>
[JsonConverter(typeof(ManifestChildCommandNodeJsonConverter))]
public sealed class ManifestChildCommandNode : OneOfBase<ManifestCommandNode[], string[], string> {
  /// <summary>
  ///   Initializes a new instance of the <see cref="ManifestChildCommandNode" /> class.
  /// </summary>
  /// <param name="value">The value to initialize the instance.</param>
  public ManifestChildCommandNode(ManifestCommandNode[] value) : base(value) { }

  /// <summary>
  ///   Initializes a new instance of the <see cref="ManifestChildCommandNode" /> class.
  /// </summary>
  /// <param name="value">The value to initialize the instance.</param>
  public ManifestChildCommandNode(string value) : base(value) { }

  /// <summary>
  ///   Initializes a new instance of the <see cref="ManifestChildCommandNode" /> class.
  /// </summary>
  /// <param name="value">The value to initialize the instance.</param>
  public ManifestChildCommandNode(string[] value) : base(value) { }

  public static implicit operator ManifestChildCommandNode(ManifestCommandNode[] value) => new(value);
  public static implicit operator ManifestChildCommandNode(string[] value) => new(value);
  public static implicit operator ManifestChildCommandNode(string value) => new(value);
}
