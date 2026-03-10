using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using PayrollEngine.Client.Scripting;

// args[0] = path to PayrollEngine.Client.Scripting.dll
if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: ActionDocGenerator <scripting-dll-path>");
    return 1;
}

var dllPath = Path.GetFullPath(args[0]);
if (!File.Exists(dllPath))
{
    Console.Error.WriteLine($"File not found: {dllPath}");
    return 1;
}

try
{
    Console.Error.WriteLine($"Loading: {Path.GetFileName(dllPath)}");
    var (_, actions) = ActionReflector.LoadFrom(dllPath);
    Console.Error.WriteLine($"Actions: {actions.Count}");

    var options = new JsonSerializerOptions
    {
        WriteIndented = false,
        Converters = { new JsonStringEnumConverter() }
    };
    Console.WriteLine(JsonSerializer.Serialize(actions, options));
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.GetType().Name}: {ex.Message}");
    return 1;
}
