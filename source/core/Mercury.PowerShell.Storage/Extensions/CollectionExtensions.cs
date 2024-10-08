// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.Storage.Extensions;

[ExcludeFromCodeCoverage]
internal static class CollectionExtensions {
  /// <summary>
  ///   Performs the specified action on each element of the <see cref="IEnumerable{T}" />.
  /// </summary>
  /// <param name="source">The elements source.</param>
  /// <param name="action">The <see cref="Action" /> delegate to perform on each element of the <see cref="IEnumerable{T}" /></param>
  /// <typeparam name="T">The type of the elements of the <see cref="IEnumerable{T}" />.</typeparam>
  /// <exception cref="ArgumentNullException">If the action is <c>null</c>.</exception>
  public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
    ArgumentNullException.ThrowIfNull(action);

    foreach (var item in source) {
      action(item);
    }
  }

  /// <summary>
  ///   Converts an <see cref="IEnumerable{T}" /> inside a <see cref="Task{TResult}" /> to an <see cref="IImmutableList{T}" />.
  /// </summary>
  /// <param name="task">The <see cref="Task{TResult}" /> to convert.</param>
  /// <typeparam name="T">The type of the result of the <see cref="Task{TResult}" />, which is an <see cref="IEnumerable{T}" />.</typeparam>
  /// <returns>The immutable list.</returns>
  public static async Task<IImmutableList<T>> ToImmutableListAsync<T>(this Task<IEnumerable<T>> task)
    => (await task).ToImmutableList();
}
