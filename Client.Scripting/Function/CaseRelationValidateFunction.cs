/* CaseRelationValidateFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Validate related case, considering related source case values, see <see cref="CaseRelationFunction.SourceValue"/> and <see cref="CaseRelationFunction.HasSourceValue"/></summary>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CaseRelationValidateFunction : CaseRelationFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CaseRelationValidateFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseRelationValidateFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get case relation validate actions</summary>
    public string[] GetValidateActions() =>
        Runtime.GetValidateActions();

    /// <summary>Test for issues</summary>
    public bool HasIssues() => Runtime.HasIssues();

    /// <summary>Add a new case issue</summary>
    /// <param name="message">The issue message</param>
    /// <param name="number">The issue number</param>
    public void AddIssue(string message, int number = 0) =>
        Runtime.AddIssue(message, number);

    /// <summary>Add a new case field issue</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="message">The issue message</param>
    /// <param name="number">The issue number</param>
    public void AddIssue(string caseFieldName, string message, int number = 0) =>
        Runtime.AddIssue(caseFieldName, message, number);

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Validate()
    {
        if (!InvokeValidateActions())
        {
            return false;
        }

        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion

        // compiler will optimize this out if the code provides a return
        return null;
    }

    private bool InvokeValidateActions()
    {
        var context = new CaseRelationActionContext(this);
        foreach (var action in GetValidateActions())
        {
            InvokeConditionAction<CaseRelationActionContext, CaseRelationValidateActionAttribute>(context, action);
            if (!context.HasIssues)
            {
                continue;
            }
            context.Issues.ForEach(x => AddIssue(x.Message));
            return false;
        }
        return true;
    }
}