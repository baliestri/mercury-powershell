// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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

    serviceProvider.bindInjections(cmdlet);
  }

  [ExcludeFromCodeCoverage]
  private static void bindInjections<TCmdlet>(this IServiceProvider serviceProvider, TCmdlet obj) where TCmdlet : PSAsyncCmdlet {
    var type = obj.GetType();

    var properties = type
      .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
      .Where(propertyInfo => Attribute.IsDefined(propertyInfo, typeof(InjectAttribute)));

    var fields = type
      .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
      .Where(fieldInfo => Attribute.IsDefined(fieldInfo, typeof(InjectAttribute)));

    var parallelExceptions = new ConcurrentQueue<Exception?>();
    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };

    Parallel.ForEach(properties, parallelOptions, property => {
      var serviceDependencyAttribute = property.GetCustomAttribute<InjectAttribute>(true);

      if (serviceDependencyAttribute is null) {
        return;
      }

      var service = serviceProvider.GetService(property.PropertyType);
      if (service is not null) {
        try {
          property.SetValue(obj, service);
        }
        catch (ArgumentException) {
          // Property setter does not exist, try to set the backing field
          var backingField = type.GetField($"<{property.Name}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
          backingField?.SetValue(obj, service);
        }

        return;
      }

      parallelExceptions.Enqueue(RequiredServiceNotFoundException.TryCreate(serviceDependencyAttribute.Required, property.PropertyType));
    });

    Parallel.ForEach(fields, parallelOptions, field => {
      var serviceDependencyAttribute = field.GetCustomAttribute<InjectAttribute>(true);

      if (serviceDependencyAttribute is null) {
        return;
      }

      var service = serviceProvider.GetService(field.FieldType);
      if (service is not null) {
        field.SetValue(obj, service);
        return;
      }

      parallelExceptions.Enqueue(RequiredServiceNotFoundException.TryCreate(serviceDependencyAttribute.Required, field.FieldType));
    });

    var notNullParallelExceptions = parallelExceptions
      .Where(exception => exception is not null)
      .Cast<Exception>()
      .ToArray();

    if (notNullParallelExceptions.Length > 0) {
      throw new AggregateException(notNullParallelExceptions);
    }
  }
}
