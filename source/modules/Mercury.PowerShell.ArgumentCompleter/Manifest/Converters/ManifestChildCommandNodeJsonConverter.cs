// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Text.Json;
using System.Text.Json.Serialization;
using Mercury.PowerShell.ArgumentCompleter.Extensions;

namespace Mercury.PowerShell.ArgumentCompleter.Manifest.Converters;

internal sealed class ManifestChildCommandNodeJsonConverter : JsonConverter<ManifestChildCommandNode> {
  /// <inheritdoc />
  public override ManifestChildCommandNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType == JsonTokenType.String) {
      var manifestCommandNodeString = reader.GetString() ?? string.Empty;

      return new ManifestChildCommandNode(manifestCommandNodeString);
    }

    if (JsonSerializerExtensions.TryDeserialize<ManifestCommandNode[]>(ref reader, options, out var manifestCommandNodes)) {
      return new ManifestChildCommandNode(manifestCommandNodes);
    }

    if (JsonSerializerExtensions.TryDeserialize<string[]>(ref reader, options, out var manifestCommandNodeStrings)) {
      return new ManifestChildCommandNode(manifestCommandNodeStrings);
    }

    throw new JsonException("Invalid JSON for ManifestChildCommandNode");
  }

  /// <inheritdoc />
  public override void Write(Utf8JsonWriter writer, ManifestChildCommandNode value, JsonSerializerOptions options)
    => value.Switch(
      manifestCommandNodes => JsonSerializer.Serialize(writer, manifestCommandNodes, options),
      manifestCommandNodeStrings => JsonSerializer.Serialize(writer, manifestCommandNodeStrings, options),
      writer.WriteStringValue
    );
}
