// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Storage.Options.Abstractions;

namespace Mercury.PowerShell.Storage.Extensions;

[ExcludeFromCodeCoverage]
internal static class StorageOptionsExtensions {
  /// <summary>
  ///   Gets the full path of the database file.
  /// </summary>
  /// <param name="options">The options.</param>
  /// <returns>The full path of the database file.</returns>
  public static string FilePath(this IStorageOptions options)
    => Path.Combine(FileSystem.Current.StorageDirectory, options.Name);
}
