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

  private HookVariableNames(string currentValue) : this(currentValue, tryGetEquivalentHookType(currentValue)) { }

  private HookVariableNames(HookType equivalentHookType) : this(tryGetCurrentValue(equivalentHookType), equivalentHookType) { }

  private HookVariableNames(string currentValue, HookType equivalentHookType)
    => (CurrentValue, EquivalentHookType) = (formatHookVariableName(currentValue), equivalentHookType);

  /// <summary>
  ///   Hook for changing the working directory.
  /// </summary>
  public static readonly HookVariableNames ChangeWorkingDirectory = new(nameof(HookType.ChangeWorkingDirectory), HookType.ChangeWorkingDirectory);

  /// <summary>
  ///   Hook for pre-prompt.
  /// </summary>
  public static readonly HookVariableNames PrePrompt = new(nameof(HookType.PrePrompt), HookType.PrePrompt);

  private static HookType tryGetEquivalentHookType(string value)
    => value switch {
      $"MercuryProxyHook{nameof(HookType.ChangeWorkingDirectory)}" => HookType.ChangeWorkingDirectory,
      $"MercuryProxyHook{nameof(HookType.PrePrompt)}" => HookType.PrePrompt,
      var _ => throw new ArgumentException("Invalid hook variable name.", nameof(value))
    };

  private static string tryGetCurrentValue(HookType hookType)
    => hookType switch {
      HookType.ChangeWorkingDirectory => ChangeWorkingDirectory,
      HookType.PrePrompt => PrePrompt,
      var _ => throw new ArgumentException("Invalid hook type.", nameof(hookType))
    };

  private static string formatHookVariableName(string hookType)
    => $"MercuryProxyHook{hookType}";

  public static implicit operator string(HookVariableNames hookVariableNames) => hookVariableNames.CurrentValue;
  public static implicit operator HookType(HookVariableNames hookVariableNames) => hookVariableNames.EquivalentHookType;
  public static implicit operator HookVariableNames(string value) => new(value);
  public static implicit operator HookVariableNames(HookType hookType) => new(hookType);
}
