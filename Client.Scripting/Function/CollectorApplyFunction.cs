﻿/* CollectorApplyFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Apply value to a collector (default: wage type value)</summary>
/// <example>
/// <code language="c#">
/// // Example restricted input wage type value
/// System.Math.Max(WageTypeValue, 5000)
/// </code>
/// <code language="c#">
/// // Example restricted to a wage type
/// WageTypeNumber == 2250 ? WageTypeValue : null
/// </code>
/// <code language="c#">
/// // Example restricted to the collector summary
/// CollectorSum + WageTypeValue > 10000 ? 10000 - CollectorSum + WageTypeValue : WageTypeValue
/// </code>
/// </example>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CollectorApplyFunction : CollectorFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CollectorApplyFunction(object runtime) :
        base(runtime)
    {
        // wage type
        WageTypeNumber = Runtime.WageTypeNumber;
        WageTypeName = Runtime.WageTypeName;
        WageTypeValue = Runtime.WageTypeValue;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CollectorApplyFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>The wage type number</summary>
    public decimal WageTypeNumber { get; }

    /// <summary>The wage type name</summary>
    public string WageTypeName { get; }

    /// <summary>The wage type result value</summary>
    public decimal WageTypeValue { get; }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public decimal? GetValue()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}