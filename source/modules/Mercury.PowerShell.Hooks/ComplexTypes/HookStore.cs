// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Enums;

namespace Mercury.PowerShell.Hooks.ComplexTypes;

/// <summary>
///   Represents a store of hooks.
/// </summary>
public readonly struct HookStore : IEquatable<HookStore>, IEqualityComparer<HookStore> {
  /// <inheritdoc />
  public bool Equals(HookStore other)
    => Items.Equals(other.Items) && Type == other.Type;

  /// <inheritdoc />
  public override bool Equals(object? obj)
    => obj is HookStore other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
    => HashCode.Combine(Items, (int)Type);

  public HashSet<HookItem> Items { get; }

  /// <summary>
  ///   Gets the type of the hook.
  /// </summary>
  public HookType Type { get; }

  private HookStore(HashSet<HookItem> items, HookType type)
    => (Items, Type) = (items, type);

  /// <summary>
  ///   Creates a new instance of the <see cref="HookStore" /> class.
  /// </summary>
  /// <param name="items">The initial hook items.</param>
  /// <param name="type">The type of the hook.</param>
  /// <returns>A new instance of the <see cref="HookStore" /> class.</returns>
  public static HookStore Create(HashSet<HookItem> items, HookType type)
    => new(items, type);

  /// <summary>
  ///   Creates an empty instance of the <see cref="HookStore" /> class.
  /// </summary>
  /// <param name="type">The type of the hook.</param>
  /// <returns>An empty instance of the <see cref="HookStore" /> class.</returns>
  public static HookStore Empty(HookType type)
    => new([], type);

  /// <inheritdoc />
  public bool Equals(HookStore left, HookStore right)
    => left.Items.Equals(right.Items) && left.Type == right.Type;

  /// <inheritdoc />
  public int GetHashCode(HookStore obj)
    => HashCode.Combine(obj.Items, (int)obj.Type);

  public static bool operator ==(HookStore left, HookStore right) => left.Equals(right);
  public static bool operator !=(HookStore left, HookStore right) => !(left == right);
}
