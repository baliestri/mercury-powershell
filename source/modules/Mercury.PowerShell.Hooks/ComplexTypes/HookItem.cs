// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace Mercury.PowerShell.Hooks.ComplexTypes;

/// <summary>
///   Represents an item of a hook.
/// </summary>
public readonly struct HookItem : IEquatable<HookItem>, IEqualityComparer<HookItem> {
  [SetsRequiredMembers]
  private HookItem(string identifier, ScriptBlock action)
    => (Identifier, Action) = (identifier, action);

  /// <summary>
  ///   The unique identifier of the hook.
  /// </summary>
  public required string Identifier { get; init; }

  /// <summary>
  ///   The action of the hook to be executed when the hook is triggered.
  /// </summary>
  public required ScriptBlock Action { get; init; }

  /// <summary>
  ///   Creates a new <see cref="HookItem" />.
  /// </summary>
  /// <param name="identifier">The unique identifier of the hook.</param>
  /// <param name="action">The action of the hook to be executed when the hook is triggered.</param>
  /// <returns>A new <see cref="HookItem" />.</returns>
  public static HookItem Create(string identifier, ScriptBlock action)
    => new(identifier, action);

  /// <inheritdoc />
  public bool Equals(HookItem other)
    => Identifier == other.Identifier && Action.Equals(other.Action);

  /// <inheritdoc />
  public override bool Equals(object? obj)
    => obj is HookItem other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
    => HashCode.Combine(Identifier, Action);

  /// <inheritdoc />
  public bool Equals(HookItem x, HookItem y)
    => x.Identifier == y.Identifier;

  /// <inheritdoc />
  public int GetHashCode(HookItem obj)
    => obj.Identifier.GetHashCode();

  public static bool operator ==(HookItem left, HookItem right) => left.Equals(right);
  public static bool operator !=(HookItem left, HookItem right) => !(left == right);
}
