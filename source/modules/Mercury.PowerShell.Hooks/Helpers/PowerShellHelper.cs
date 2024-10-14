// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using System.Reflection;

namespace Mercury.PowerShell.Hooks.Helpers;

internal static class PowerShellHelper {
  /// <summary>
  ///   Gets the parameters of a cmdlet.
  /// </summary>
  /// <param name="cmdlet">The cmdlet to get the parameters from.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet.</typeparam>
  /// <returns>The parameters of the cmdlet.</returns>
  public static PSObject GetParameters<TCmdlet>(TCmdlet cmdlet) where TCmdlet : PSCmdlet {
    var parameters = new PSObject();

    var @params = cmdlet
      .GetType()
      .GetProperties()
      .Where(property => property.GetCustomAttribute<ParameterAttribute>() is not null)
      .ToArray();

    foreach (var property in @params) {
      var value = property.GetValue(cmdlet);

      parameters.Properties.Add(new PSNoteProperty(property.Name, value));
    }

    return parameters;
  }
}
