/* CaseObject */

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting;

/// <summary>Case object attribute</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class CaseObjectAttribute(string ns) : Attribute
{
    /// <summary>Object namespace</summary>
    public string Ns { get; } = ns;
}

/// <summary>Case field ignore attribute</summary>
[AttributeUsage(AttributeTargets.Property)]
public class CaseFieldIgnoreAttribute : Attribute;

/// <summary>Case object extension methods</summary>
public static class CaseObjectExtensions
{
    /// <summary>Get member namespace</summary>
    /// <param name="member">Member</param>
    public static string GetNamespace(this MemberInfo member) =>
        (member.GetCustomAttribute(typeof(CaseObjectAttribute)) as CaseObjectAttribute)?.Ns;

    /// <summary>Get case field name</summary>
    /// <param name="property">Property</param>
    public static string GetCaseFieldName(this PropertyInfo property) => 
        property.GetNamespace() + property.Name;
}

/// <summary>Case object</summary>
public interface ICaseObject
{
    /// <summary>Get case object by property name</summary>
    T GetObject<T>(string propertyName) where T : class, ICaseObject;

    /// <summary>Get case objects</summary>
    List<T> GetObjects<T>() where T : class, ICaseObject;

    /// <summary>Get case field name</summary>
    string GetCaseFieldName(string propertyName);

    /// <summary>Get case object value by case field name</summary>
    object GetValue(string caseFieldName);

    /// <summary>Set case object value by case field name</summary>
    void SetValue(string caseFieldName, object value);
}

/// <summary>Case object/// </summary>
public abstract class CaseObject : ICaseObject
{

    #region Objects

    /// <inheritdoc />
    public T GetObject<T>(string propertyName) where T : class, ICaseObject =>
        GetObject<T>(GetProperty(propertyName), this);

    /// <inheritdoc />
    public List<T> GetObjects<T>() where T : class, ICaseObject
    {
        var objects = new List<T>();
        foreach (var propertyInfo in GetType().GetProperties())
        {
            var obj = GetObject<T>(propertyInfo, this);
            if (obj != null)
            {
                objects.Add(obj);
            }
        }
        return objects;
    }

    #endregion

    #region Case Field

    /// <inheritdoc />
    public string GetCaseFieldName(string propertyName) =>
        GetProperty(propertyName).GetCaseFieldName();

    /// <inheritdoc />
    public object GetValue(string caseFieldName)
    {
        var propertyObject = FindDeclaringObject(caseFieldName, null, this);
        if (propertyObject.Declaring == null)
        {
            throw new ScriptException($"Unknown object for case field {caseFieldName}");
        }
        return propertyObject.Property.GetValue(propertyObject.Declaring);
    }

    /// <inheritdoc />
    public void SetValue(string caseFieldName, object value)
    {
        var propertyObject = FindDeclaringObject(caseFieldName, null, this);
        if (propertyObject.Declaring == null)
        {
            throw new ScriptException($"Unknown object case field {caseFieldName}");
        }

        // read only property
        if (!propertyObject.Property.CanWrite)
        {
            return;
        }

        try
        {
            propertyObject.Property.SetValue(propertyObject.Declaring, value);
        }
        catch (Exception exception)
        {
            throw new ScriptException($"Case object field {caseFieldName} error value {value}.", exception);
        }
    }

    /// <summary>Get property by name</summary>
    /// <param name="propertyName">Property name</param>
    protected PropertyInfo GetProperty(string propertyName) =>
        GetProperty(GetType(), propertyName);

    /// <summary>Find declaring object with property using a recursive search</summary>
    /// <param name="caseFieldName">Case field name</param>
    /// <param name="namespace">The namespace</param>
    /// <param name="caseObject">Case object</param>
    private (PropertyInfo Property, ICaseObject Declaring) FindDeclaringObject(string caseFieldName, string @namespace, ICaseObject caseObject)
    {
        // fallback namespace
        if (string.IsNullOrWhiteSpace(@namespace))
        {
            @namespace = caseObject.GetType().GetNamespace();
        }

        var properties = GetProperties(caseObject.GetType(), recursive: false);
        foreach (var property in properties)
        {
            var propertyCaseFieldName = @namespace + property.Property.Name;

            // matching property with case object
            if (string.Equals(propertyCaseFieldName, caseFieldName))
            {
                return (property.Property, caseObject);
            }

            // child object
            if (property.Property.GetValue(caseObject) is ICaseObject childValue)
            {
                // recursive call
                var childProperty = FindDeclaringObject(caseFieldName, property.Property.GetNamespace(), childValue);
                if (childProperty.Property != null)
                {
                    return childProperty;
                }
            }
        }
        return new(null, null);
    }

