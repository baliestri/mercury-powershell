// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Storage.Abstractions;
using Mercury.PowerShell.Storage.Internal;

namespace Mercury.PowerShell.Storage;

/// <summary>
///   Provides an easy way to access the file system.
/// </summary>
public static class FileSystem {
  private static IFileSystem? _currentImplementation;

  /// <summary>
  ///   Gets the current file system implementation.
  /// </summary>
  public static IFileSystem Current => _currentImplementation ??= new FileSystemImplementation();
}
