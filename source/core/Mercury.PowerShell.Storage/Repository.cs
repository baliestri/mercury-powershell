// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using Mercury.PowerShell.Storage.Abstractions;
using Mercury.PowerShell.Storage.Extensions;
using SQLite;

namespace Mercury.PowerShell.Storage;

/// <summary>
///   Represents a repository.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
internal sealed class Repository<TEntity>(IStorageProvider provider) : IRepository<TEntity> where TEntity : Entity, new() {
  private readonly SQLiteAsyncConnection _connection = provider.Connection;

  /// <inheritdoc />
  public IReadOnlyRepository<TEntity> AsReadOnly()
    => this;

  /// <inheritdoc />
  public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entity);

    await Task.Run(async () => await _connection.InsertAsync(entity), cancellationToken);
  }

  /// <inheritdoc />
  public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entities);

    await _connection.InsertAllAsync(entities);
  }

  /// <inheritdoc />
  public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entity);

    await Task.Run(async () => {
      entity.UpdatedAt = DateTime.UtcNow;
      await _connection.UpdateAsync(entity);
    }, cancellationToken);
  }

  /// <inheritdoc />
  public async Task UpdateAsync(long id, Action<TEntity> action, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);
    ArgumentNullException.ThrowIfNull(action);

    var entity = await GetAsync(id, cancellationToken);

    if (entity is not null) {
      action(entity);
      await UpdateAsync(entity, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entities);

    await Task.Run(async () => {
      var enumerable = entities.ToList();
      enumerable.ForEach(entity => entity.UpdatedAt = DateTime.UtcNow);
      await _connection.UpdateAllAsync(enumerable);
    }, cancellationToken);
  }

  /// <inheritdoc />
  public async Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(action);

    var entities = await FindAsync(predicate, cancellationToken).ToImmutableListAsync();

    if (entities.Any()) {
      entities.ForEach(action);
      await UpdateAsync(entities, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entity);

    await Task.Run(async () => await _connection.DeleteAsync(entity), cancellationToken);
  }

  /// <inheritdoc />
  public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entities);

    await Task.Run(async () => {
      var enumerable = entities.ToList();

      await _connection.RunInTransactionAsync(connection => enumerable.ForEach(entity => connection.Delete(entity)));
    }, cancellationToken);
  }

  /// <inheritdoc />
  public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    var entities = await FindAsync(predicate, cancellationToken).ToImmutableListAsync();

    if (entities.Any()) {
      await DeleteAsync(entities, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task DeleteAsync(long id, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);

    var entity = await GetAsync(entity => entity.Id == id, cancellationToken);

    if (entity is not null) {
      await DeleteAsync(entity, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(ids);

    var entities = await FindAsync(entity => ids.Contains(entity.Id), cancellationToken).ToImmutableListAsync();

    if (entities.Any()) {
      await DeleteAsync(entities, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    => await Task.Run(async () => await _connection.Table<TEntity>().CountAsync(), cancellationToken);

  /// <inheritdoc />
  public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await Task.Run(async () => await _connection.Table<TEntity>().CountAsync(predicate), cancellationToken);
  }

  /// <inheritdoc />
  public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    var count = await CountAsync(predicate, cancellationToken);

    return count > 0;
  }

  /// <inheritdoc />
  public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);

    var count = await CountAsync(entity => entity.Id == id, cancellationToken);

    return count > 0;
  }

  /// <inheritdoc />
  public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await Task.Run(async () => await _connection.Table<TEntity>().FirstOrDefaultAsync(predicate), cancellationToken);
  }

  /// <inheritdoc />
  public async Task<TEntity?> GetAsync(long id, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);

    return await Task.Run(async () => await _connection.Table<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id), cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken = default)
    => await Task.Run(async () => await _connection.Table<TEntity>().ToListAsync(), cancellationToken);

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await Task.Run(async () => await _connection.Table<TEntity>().Where(predicate).ToListAsync(), cancellationToken);
  }
}
