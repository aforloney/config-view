# config-view

## Purpose

An extension method for iterating and viewing the contents of your configuration as an endpoint.

This was inspired off of an extenstion method on `IConfigurationRoot` exposed by the .NET team to _debug view_ the pieced together configuration values for the running application across all possible configured providers.

However, the extension method returned the contents as a `string` and required manual work to expose the contents behind an endpoint. This aims to workaorund that.

## Usage

The purpose of the repository was to be able to leverage easily viewing the contents of your configuration with the following lines,

``` C#
app.AddConfigEndpoint();
...
services.AddConfigEndpoint();
```

## Notes

This is not meant to be Production facing in it's current state and future support will only
add the extension for non-Production environments.

