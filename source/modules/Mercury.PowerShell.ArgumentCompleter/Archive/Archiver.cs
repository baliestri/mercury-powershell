// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.IO.Compression;
using System.Management.Automation;
using System.Text;
using Mercury.PowerShell.ArgumentCompleter.Archive.Abstractions;
using Mercury.PowerShell.DependencyInjection.Attributes;

namespace Mercury.PowerShell.ArgumentCompleter.Archive;

[Injectable(ServiceType = typeof(IArchiver))]
internal sealed class Archiver : IArchiver, IArchiverCompressor {
  public const string MACZ_EXTENSION = ".macz";
  public const string SHA256_EXTENSION = ".macz.sha256";

  private FileInfo[] _files = [];

  private DirectoryInfo _outputDirectoryInfo = default!;
  private string _outputFilename = default!;

  /// <inheritdoc />
  public IArchiverCompressor For(DirectoryInfo outputDirectoryInfo, string outputFilename, FileInfo[] files) {
    ArgumentNullException.ThrowIfNull(outputDirectoryInfo);
    ArgumentNullException.ThrowIfNull(files);
    ArgumentException.ThrowIfNullOrEmpty(outputFilename);

    _outputDirectoryInfo = outputDirectoryInfo;
    _outputFilename = outputFilename;
    _files = files;

    return this;
  }

  /// <inheritdoc />
  public async Task Compress(CancellationToken cancellationToken = default) {
    validateOrThrow();

    var outputDirectory = _outputDirectoryInfo.FullName;
    var compressedFilename = concatMaczExtension(_outputFilename);
    var checksumFilename = concatChecksumExtension(_outputFilename);

    await using var compressedFileStream = File.Create(Path.Combine(outputDirectory, compressedFilename));
    using var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true, Encoding.UTF8);

    foreach (var file in _files) {
      var entry = zipArchive.CreateEntry(file.Name, CompressionLevel.SmallestSize);

      await using var fileStream = file.OpenRead();
      await using var entryStream = entry.Open();

      await fileStream.CopyToAsync(entryStream, cancellationToken);
    }

    var checksum = await ArchiverChecksum.GenerateAsync(compressedFileStream, cancellationToken);
    await File.WriteAllTextAsync(Path.Combine(outputDirectory, checksumFilename), checksum, cancellationToken);
  }

  private static string concatMaczExtension(string filename)
    => $"{filename}{MACZ_EXTENSION}";

  private static string concatChecksumExtension(string filename)
    => $"{filename}{SHA256_EXTENSION}";

  private void validateOrThrow() {
    var hasFiles = _files.Length != 0;
    var hasOutputDirectory = _outputDirectoryInfo.Exists;
    var hasOutputFilename = !string.IsNullOrWhiteSpace(_outputFilename);

    var hasManifestFIle = _files.Any(file => file.Name == "manifest.json");

    if (!hasFiles) {
      throw new FileNotFoundException("No files found.");
    }

    if (!hasOutputDirectory) {
      throw new DirectoryNotFoundException("The output directory does not exist.");
    }

    if (!hasOutputFilename) {
      throw new PSArgumentException("The output filename is not valid.");
    }

    if (!hasManifestFIle) {
      throw new FileNotFoundException("The manifest file is missing.");
    }
  }
}
