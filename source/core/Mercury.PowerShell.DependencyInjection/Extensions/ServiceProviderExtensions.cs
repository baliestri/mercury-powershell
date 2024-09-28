// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Reflection;
using Mercury.PowerShell.DependencyInjection.Attributes;
using Mercury.PowerShell.DependencyInjection.Exceptions;

namespace Mercury.PowerShell.DependencyInjection.Extensions;

/// <summary>
///   Extension methods for <see cref="IServiceProvider" />.
/// </summary>
public static class ServiceProviderExtensions {
  /// <summary>
  ///   Binds the injections for the specified cmdlet.
  /// </summary>
  /// <param name="serviceProvider">The service provider.</param>
  /// <param name="cmdlet">The cmdlet to bind the injections.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet.</typeparam>
  public static void BindCmdletInjections<TCmdlet>(this IServiceProvider serviceProvider, TCmdlet cmdlet) where TCmdlet : PSAsyncCmdlet {
    ArgumentNullException.ThrowIfNull(serviceProvider);
    ArgumentNullException.ThrowIfNull(cmdlet);

    serviceProvider.bindPropertyInjections(cmdlet);
    serviceProvider.bindFieldInjections(cmdlet);
  }

  private static void bindPropertyInjections<TCmdlet>(this IServiceProvider serviceProvider, TCmdlet obj) where TCmdlet : PSAsyncCmdlet {
    var type = obj.GetType();
    var properties = type
      .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
      .Where(propertyInfo => Attribute.IsDefined(propertyInfo, typeof(InjectAttribute)));

    foreach (var property in properties) {
      var serviceDependencyAttribute = property.GetCustomAttribute<InjectAttribute>(true);

      if (serviceDependencyAttribute is null) {
        continue;
      }

      var service = serviceProvider.GetService(property.PropertyType);
      if (service is not null) {
        property.SetValue(obj, service);
        continue;
      }

      RequiredServiceNotFoundException.ThrowIf(serviceDependencyAttribute.Required, property.PropertyType);
    }
  }

  private static void bindFieldInjections<TCmdlet>(this IServiceProvider serviceProvider, TCmdlet obj) where TCmdlet : PSAsyncCmdlet {
    var type = obj.GetType();
    var fields = type
      .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
      .Where(fieldInfo => Attribute.IsDefined(fieldInfo, typeof(InjectAttribute)));

    foreach (var field in fields) {
      var serviceDependencyAttribute = field.GetCustomAttribute<InjectAttribute>(true);

      if (serviceDependencyAttribute is null) {
        continue;
      }

      var service = serviceProvider.GetService(field.FieldType);
      if (service is not null) {
        field.SetValue(obj, service);
        continue;
      }

      RequiredServiceNotFoundException.ThrowIf(serviceDependencyAttribute.Required, field.FieldType);
    }
  }
}
