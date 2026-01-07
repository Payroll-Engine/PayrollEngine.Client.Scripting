using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using PayrollEngine.Client.Scripting.Function;

namespace PayrollEngine.Client.Scripting;

/// <summary>
/// Script action property provider
/// </summary>
public static class ScriptPropertyProvider
{
    private static readonly Dictionary<FunctionType, Type> FunctionProperties = new()
    {
        { FunctionType.CaseAvailable, typeof(CaseAvailableFunction) },
        { FunctionType.CaseBuild, typeof(CaseBuildFunction) },
        { FunctionType.CaseValidate, typeof(CaseValidateFunction) },
        { FunctionType.CaseRelationBuild, typeof(CaseRelationBuildFunction) },
        { FunctionType.CaseRelationValidate, typeof(CaseRelationValidateFunction) },
        { FunctionType.CollectorStart, typeof(CollectorStartFunction) },
        { FunctionType.CollectorApply, typeof(CollectorApplyFunction) },
        { FunctionType.CollectorEnd, typeof(CollectorEndFunction) },
        { FunctionType.WageTypeValue, typeof(WageTypeValueFunction) },
        { FunctionType.WageTypeResult, typeof(WageTypeResultFunction) }
    };

    /// <summary>Get function properties names by function type</summary>
    /// <param name="functionType">The function type</param>
    /// <param name="readOnly">Read only properties (default: true)</param>
    public static List<ActionPropertyInfo> GetProperties(FunctionType functionType, bool readOnly = true)
    {
        if (functionType == default)
        {
            return [];
        }

        var properties = new List<ActionPropertyInfo>();
        foreach (var functionProperty in FunctionProperties)
        {
            var propertyFunctionType = functionProperty.Key;
            if (!functionType.HasFlag(propertyFunctionType))
            {
                continue;
            }

            // type properties
            var type = functionProperty.Value;
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // read-only filter
                if (readOnly && property.CanWrite)
                {
                    continue;
                }

                // action property attribute
                var actionProp = property.GetCustomAttribute<ActionPropertyAttribute>();
                if (actionProp == null)
                {
                    continue;
                }

                // action property info
                properties.Add(new ActionPropertyInfo
                {
                    FunctionType = propertyFunctionType,
                    Name = actionProp.Name ?? property.Name,
                    Description = actionProp.Description,
                    Type = property.PropertyType,
                    ReadOnly = !property.CanWrite
                });
            }
        }

        // properties ordered by name
        return properties.OrderBy(x => x.Name).ToList();
    }
}