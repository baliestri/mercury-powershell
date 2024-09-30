using System.Diagnostics.CodeAnalysis;
using SQLite;

namespace Mercury.PowerShell.Storage;

/// <summary>
///   Represents an entity.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class Entity {
  /// <summary>
  ///   The entity identifier.
  /// </summary>
  [PrimaryKey]
  [AutoIncrement]
  public long Id { get; init; }

  /// <summary>
  ///   The entity creation date.
  /// </summary>
  public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

  /// <summary>
  ///   The entity update date.
  /// </summary>
  public DateTimeOffset? UpdatedAt { get; set; }
}