    #endregion

    #region Case Field Reflection

    /// <summary>Get case field name</summary>
    public static string GetCaseFieldName<T>(string propertyName)
        where T : class, ICaseObject =>
        GetProperty(typeof(T), propertyName)?.GetCaseFieldName();

    /// <summary>Get case field names</summary>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <param name="writeable">Writeable properties only</param>
    public static List<string> GetCaseFieldNames<T>(bool recursive = true, bool writeable = false)
        where T : class, ICaseObject =>
        GetProperties(typeof(T), recursive, writeable).Select(x => x.CaseFieldName).ToList();

    #endregion

    #region Property Reflection

    /// <summary>Get object properties</summary>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <param name="writeable">Writeable properties only</param>
    public static List<(PropertyInfo Property, string CaseFieldName)> GetProperties<T>(bool recursive = true, bool writeable = false)
        where T : class, ICaseObject =>
        GetProperties(typeof(T), recursive, writeable);

    /// <summary>Get object properties</summary>
    /// <param name="type">Object type</param>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <param name="writeable">Writeable properties only</param>
    public static List<(PropertyInfo Property, string CaseFieldName)> GetProperties(Type type, bool recursive = true,
        bool writeable = false) =>
        FindProperties(type, recursive, writeable, type.GetNamespace());

    /// <summary>Find object properties with recursive search</summary>
    /// <param name="type">Object type</param>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <param name="writeable">Writeable properties only</param>
    /// <param name="namespace">The namespace</param>
    private static List<(PropertyInfo Property, string CaseFieldName)> FindProperties(Type type, bool recursive,
        bool writeable, string @namespace)
    {
        // fallback namespace
        if (string.IsNullOrWhiteSpace(@namespace))
        {
            @namespace = type.GetNamespace();
        }

        var properties = new List<(PropertyInfo Property, string CaseFieldName)>();
        foreach (var propertyInfo in type.GetProperties())
        {
            if (recursive && IsObjectProperty(propertyInfo))
            {
                // child object, recursive call
                properties.AddRange(FindProperties(propertyInfo.PropertyType, true, writeable, propertyInfo.GetNamespace()));
            }
            else if (IsFieldProperty(propertyInfo, writeable))
            {
                // case field
                properties.Add(new(propertyInfo, @namespace + propertyInfo.GetCaseFieldName()));
            }
        }
        return properties;
    }

    private static PropertyInfo GetProperty(Type type, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException(nameof(propertyName));
        }
        var property = type.GetProperty(propertyName);
        if (property == null)
        {
            throw new ScriptException($"Unknown property {propertyName} in type {type}.");
        }
        return IgnoredProperty(property) ? null : property;
    }

    #endregion

    #region Object Reflection

    private static T GetObject<T>(PropertyInfo property, object source) where T : class, ICaseObject =>
        IsObjectProperty(property) ? property.GetValue(source) as T : null;

    private static bool IgnoredProperty(PropertyInfo property) =>
        property.GetCustomAttribute(typeof(CaseFieldIgnoreAttribute)) != null;

    private static bool IsObjectProperty(PropertyInfo property)
    {
        // ignored property
        if (IgnoredProperty(property))
        {
            return false;
        }

        // read property
        if (!property.CanRead)
        {
            return false;
        }

        // missing public get
        var method = property.GetGetMethod(nonPublic: true);
        if (method == null || !method.IsPublic)
        {
            return false;
        }

        // matching property type
        return typeof(ICaseObject).IsAssignableFrom(property.PropertyType);
    }

    private static bool IsFieldProperty(PropertyInfo property, bool writeable)
    {
        // ignored property
        if (IgnoredProperty(property))
        {
            return false;
        }

        // read property
        if (!property.CanRead)
        {
            return false;
        }

        // missing public get
        var getMethod = property.GetGetMethod(nonPublic: true);
        if (getMethod == null || !getMethod.IsPublic)
        {
            return false;
        }

        // writeable
        if (writeable && !property.CanWrite)
        {
            return false;
        }
        // write property
        if (property.CanWrite)
        {
            // missing public set
            var setMethod = property.GetSetMethod(nonPublic: true);
            if (setMethod == null || !setMethod.IsPublic)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

}
