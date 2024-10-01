# Mercury.PowerShell.Storage

This one has `PowerShell` in the name, but I'm unsure about this. I pretend to use it to store data
through [Mercury.PowerShell.DependencyInjection](../Mercury.PowerShell.DependencyInjection) injection, but it does not
specifically use PowerShell. It's just a storage library.

## Note

This package was made for personal use, it was developed to .NET 8.0+ and not to .NET Standard. If you want to use it to be targeted to another
framework, you can open an issue, and we can discuss it.

## Installation

```powershell
dotnet add package Mercury.PowerShell.DependencyInjection
dotnet add package Mercury.PowerShell.Storage
```

## Usage

First, you need to register the storage service in the `IServiceCollection`:

```csharp
using Mercury.PowerShell.DependencyInjection.Abstractions;
using Mercury.PowerShell.Storage.Extensions;

namespace SomeNamespace.Pipelines;

public sealed class MyStoragePipeline : IServicePipelines {
    public void Register(IServiceCollection serviceCollection)
        => serviceCollection.AddStorage(options => options
            .WithName("Mercury.PowerShell.ArgumentCompleter.db3")
            .Done()
        );
}
```

Now, you create an entity class that will be stored in the database:

```csharp
using Mercury.PowerShell.Storage.Abstractions;

namespace SomeNamespace.Entities;

public sealed class SomeEntity : Entity {
    public string Name { get; set; }
    public int Age { get; set; }
}
```

Then, you can use the `IReadOnlyRepository` or `IRepository` interfaces to interact with the database:

**PS**: If you want to deal directly with the SQLite connection, you can use the `IStorageProvider` interface.

```csharp
using Mercury.PowerShell.Storage.Abstractions;
using SomeNamespace.Entities;

namespace SomeNamespace.Services;

public sealed class MyService {
    private readonly IReadOnlyRepository<SomeEntity> _someEntityRepository;

    public MyService(IReadOnlyRepository<SomeEntity> someEntityRepository)
        => _repository = repository;

    public async Task DoSomething(CancellationToken cancellationToken = default) {
        var data = _repository.GetAsync(22, cancellationToken); // Using Id as an example
        var anotherData = _repository.GetAsync(e => e.Name == "SomeName", cancellationToken);

        // Do something with the data
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](../../../LICENSE.md) file for details.
