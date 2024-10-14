// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.Hooks.Enums;

/// <summary>
///   Enumerates the hook variable names.
/// </summary>
public readonly struct HookVariableNames {
  /// <summary>
  ///   The current value.
  /// </summary>
  public string CurrentValue { get; }

  /// <summary>
  ///   The equivalent hook type.
  /// </summary>
  public HookType EquivalentHookType { get; }

  private HookVariableNames(string currentValue) : this(tryGetEquivalentHookType(currentValue)) { }

  private HookVariableNames(HookType equivalentHookType)
    => (CurrentValue, EquivalentHookType) = (formatHookVariableName(equivalentHookType), equivalentHookType);

  /// <summary>
  ///   Hook for changing the working directory.
  /// </summary>
  public static readonly HookVariableNames ChangeWorkingDirectory = new(HookType.ChangeWorkingDirectory);

  /// <summary>
  ///   Hook for pre-prompt.
  /// </summary>
  public static readonly HookVariableNames PrePrompt = new(HookType.PrePrompt);

  private static HookType tryGetEquivalentHookType(string value)
    => value switch {
      $"MercuryProxyHook{nameof(HookType.ChangeWorkingDirectory)}" => HookType.ChangeWorkingDirectory,
      $"MercuryProxyHook{nameof(HookType.PrePrompt)}" => HookType.PrePrompt,
      var _ => throw new ArgumentException("Invalid hook variable name.", nameof(value))
    };

  private static string formatHookVariableName(HookType hookType)
    => $"MercuryProxyHook{hookType}";

  public static implicit operator string(HookVariableNames hookVariableNames) => hookVariableNames.CurrentValue;
  public static implicit operator HookType(HookVariableNames hookVariableNames) => hookVariableNames.EquivalentHookType;
  public static implicit operator HookVariableNames(string value) => new(value);
  public static implicit operator HookVariableNames(HookType hookType) => new(hookType);
}
