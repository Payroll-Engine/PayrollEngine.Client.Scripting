/* CollectorEndFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>End the collector</summary>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CollectorEndFunction : CollectorFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CollectorEndFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CollectorEndFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get collector values</summary>
    public decimal[] GetValues() => Runtime.GetValues();

    /// <summary>Set collector values</summary>
    public void SetValues(decimal[] values) => Runtime.SetValues(values);

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public object End()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}