# Nzb [![Build status](https://ci.appveyor.com/api/projects/status/y6l7t6xfjb1dtb7j/branch/master?svg=true)](https://ci.appveyor.com/project/khellang/nzb/branch/master)

A tiny library for parsing NZB documents with .NET. The NZB Format Specification is [available here](http://wiki.sabnzbd.org/nzb-specs).

The library is implemented using .NET Standard 1.0, with support for the following platforms (minimum):

 - .NET Framework 4.5
 - Windows 8
 - Windows Phone 8.1
 - Windows Phone Silverlight 8
 - Xamarin.Android
 - Xamarin.iOS

It is shipped as [a NuGet package](https://www.nuget.org/packages/Nzb).

To install it, simply search for `Nzb` in the Visual Studio Package Manager window, or write

> Install-Package Nzb

In the Package Manager Console.

## Usage

Using the library couldn't be more simple. There are two methods to call:

 - `NzbDocument.Load` - Loads a document from the specified `Stream`, optionally using a specified `Encoding`.
 - `NzbDocument.Parse` - Parses a document from the specified `string`.

The library consists of four public types:

 1. `NzbDocument` - Represents an NZB document. This is the type returned from `NzbDocument.Load` or `NzbDocument.Parse`.
 2. `NzbFile` - Represents a file linked in the NZB document.
 3. `NzbSegment` - Represents one (of potentially many) segment(s) that makes up an `NzbFile`.

## Example

Here's a quick example on how to use it (it's embarrassingly simple):

```csharp
public static class Program
{
    public static void Main(string[] args)
    {
        MainAsync(args).Wait();
    }

    public static async Task MainAsync(string[] args)
    {
        using (var documentStream = File.OpenRead("file.nzb"))
        {
            var document = await NzbDocument.Load(documentStream);

            // Access document properties here...
        }
    }
}
```

## Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=khellang&utm_medium=Nzb) and [Dapper Plus](https://dapper-plus.net/?utm_source=khellang&utm_medium=Nzb) are major sponsors and proud to contribute to the development of Nzb.

[![Entity Framework Extensions](https://raw.githubusercontent.com/khellang/khellang/refs/heads/master/.github/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=khellang&utm_medium=Nzb)

[![Dapper Plus](https://raw.githubusercontent.com/khellang/khellang/refs/heads/master/.github/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=khellang&utm_medium=Nzb)
