# Payroll Engine Client Scripting
👉 This library is part of the [Payroll Engine](https://github.com/Payroll-Engine/PayrollEngine/wiki).

The scripting library that defines the commonality between the backend and the clients:
- Scripting functions
- Scripting runtime
- Script parsers
- System scripts

## HTML documentation
The client scripting library contains static HTML documentation for scripting developers. This is created using [docx](https://github.com/dotnet/docfx) [MIT]. The following commands are available in the `docfx` folder:
- `Static.Build` - builds the static HTML content (output is the `_site` subdirectory)
- `Static.Static` - opens the static help (`_site\index.html`)
- `Server.Start` - start the static help on http://localhost:5865/ (dark mode support)

## Third party components
- Documentation generation with [docfx](https://github.com/dotnet/docfx/) - license `MIT`
- Tests with [xunit](https://github.com/xunit) - license `Apache 2.0`

## Build
Supported runtime environment variables:
- *PayrollEnginePackageDir* - the NuGet package destination directory (optional)
