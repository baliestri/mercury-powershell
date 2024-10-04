// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.ArgumentCompleter.Archive.Abstractions;

/// <summary>
///   Defines the contract for an archiver.
/// </summary>
public interface IArchiver {
  /// <summary>
  ///   Configures the archiver for the given output directory, output filename, and files.
  /// </summary>
  /// <param name="outputDirectoryInfo">The output directory information.</param>
  /// <param name="outputFilename">The output filename.</param>
  /// <param name="files">The files to be compressed.</param>
  /// <returns>The compressor.</returns>
  IArchiverCompressor For(DirectoryInfo outputDirectoryInfo, string outputFilename, FileInfo[] files);
}
