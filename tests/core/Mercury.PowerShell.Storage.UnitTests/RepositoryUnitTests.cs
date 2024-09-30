// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using JetBrains.Annotations;
using Mercury.PowerShell.Storage.Abstractions;
using Mercury.PowerShell.Storage.Extensions;
using Mercury.PowerShell.Storage.Internal;
using Mercury.PowerShell.Storage.Options;
using Mercury.PowerShell.Storage.UnitTests.Fake;
using Shouldly;

namespace Mercury.PowerShell.Storage.UnitTests;

[TestSubject(typeof(Repository<>))]
public class RepositoryUnitTests : IAsyncLifetime {
  private IStorageProvider _provider = default!;

  /// <inheritdoc />
  public async Task InitializeAsync() {
    var options = StorageOptions
      .Configure()
      .WithName("test.db3")
      .WithTables(typeof(FakeEntity))
      .Done();
    var provider = new StorageProvider(options);

    await provider.InitializeAsync();

    _provider = provider;
  }

  /// <inheritdoc />
  public async Task DisposeAsync() {
    await _provider.Connection.CloseAsync();
    File.Delete(_provider.Options.FilePath());
  }

  [Fact]
  public async Task AddAsync_WithEntity_ShouldAddEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entity = new FakeEntity { Id = 1, Name = "Test" };

    // Act
    await repository.AddAsync(entity);

    // Assert
    var result = await repository.GetAsync(entity.Id);
    result.ShouldBeEquivalentTo(entity);
  }

  [Fact]
  public async Task AddAsync_WithEntities_ShouldAddEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    // Act
    await repository.AddAsync(entities);

    // Assert
    var result = await repository.FindAsync();
    result.ShouldBeEquivalentTo(entities.ToList());
  }

  [Fact]
  public async Task UpdateAsync_WithEntity_ShouldUpdateEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entity = new FakeEntity { Id = 1, Name = "Test" };

    await repository.AddAsync(entity);

    // Act
    entity.Name = "Updated";
    await repository.UpdateAsync(entity);

    // Assert
    var result = await repository.GetAsync(entity.Id);
    result.ShouldBeEquivalentTo(entity);
  }

  [Fact]
  public async Task UpdateAsync_WithEntities_ShouldUpdateEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    foreach (var entity in entities) {
      entity.Name = "Updated";
    }

    await repository.UpdateAsync(entities);

    // Assert
    var result = await repository.FindAsync();
    result.ShouldBeEquivalentTo(entities.ToList());
  }

  [Fact]
  public async Task UpdateAsync_WithPredicate_ShouldUpdateEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    await repository.UpdateAsync(entity => entity.Id == 1, entity => entity.Name = "Updated");

    // Assert
    var result = await repository.GetAsync(entity => entity.Id == 1);

    result.ShouldNotBeNull();
    result.Name.ShouldBe("Updated");
  }

  [Fact]
  public async Task DeleteAsync_WithEntity_ShouldDeleteEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entity = new FakeEntity { Id = 1, Name = "Test" };

    await repository.AddAsync(entity);

    // Act
    await repository.DeleteAsync(entity);

    // Assert
    var result = await repository.GetAsync(entity.Id);
    result.ShouldBeNull();
  }

  [Fact]
  public async Task DeleteAsync_WithEntities_ShouldDeleteEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    await repository.DeleteAsync(entities);

    // Assert
    var result = await repository.FindAsync();
    result.ShouldBeEmpty();
  }

  [Fact]
  public async Task DeleteAsync_WithPredicate_ShouldDeleteEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    await repository.DeleteAsync(entity => entity.Id == 1);

    // Assert
    var result = await repository.GetAsync(entity => entity.Id == 1);
    result.ShouldBeNull();
  }

  [Fact]
  public async Task CountAsync_WithPredicate_ShouldReturnCount() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.CountAsync(entity => entity.Id == 1);

    // Assert
    result.ShouldBe(1);
  }

  [Fact]
  public async Task ExistsAsync_WithPredicate_ShouldReturnTrue() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.ExistsAsync(entity => entity.Id == 1);

    // Assert
    result.ShouldBeTrue();
  }

  [Fact]
  public async Task ExistsAsync_WithPredicate_ShouldReturnFalse() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.ExistsAsync(entity => entity.Id == 4);

    // Assert
    result.ShouldBeFalse();
  }

  [Fact]
  public async Task ExistsAsync_WithId_ShouldReturnTrue() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.ExistsAsync(1);

    // Assert
    result.ShouldBeTrue();
  }

  [Fact]
  public async Task ExistsAsync_WithId_ShouldReturnFalse() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.ExistsAsync(4);

    // Assert
    result.ShouldBeFalse();
  }

  [Fact]
  public async Task GetAsync_WithPredicate_ShouldReturnEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.GetAsync(entity => entity.Id == 1);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldBeEquivalentTo(entities.First());
  }

  [Fact]
  public async Task GetAsync_WithId_ShouldReturnEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.GetAsync(1);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldBeEquivalentTo(entities.First());
  }

  [Fact]
  public async Task FindAsync_ShouldReturnEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.FindAsync();

    // Assert
    result.ShouldBeEquivalentTo(entities.ToList());
  }

  [Fact]
  public async Task FindAsync_WithPredicate_ShouldReturnEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.FindAsync(entity => entity.Id == 1).ToImmutableListAsync();

    // Assert
    result.ShouldNotBeEmpty();
    result.ShouldContain(entity => entity.Id == 1);
  }

  [Fact]
  public async Task FindAsync_WithPredicate_ShouldReturnEmptyList() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.FindAsync(entity => entity.Id == 4);

    // Assert
    result.ShouldBeEmpty();
  }

  [Fact]
  public async Task UpdateAsync_WithIdAndAction_ShouldUpdateEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entity = new FakeEntity { Id = 1, Name = "Test" };

    await repository.AddAsync(entity);

    // Act
    await repository.UpdateAsync(entity.Id, e => e.Name = "Updated");

    // Assert
    var result = await repository.GetAsync(entity.Id);
    result.ShouldNotBeNull();
    result.Name.ShouldBe("Updated");
  }

  [Fact]
  public async Task DeleteAsync_WithId_ShouldDeleteEntity() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entity = new FakeEntity { Id = 1, Name = "Test" };

    await repository.AddAsync(entity);

    // Act
    await repository.DeleteAsync(entity.Id);

    // Assert
    var result = await repository.GetAsync(entity.Id);
    result.ShouldBeNull();
  }

  [Fact]
  public async Task DeleteAsync_WithIds_ShouldDeleteEntities() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    await repository.DeleteAsync(entities.Select(e => e.Id));

    // Assert
    var result = await repository.FindAsync();
    result.ShouldBeEmpty();
  }

  [Fact]
  public async Task CountAsync_WithNoParameters_ShouldReturnCount() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);
    var entities = new[] {
      new FakeEntity { Id = 1, Name = "Test 1" },
      new FakeEntity { Id = 2, Name = "Test 2" },
      new FakeEntity { Id = 3, Name = "Test 3" }
    };

    await repository.AddAsync(entities);

    // Act
    var result = await repository.CountAsync();

    // Assert
    result.ShouldBe(entities.Length);
  }

  [Fact]
  public void AsReadOnly_ShouldReturnReadOnlyRepository() {
    // Arrange
    var repository = new Repository<FakeEntity>(_provider);

    // Act
    var result = repository.AsReadOnly();

    // Assert
    result.ShouldNotBeNull();
    result.ShouldBeAssignableTo<IReadOnlyRepository<FakeEntity>>();
  }
}
