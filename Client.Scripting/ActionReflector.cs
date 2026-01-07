using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using PayrollEngine.Client.Model;

namespace PayrollEngine.Client.Scripting;

/// <summary>
/// Action reflector
/// </summary>
public static class ActionReflector
{
    /// <summary>
    /// Load action infos from assembly file
    /// </summary>
    /// <param name="assemblyName">Assembly file name</param>
    /// <returns>Assembly and list of action infos.</returns>
    public static (Assembly Assembly, List<ActionInfo> Actions) LoadFrom(string assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName))
        {
            throw new ArgumentException(nameof(assemblyName));
        }
        if (!File.Exists(assemblyName))
        {
            throw new PayrollException($"Invalid action assembly file {assemblyName}");
        }

        var assembly = Assembly.LoadFrom(assemblyName);
        var actions = LoadFrom(assembly);
        return (assembly, actions);
    }

    /// <summary>
    /// Load action infos from assembly
    /// </summary>
    /// <param name="assembly">Assembly</param>
    /// <returns>List of action infos.</returns>
    public static List<ActionInfo> LoadFrom(Assembly assembly)
    {
        if (assembly == null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        // setup action cache
        var actions = new List<ActionInfo>();

        // each type
        var types = assembly.GetTypes().Where(x => IsFunctionType(x) && !x.IsGenericType && !x.IsNested);
        foreach (var type in types)
        {
            // each method
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                // action attribute
                var actionAttribute = GetActionAttribute(method);
                if (actionAttribute == null)
                {
                    continue;
                }

                // action attribute
                var actionInfo = new ActionInfo(type)
                {
                    Name = GetPropertyValue<string>(actionAttribute,
                        nameof(ActionAttribute.Name)),
                    Description = GetPropertyValue<string>(actionAttribute,
                        nameof(ActionAttribute.Description)),
                    Categories = [.. GetPropertyValue<string[]>(actionAttribute,
                        nameof(ActionAttribute.Categories))],
                    Parameters = [],
                    Issues = []
                };
                actions.Add(actionInfo);

                // action parameter attributes (optional)
                var parameterAttributes = GetActionParameterAttributes(method,
                    typeof(ActionParameterAttribute));
                if (parameterAttributes != null)
                {
                    foreach (var parameterAttribute in parameterAttributes)
                    {
                        var name = GetPropertyValue<string>(parameterAttribute,
                            nameof(ActionParameterAttribute.Name));

                        // test parameter
                        if (!method.GetParameters().Any(x => string.Equals(x.Name, name)))
                        {
                            throw new PayrollException($"Invalid action parameter {actionInfo.Name}.{name}");
                        }

                        var actionParameterInfo = new ActionParameterInfo
                        {
                            Name = name,
                            Description = GetPropertyValue<string>(parameterAttribute,
                                nameof(ActionParameterAttribute.Description)),
                            ValueReferences = [],
                            ValueSources = [],
                            ValueTypes = []
                        };

                        var valueReferences = GetPropertyValue<string[]>(parameterAttribute,
                            nameof(ActionParameterAttribute.ValueReferences));
                        if (valueReferences != null)
                        {
                            actionParameterInfo.ValueReferences.AddRange(valueReferences);
                        }
                        var valueSources = GetPropertyValue<string[]>(parameterAttribute,
                            nameof(ActionParameterAttribute.ValueSources));
                        if (valueSources != null)
                        {
                            actionParameterInfo.ValueSources.AddRange(valueSources);
                        }
                        var valueTypes = GetPropertyValue<string[]>(parameterAttribute,
                            nameof(ActionParameterAttribute.ValueTypes));
                        if (valueTypes != null)
                        {
                            actionParameterInfo.ValueTypes.AddRange(valueTypes);
                        }

                        actionInfo.Parameters.Add(actionParameterInfo);
                    }
                }

                // action issue attributes (optional)
                var issuesAttributes = GetActionParameterAttributes(method, typeof(ActionIssueAttribute));
                if (issuesAttributes != null)
                {
                    foreach (var issueAttribute in issuesAttributes)
                    {
                        actionInfo.Issues.Add(new()
                        {
                            Name = GetPropertyValue<string>(issueAttribute,
                                nameof(ActionIssueAttribute.Name)),
                            Message = GetPropertyValue<string>(issueAttribute,
                                nameof(ActionIssueAttribute.Message)),
                            ParameterCount = GetPropertyValue<int>(issueAttribute,
                                nameof(ActionIssueAttribute.ParameterCount))
                        });
                    }
                }
            }
        }

        // order by action name
        actions.Sort((x, y) => string.CompareOrdinal(x.Name, y.Name));
        return actions;
    }

    /// <summary>
    /// Test for function type
    /// </summary>
    /// <param name="type">Type to test</param>
    private static bool IsFunctionType(Type type)
    {
        while (type != null)
        {
            if (type == typeof(Function.Function))
            {
                return true;
            }
            type = type.BaseType;
        }
        return false;
    }

    /// <summary>
    /// Get action attribute
    /// </summary>
    /// <param name="method">Action method</param>
    private static Attribute GetActionAttribute(MemberInfo method)
    {
        var actionAttributeName = nameof(ActionAttribute);
        var actionAttributeNamespace = typeof(ActionAttribute).Namespace;
        if (actionAttributeNamespace == null)
        {
            return null;
        }
        foreach (var methodAttribute in method.GetCustomAttributes())
        {
            // action attribute type
            var methodTypeName = methodAttribute.GetType().FullName;
            if (methodTypeName != null &&
                methodTypeName.StartsWith(actionAttributeNamespace) &&
                methodTypeName.EndsWith(actionAttributeName))
            {
                return methodAttribute;
            }
        }
        return null;
    }

    /// <summary>
    /// Get action parameters attribute
    /// </summary>
    /// <param name="method">Action method</param>
    /// <param name="attributeType">Attribute type</param>
    private static List<Attribute> GetActionParameterAttributes(MemberInfo method, Type attributeType)
    {
        var attributes = new List<Attribute>();
        var attributeName = attributeType.FullName;
        foreach (var methodAttribute in method.GetCustomAttributes())
        {
            // matching type
            if (string.Equals(attributeName, methodAttribute.GetType().FullName))
            {
                attributes.Add(methodAttribute);
            }
        }
        return attributes.Any() ? attributes : null;
    }

    /// <summary>
    /// Get property value
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="source">Source object</param>
    /// <param name="propertyName">Property name</param>
    private static T GetPropertyValue<T>(object source, string propertyName)
    {
        if (source == null)
        {
            return default;
        }
        var property = source.GetType().GetProperty(propertyName);
        if (property == null)
        {
            return default;
        }
        return (T)property.GetValue(source);
    }
}