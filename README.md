# config-view

![Publish Packages](https://github.com/aforloney/config-view/actions/workflows/dotnet.yml/badge.svg)

## Purpose

An extension method for iterating and viewing the contents of your configuration as an endpoint.

This was inspired off of an extenstion method on `IConfigurationRoot` exposed to _debug view_ the pieced together configuration values for the application across all possible configured providers.

However, the extension method returned all of the contents as a `string` and required manual work to expose the contents behind an endpoint. This aims to workaorund that and provide the ability to specify configuration provider to view (e.g, JSON settings, memory, environment variables, etc).

## Usage

The purpose of the repository was to easily and quickly view the contents of your configuration with as little lines of code as possible. Currently, you can register either viewing _all_ confgiuration provider types or _JSON_ specific through extension methods. See below for more information:

### ASP.NET Core 5
``` C#
if (app.Environment.IsDevelopment())
{
    app.AddConfigEndpoint();
    ...
}
```

### ASP.NET Core 6
``` C#
if (app.Environment.IsDevelopment())
{
    app.AddConfigEndpoint(builder); // where Builder is of WebApplicationBuilder
    ...
}
```

And then register the appropriate type of configuration you wish to view,

``` C#
// for all type of providers,
services.AddConfigEndpoint();
// or JSON only,
services.AddJsonConfigEndpoint();
// or Environment Variables only,
services.AddEnvironmentConfigEndpoint();
// or Memory Configuration only,
services.AddMemoryConfigEndpoint();
```

Once added, the application will expose a new `GET` endpoint called `/config` to display the contents of the specific configuration provider that was used earlier when registering the service.
