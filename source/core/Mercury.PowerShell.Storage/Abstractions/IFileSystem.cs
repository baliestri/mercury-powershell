// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.Storage.Abstractions;

/// <summary>
///   Provides an easy way to access the file system.
/// </summary>
public interface IFileSystem {
  /// <summary>
  ///   Gets the location of the storage directory, where the data can be stored.
  /// </summary>
  string StorageDirectory { get; }
}
