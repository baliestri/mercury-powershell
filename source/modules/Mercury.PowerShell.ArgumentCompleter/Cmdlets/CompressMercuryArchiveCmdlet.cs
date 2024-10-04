// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.ArgumentCompleter.Archive.Abstractions;
using Mercury.PowerShell.DependencyInjection;
using Mercury.PowerShell.DependencyInjection.Attributes;

namespace Mercury.PowerShell.ArgumentCompleter.Cmdlets;

/// <summary>
///   Compresses a Mercury archive.
/// </summary>
[Cmdlet(VerbsData.Compress, "MercuryArchive")]
public sealed class CompressMercuryArchiveCmdlet : PSAsyncCmdlet {
  private const string PATH_PARAMETER_SET = "Path";
  private const string FILES_PARAMETER_SET = "Files";

  [Inject(Required = true)]
  private IArchiver Archiver { get; } = default!;

  [Parameter(Mandatory = true)]
  public string Name { get; set; } = default!;

  [Parameter(Mandatory = false, ParameterSetName = PATH_PARAMETER_SET)]
  public string? InputPath { get; set; }

  [Parameter(Mandatory = false, ParameterSetName = PATH_PARAMETER_SET)]
  public string? OutputPath { get; set; }

  [Parameter(Mandatory = true, ParameterSetName = FILES_PARAMETER_SET)]
  public string[] Files { get; set; } = [];

  /// <inheritdoc />
  protected override async Task BeginProcessingAsync(CancellationToken cancellationToken = default) {
    switch (ParameterSetName) {
      case PATH_PARAMETER_SET: {
        InputPath ??= SessionState.Path.GetUnresolvedProviderPathFromPSPath(".");
        OutputPath ??= SessionState.Path.GetUnresolvedProviderPathFromPSPath(".");

        break;
      }
      case FILES_PARAMETER_SET: {
        if (Files.Length == 0) {
          throw new PSArgumentException("The files parameter set requires at least one file.");
        }

        break;
      }
      default: {
        throw new PSArgumentException("The parameter set is not supported.");
      }
    }

    await Task.CompletedTask;
  }

  /// <inheritdoc />
  protected override async Task ProcessRecordAsync(CancellationToken cancellationToken = default) {
    switch (ParameterSetName) {
      case PATH_PARAMETER_SET:
        await ProcessPathParameterSetAsync(cancellationToken);
        break;

      case FILES_PARAMETER_SET:
        await ProcessFilesParameterSetAsync(cancellationToken);
        break;

      default:
        throw new PSArgumentException("The parameter set is not supported.");
    }
  }

  private async Task ProcessPathParameterSetAsync(CancellationToken cancellationToken = default) {
    var inputDirectoryInfo = new DirectoryInfo(InputPath!);
    var outputDirectoryInfo = new DirectoryInfo(OutputPath!);
    var inputFiles = inputDirectoryInfo
      .GetFiles()
      .Where(fileInfo => !fileInfo.Name.StartsWith('.'))
      .Where(fileInfo => fileInfo.Extension is ".json" or ".ps1")
      .ToArray();

    await Archiver
      .For(outputDirectoryInfo, Name, inputFiles)
      .Compress(cancellationToken);
  }

  private async Task ProcessFilesParameterSetAsync(CancellationToken cancellationToken = default) {
    var outputDirectoryInfo = new DirectoryInfo(SessionState.Path.GetUnresolvedProviderPathFromPSPath("."));

    var inputFiles = Files
      .Select(filePath => new FileInfo(filePath))
      .ToArray();

    await Archiver
      .For(outputDirectoryInfo, Name, inputFiles)
      .Compress(cancellationToken);
  }
}
