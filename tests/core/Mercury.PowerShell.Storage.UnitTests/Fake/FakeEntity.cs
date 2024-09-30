// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Storage.Attributes;

namespace Mercury.PowerShell.Storage.UnitTests.Fake;

[ExcludeFromCodeCoverage]
[StorageTable]
public sealed class FakeEntity : Entity {
  public string? Name { get; set; }
}
