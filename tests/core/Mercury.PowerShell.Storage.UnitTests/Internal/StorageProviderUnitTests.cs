// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using JetBrains.Annotations;
using Mercury.PowerShell.Storage.Extensions;
using Mercury.PowerShell.Storage.Internal;
using Mercury.PowerShell.Storage.Options;
using Mercury.PowerShell.Storage.UnitTests.Fake;
using Shouldly;

namespace Mercury.PowerShell.Storage.UnitTests.Internal;

[TestSubject(typeof(StorageProvider))]
public class StorageProviderUnitTests {
  [Fact]
  public void StorageProviderOptions_ShouldThrow_WhenNameIsNullOrEmpty() {
    // Arrange
    var emptyOptions = StorageOptions
      .Configure()
      .WithName(string.Empty)
      .Done();
    var nullOptions = StorageOptions
      .Configure()
      .WithName(null!)
      .Done();

    // Assert
    Should.Throw<ArgumentException>(emptyOptionsAct);
    Should.Throw<ArgumentException>(nullOptionsAct);
    return;

    // Act
    void emptyOptionsAct() => new StorageProvider(emptyOptions);
    void nullOptionsAct() => new StorageProvider(nullOptions);
  }

  [Fact]
  public void StorageProviderOptions_ShouldNotThrow_WhenNameIsNotNullOrEmpty() {
    // Arrange
    var options = StorageOptions
      .Configure()
      .WithName("test.db3")
      .Done();

    // Act
    var provider = new StorageProvider(options);

    // Assert
    provider.ShouldNotBeNull();
  }

  [Fact]
  public async Task StorageProviderConnection_ShouldNotBeNull_WhenInitialized() {
    // Arrange
    var options = StorageOptions
      .Configure()
      .WithName("test.db3")
      .Done();
    var provider = new StorageProvider(options);

    // Act
    await provider.InitializeAsync();

    // Assert
    provider.Connection.ShouldNotBeNull();

    // Clean
    await provider.Connection.CloseAsync();
    File.Delete(options.FilePath());
  }

  [Fact]
  public async Task StorageProviderConnection_ShouldNotBeNull_WhenInitializedWithTables() {
    // Arrange
    var options = StorageOptions
      .Configure()
      .WithName("test2.db3")
      .WithTables(typeof(FakeEntity))
      .Done();
    var provider = new StorageProvider(options);

    // Act
    await provider.InitializeAsync();

    // Assert
    provider.Connection.ShouldNotBeNull();

    // Clean
    await provider.Connection.CloseAsync();
    File.Delete(options.FilePath());
  }

  [Fact]
  public async Task StorageProviderConnection_ShouldHaveTheCreatedTables_WhenInitialized() {
    // Arrange
    var options = StorageOptions
      .Configure()
      .WithName("test3.db3")
      .Done();
    var provider = new StorageProvider(options);

    // Act
    await provider.InitializeAsync();

    // Assert
    var tables = await provider.Connection.GetTableInfoAsync("FakeEntity");
    tables.ShouldNotBeEmpty();

    // Clean
    await provider.Connection.CloseAsync();
    File.Delete(options.FilePath());
  }
}
