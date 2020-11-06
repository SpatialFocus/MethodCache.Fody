# SpatialFocus.MethodCache.Fody

A method cache [Fody](https://github.com/Fody/Home/) plugin

[![Nuget](https://img.shields.io/nuget/v/SpatialFocus.MethodCache.Fody)](https://www.nuget.org/packages/SpatialFocus.MethodCache.Fody/)
[![Build & Publish](https://github.com/SpatialFocus/MethodCache.Fody/workflows/Build%20&%20Publish/badge.svg)](https://github.com/SpatialFocus/MethodCache.Fody/actions)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FSpatialFocus%2FMethodCache.Fody.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FSpatialFocus%2FMethodCache.Fody?ref=badge_shield)

Caches return values of methods decorated with a `[Cache]` Attribute. Integrates with the .NET Extension [IMemoryCache](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.memory.imemorycache) interface.

## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).

### NuGet installation

Install the [SpatialFocus.MethodCache.Fody NuGet package](https://nuget.org/packages/SpatialFocus.MethodCache.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package SpatialFocus.MethodCache.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.

### Add to FodyWeavers.xml

Add `<SpatialFocus.MethodCache/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<Weavers>
    <SpatialFocus.MethodCache/>
</Weavers>
```

## Overview

Before code:

```csharp
[Cache]
public class BasicSample
{
    public BasicSample(IMemoryCache memoryCache)
    {
        MemoryCache = memoryCache;
    }

    // MethodCache.Fody will look for a property implementing
    // the Microsoft.Extensions.Caching.Memory.IMemoryCache interface
    protected IMemoryCache MemoryCache { get; }

    public int Add(int a, int b)
    {
        return a + b;
    }
}
```

What gets compiled

```csharp
[Cache]
public class BasicSample
{
    public BasicSample(IMemoryCache memoryCache)
    {
        MemoryCache = memoryCache;
    }

    protected IMemoryCache MemoryCache { get; }

    public int Add(int a, int b)
    {
        // Create a unique cache key, based on namespace, class name and method name as first parameter
        // and corresponding generic class parameters, generic method parameters and method parameters
        Tuple<string, int, int> key = new Tuple<string, int, int>("Namespace.BasicSample.Add", a, b);

        // Check and return if a cached value exists for key
        if (MemoryCache.TryGetValue(key, out int value))
        {
            return value;
        }

        // Before each return statement, save the value that would be returned in the cache
        value = a + b;
        MemoryCache.Set<int>(key, value);
        return value;
    }
}
```

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FSpatialFocus%2FMethodCache.Fody.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FSpatialFocus%2FMethodCache.Fody?ref=badge_large)

----

Made with :heart: by [Spatial Focus](https://spatial-focus.net/)