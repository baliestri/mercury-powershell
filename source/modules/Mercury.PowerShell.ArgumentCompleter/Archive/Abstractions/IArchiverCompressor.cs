// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.ArgumentCompleter.Archive.Abstractions;

/// <summary>
///   Defines the contract for an archiver compressor.
/// </summary>
public interface IArchiverCompressor {
  /// <summary>
  ///   Compresses the files.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task Compress(CancellationToken cancellationToken = default);
}
