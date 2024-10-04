// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Mercury.PowerShell.ArgumentCompleter.Extensions;

[ExcludeFromCodeCoverage]
internal static class JsonSerializerExtensions {
  /// <summary>
  ///   Tries to deserialize the JSON content to the specified type.
  /// </summary>
  /// <param name="reader">The reader.</param>
  /// <param name="options">The options.</param>
  /// <param name="result">The result.</param>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <returns><see langword="true" /> if the deserialization was successful; otherwise, <see langword="false" />.</returns>
  public static bool TryDeserialize<T>(ref Utf8JsonReader reader, JsonSerializerOptions options, [NotNullWhen(true)] out T? result) {
    try {
      result = JsonSerializer.Deserialize<T>(ref reader, options);
      return result is not null;
    }
    catch {
      result = default;
      return false;
    }
  }
}
