// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;

namespace Mercury.PowerShell.Storage.Abstractions;

/// <summary>
///   Defines a contract for a read-only repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IReadOnlyRepository<TEntity> where TEntity : Entity {
  /// <summary>
  ///   Counts all entities in the repository.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The number of entities.</returns>
  Task<int> CountAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Counts all entities in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The number of entities.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Checks if an entity exists in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns><c>true</c> if the entity exists, <c>false</c> otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Checks if an entity exists in the repository by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the entity to find.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns><c>true</c> if the entity exists, <c>false</c> otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="id" /> is <c>null</c>.</exception>
  Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Gets an entity in the repository by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the entity to find.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entity if found, null otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="id" /> is <c>null</c>.</exception>
  Task<TEntity?> GetAsync(long id, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Gets an entity in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entity if found, null otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities.</returns>
  Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities found by the predicate.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
