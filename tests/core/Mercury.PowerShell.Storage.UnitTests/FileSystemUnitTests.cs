// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using JetBrains.Annotations;
using Mercury.PowerShell.Storage.Abstractions;
using Mercury.PowerShell.Storage.Internal;
using Shouldly;

namespace Mercury.PowerShell.Storage.UnitTests;

[TestSubject(typeof(FileSystem))]
public class FileSystemUnitTests {
  [Fact]
  public void CurrentImplementation_ShouldBeAssignableToFileSystem() {
    // Act
    var current = FileSystem.Current;

    // Assert
    current.ShouldBeOfType<FileSystemImplementation>();
    current.ShouldBeAssignableTo<IFileSystem>();
  }
}
