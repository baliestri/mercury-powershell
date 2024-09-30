// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Mercury.PowerShell.Storage.Abstractions;

namespace Mercury.PowerShell.Storage.Internal;

[ExcludeFromCodeCoverage]
internal sealed class FileSystemImplementation : IFileSystem {
  private static readonly string _directoryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ? "Mercury"
    : "mercury";

  private static readonly string _storageDirectoryPath = Path
    .Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _directoryName);

  public FileSystemImplementation() {
    if (!Directory.Exists(_storageDirectoryPath)) {
      Directory.CreateDirectory(_storageDirectoryPath);
    }
  }

  /// <inheritdoc />
  public string StorageDirectory => _storageDirectoryPath;
}
