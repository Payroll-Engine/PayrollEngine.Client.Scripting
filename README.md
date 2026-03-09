# Payroll Engine Client Scripting

> Part of the [Payroll Engine](https://github.com/Payroll-Engine/PayrollEngine) open-source payroll automation framework.
> Full documentation at [payrollengine.org](https://payrollengine.org).

The scripting library is the shared contract between the backend scripting runtime and client-side tooling. It defines all scripting functions, runtime interfaces, No-Code action syntax, and script parsing infrastructure used by both the backend Roslyn compiler and the [Client Services](https://github.com/Payroll-Engine/PayrollEngine.Client.Services) SDK.

Typical use cases:
- Write **No-Code actions** in regulation YAML/JSON without any programming knowledge
- Write **Low-Code scripts** in C# to implement custom payroll rules
- Use the **CaseObject** pattern to map C# types to regulation cases
- Reference the scripting API in **external function libraries** compiled with the SDK

---

## Function Hierarchy

All scripting functions derive from a common base hierarchy. Each function gives access to a progressively richer API surface:

```
Function
└── PayrollFunction          (payroll, employee, case values, lookups, periods, cycle)
    ├── CaseFunction         (case field access)
    │   ├── CaseChangeFunction
    │   │   ├── CaseAvailableFunction
    │   │   ├── CaseBuildFunction
    │   │   └── CaseValidateFunction
    │   └── CaseRelationFunction
    │       ├── CaseRelationBuildFunction
    │       └── CaseRelationValidateFunction
    ├── PayrunFunction       (payrun name, retro, forecast)
    │   ├── PayrunStartFunction
    │   ├── PayrunEndFunction
    │   ├── PayrunEmployeeAvailableFunction
    │   ├── PayrunEmployeeStartFunction
    │   ├── PayrunEmployeeEndFunction
    │   └── PayrunWageTypeAvailableFunction
    ├── CollectorFunction    (collector result, threshold, min/max)
    │   ├── CollectorStartFunction
    │   ├── CollectorApplyFunction
    │   └── CollectorEndFunction
    ├── WageTypeFunction     (wage type number, name, calendar)
    │   ├── WageTypeValueFunction
    │   └── WageTypeResultFunction
    └── ReportFunction
        ├── ReportBuildFunction
        ├── ReportStartFunction
        └── ReportEndFunction
```

All functions at a given level have access to the full API of their parent. For example, `WageTypeValueFunction` provides period navigation, case value access, and lookups from `PayrollFunction`, payrun properties from `PayrunFunction`, and wage type specifics.

---

## Runtime Compilation Model

All C# source files that make up the scripting API are compiled into the assembly as **embedded resources**. The backend Roslyn compiler extracts them at runtime to build the compilation context in which user scripts are evaluated — no separate SDK installation is required on the server.

The embedded files are grouped into three categories:

**Script core** — types available in every script context:

| File | Description |
|:--|:--|
| `ClientScript.cs` | Script base and host interface |
| `Cache.cs` | Per-script caching helpers |
| `Extensions.cs` | General extension methods |
| `Tools.cs` | Utility functions |
| `Date.cs` | Date helpers |
| `DatePeriod.cs` | Date range type |
| `HourPeriod.cs` | Hour-based period type |
| `CaseObject.cs` | `CaseObject` base class and `[CaseObjectAttribute]` |
| `PayrollValue.cs` | Nullable payroll value wrapper |
| `PeriodValue.cs` | Value scoped to a calendar period |
| `CaseValue.cs` | Case field value access |
| `CasePayrollValue.cs` | Case value as `PayrollValue` |
| `PayrollResults.cs` | Wage type and collector result collections |

**Function implementations** — one file per function class in the hierarchy (e.g., `Function\WageTypeValueFunction.cs`). These define the API surface available in each script type.

**No-Code Action extensions** — partial-class files (`Function\*.Action.cs`) that add No-Code action support to each function class. These are included only when action expressions are compiled.

**Report** — `Report\Report.cs`, the base type for report scripts.

This model means the scripting API seen by user scripts is always identical to the version shipped in the NuGet package — version drift between the compilation context and the library is not possible.

---

## Low-Code Scripting

Scripts are C# expressions embedded directly in regulation JSON or YAML. No .NET project or build toolchain is required — the backend compiles them at runtime using Roslyn. The scripting library provides the API available inside scripts.

### Wage Type Value

```csharp
// Simple case value
Employee["Salary"]

// Conditional result
(decimal)Employee["Salary"] > 0 ? Employee["Salary"] : PayrollValue.Empty

// Year-to-date from previous payruns
WageType[2300].Cycle + WageType[2300]

// Average over the last 3 completed jobs
GetWageTypeResults(2300, new PeriodResultQuery(3, PayrunJobStatus.Complete))
    .DefaultIfEmpty().Average()
```

### Case Build / Validate

```csharp
// Set a computed field value
SetFieldValue("NetSalary", (decimal)Employee["GrossSalary"] * 0.8m);

// Validate input
if ((decimal)Field["Salary"] < 500)
    AddIssue("SalaryTooLow");
```

### Payrun Lifecycle

```csharp
// PayrunStart / PayrunEnd: set runtime values for use across wage types
SetRuntimeValue("MinSalary", GetRangeLookup<decimal>("MinWage", PeriodStart.Year));
```

### External Function Libraries

For complex logic, implement functions in a separate C# project using `partial` classes:

```csharp
// MyRegulation.Scripts/CompositeWageTypeValue.cs
public class CompositeWageTypeValue(WageTypeValueFunction function)
{
    public decimal GetAdjustedSalary() =>
        (decimal)function.Employee["Salary"] * GetAdjustmentFactor();
    // ...
}

// Extend WageTypeValueFunction with partial class
public partial class WageTypeValueFunction
{
    private CompositeWageTypeValue composite;
    public CompositeWageTypeValue MyReg => composite ??= new(this);
}
```

Usage in regulation JSON:
```json
"valueExpression": "MyReg.GetAdjustedSalary()"
```

---

## CaseObject Pattern

`CaseObject` maps a C# class to a set of case fields, enabling strongly-typed access to structured case data without repeating field name strings throughout scripts.

```csharp
[CaseObject("HR.")]
public class EmploymentContract : CaseObject
{
    public decimal Salary { get; set; }        // maps to "HR.Salary"
    public DateTime EntryDate { get; set; }    // maps to "HR.EntryDate"
    public string Department { get; set; }     // maps to "HR.Department"
}
```

Access in a wage type script:
```csharp
var contract = GetRawCaseObject<EmploymentContract>(EvaluationDate);
return contract.Salary * GetLookup<decimal>("BonusFactor", contract.Department);
```

The `[CaseObjectAttribute]` sets the namespace prefix. `[CaseFieldIgnore]` excludes a property from mapping.

---

## Script Parsers

The `Script` folder contains parsers used by the Payroll Console and CI tooling to extract, import, and rebuild regulation scripts from source files.

| Parser | Regulation Object |
|:--|:--|
| `CaseScriptParser` | Case scripts (Available, Build, Validate) |
| `CaseRelationScriptParser` | Case relation scripts |
| `CollectorScriptParser` | Collector scripts (Start, Apply, End) |
| `WageTypeScriptParser` | Wage type scripts (Value, Result) |
| `PayrunScriptParser` | Payrun lifecycle scripts |
| `ReportScriptParser` | Report scripts (Build, Start, End) |

The `IScriptParser` interface is the entry point used by `ExchangeImport` in Client Services.

---

## No-Code Actions

Actions control the behaviour of payroll objects using text expressions in regulation JSON/YAML — no programming knowledge required. The following events can be controlled:

- **Case** — `Available`, `Build`, `Validate`
- **CaseRelation** — `Build`, `Validate`
- **Collector** — `Start`, `Apply`, `End`
- **WageType** — `Value`, `Result`

Actions support references to case values (`^^`), case fields (`^:`), lookups (`^#`), runtime values (`^\|`), wage type values (`^$`), and collector values (`^&`), as well as conditions, operators, type conversions, and a set of built-in functions.

The full No-Code action reference is available in the documentation:
👉 [No-Code / Low-Code Development](https://payrollengine.org/roles/regulator/no-code-low-code-development)

The full list of built-in actions is listed in the [Scripting API reference](https://github.com/Payroll-Engine/PayrollEngine/blob/main/docs/PayrollEngine.Client.Scripting.md).

Custom actions can be added via low-code; see [Custom Actions](https://payrollengine.org/roles/regulator/automation#custom-actions).

---

## Developer Reference

The HTML scripting reference is generated with [docfx](https://github.com/dotnet/docfx). The following commands are available in the `docfx` folder:

| Command | Description |
|:--|:--|
| `Static.Build` | Build static HTML content (output: `_site/`) |
| `Static.Start` | Open the static help (`_site/index.html`) |
| `Server.Start` | Serve on `http://localhost:5865/` with dark mode support |

---

## NuGet Package

Available on [NuGet.org](https://www.nuget.org/profiles/PayrollEngine):

```sh
dotnet add package PayrollEngine.Client.Scripting
```

---

## Build

Environment variable used during build:

| Variable | Description |
|:--|:--|
| `PayrollEnginePackageDir` | Output directory for the NuGet package (optional) |

---

## Third Party Components
- Documentation generation with [docfx](https://github.com/dotnet/docfx/) — license `MIT`
- C# compilation with [Microsoft.CodeAnalysis.CSharp](https://github.com/dotnet/roslyn) — license `MIT`
- Tests with [xunit](https://github.com/xunit) — license `Apache 2.0`

---

## See Also

- [No-Code / Low-Code Development](https://payrollengine.org/roles/regulator/no-code-low-code-development) — regulation authoring guide
- [Regulation Design](https://payrollengine.org/roles/regulator/regulation-design) — structuring regulation layers
- [Client Services](https://github.com/Payroll-Engine/PayrollEngine.Client.Services) — SDK using this scripting library
- [Client Test](https://github.com/Payroll-Engine/PayrollEngine.Client.Test) — test library for regulations
