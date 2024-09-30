// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Storage.Options.Abstractions;

namespace Mercury.PowerShell.Storage.Attributes;

/// <summary>
///   Marks a class as a storage table.
/// </summary>
/// <remarks>
///   It will be created automatically by the SQLite without using the <see cref="IStorageOptions.Tables" /> option.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StorageTable : Attribute;
