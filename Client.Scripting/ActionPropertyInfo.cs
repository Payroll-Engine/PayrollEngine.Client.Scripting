using System;

namespace PayrollEngine.Client.Scripting;

/// <summary>
/// Action property info
/// </summary>
public class ActionPropertyInfo
{
    /// <summary>The property name</summary>
    public FunctionType FunctionType { get; init; }

    /// <summary>The property name</summary>
    public string Name { get; init; }

    /// <summary>The property description</summary>
    public string Description { get; init; }

    /// <summary>The property type</summary>
    public Type Type { get; init; }

    /// <summary>Readonly property</summary>
    public bool ReadOnly { get; init; }
}