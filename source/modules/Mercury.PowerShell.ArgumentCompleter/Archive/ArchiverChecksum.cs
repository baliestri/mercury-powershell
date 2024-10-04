// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Security.Cryptography;
using Mercury.PowerShell.ArgumentCompleter.Archive.Abstractions;

namespace Mercury.PowerShell.ArgumentCompleter.Archive;

/// <summary>
///   Provides methods to generate and verify checksums for <see cref="IArchiver" />.
/// </summary>
public static class ArchiverChecksum {
  /// <summary>
  ///   Generates a SHA-256 checksum for the given stream.
  /// </summary>
  /// <param name="stream">The stream to generate the checksum.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The SHA-256 checksum.</returns>
  public static async Task<string> GenerateAsync(Stream stream, CancellationToken cancellationToken = default) {
    if (stream.CanSeek) {
      stream.Seek(0, SeekOrigin.Begin);
    }

    var checksumBytes = await SHA256.HashDataAsync(stream, cancellationToken);
    var checksum = BitConverter.ToString(checksumBytes).Replace("-", string.Empty);

    return checksum;
  }

  /// <summary>
  ///   Verifies the checksum of the given stream.
  /// </summary>
  /// <param name="stream">The stream to verify the checksum.</param>
  /// <param name="checksum">The checksum to verify.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns><see langword="true" /> if the checksum is valid; otherwise, <see langword="false" />.</returns>
  public static async Task<bool> VerifyAsync(Stream stream, string checksum, CancellationToken cancellationToken = default) {
    var streamChecksum = await GenerateAsync(stream, cancellationToken);

    return string.Equals(streamChecksum, checksum, StringComparison.OrdinalIgnoreCase);
  }
}
