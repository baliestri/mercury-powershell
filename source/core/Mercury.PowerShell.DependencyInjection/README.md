# Mercury.PowerShell.DependencyInjection

This package provides a simple way to deal with dependency injection in PowerShell.

## Note

This package was made for personal use, it was developed to .NET 8.0+ and not to .NET Standard. If you want to use it to be targeted to another
framework, you can open an issue, and we can discuss it.

## Installation

```powershell
dotnet add package Mercury.PowerShell.DependencyInjection
```

## Usage

First, mark your services as injectable with the `Injectable` attribute:

```csharp
using Mercury.PowerShell.DependencyInjection.Attributes;

namespace SomeNamespace.Services;

[Injectable(Lifetime = ServiceLifetime.Singleton)] // Default scope is Scoped
public sealed class MyFirstService {
    public void DoSomething() {
        // Do something
    }
}

public interface IMySecondService {
    void DoAnotherThing();
}

[Injectable(ServiceType = typeof(IMySecondService))]
public sealed class MySecondService : IMySecondService {
    public void DoAnotherThing() {
        // Do another thing
    }
}
```

Or, if you prefer, you can use the `IServicePipelines` interface to define your services:

**PS**: This one should also be used to inject services that are not possible to be marked with the `Injectable` attribute.

```csharp
using Mercury.PowerShell.DependencyInjection.Abstractions;

namespace SomeNamespace.Pipelines;

public sealed class MyServiceCollectionPipeline : IServiceCollectionPipeline {
    public void Register(IServiceCollection serviceCollection) {
        serviceCollection.AddSingleton<MyFirstService>();
        serviceCollection.AddScoped<IMySecondService, MySecondService>();

        serviceCollection.AddDatabase("MyDatabase.db3"); // Just an example
    }
}
```

Then, create a cmdlet that will use the services:

```csharp
using Mercury.PowerShell.DependencyInjection;

namespace SomeNamespace.Cmdlets;

[Cmdlet(VerbsCommon.Get, "Something")]
public sealed class GetSomethingCmdlet : PSAsyncCmdlet {
    [Inject]
    private MyFirstService _firstService = default!; // As field, default! is used to suppress the warning

    [Inject(Required = true)] // Throws an exception if the service is not found
    private IMySecondService SecondService { get; } = default!; // As property, the setter is not necessary

    // Or ProcessRecordAsync, if you prefer
    protected override async Task BeginProcessingAsync(CancellationToken cancellationToken = default) {
        _firstService.DoSomething();
        SecondService.DoAnotherThing();

        await Task.CompletedTask;
    }
}
```

The services will be automatically injected into the cmdlet.

## License

This project is licensed under the MIT License - see the [LICENSE](../../../LICENSE.md) file for details.
