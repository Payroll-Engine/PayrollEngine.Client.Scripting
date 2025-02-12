﻿/* WageTypeResultFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Setup wage type results</summary>
/// <seealso cref="WageTypeValueFunction">Wage Type Value Function</seealso>
// ReSharper disable once PartialTypeWithSinglePart
public partial class WageTypeResultFunction : WageTypeFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public WageTypeResultFunction(object runtime) :
        base(runtime)
    {
        // wage type
        WageTypeValue = Runtime.WageTypeValue;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected WageTypeResultFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>The wage type value</summary>
    public decimal WageTypeValue { get; }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public object Result()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}