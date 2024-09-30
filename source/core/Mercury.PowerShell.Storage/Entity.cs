using SQLite;

namespace Mercury.PowerShell.Storage;

/// <summary>
///   Represents an entity.
/// </summary>
public abstract class Entity {
  /// <summary>
  ///   The entity identifier.
  /// </summary>
  [PrimaryKey]
  public Ulid Id { get; init; } = Ulid.NewUlid();

  /// <summary>
  ///   The entity creation date.
  /// </summary>
  public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

  /// <summary>
  ///   The entity update date.
  /// </summary>
  public DateTimeOffset? UpdatedAt { get; set; }
}
