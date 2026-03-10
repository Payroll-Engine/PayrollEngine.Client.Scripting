# Client Scripting

The **PayrollEngine.Client.Scripting** library defines the complete scripting contract between the
Payroll Engine backend and client tooling. It contains all scripting function classes, runtime
interfaces, No-Code action infrastructure, and script parsing utilities.

All C# source files are embedded as resources in the assembly. The backend Roslyn compiler extracts
them at runtime to build the compilation context in which payroll scripts are evaluated — no separate
SDK installation is required on the server.

---

## Scripting Functions

All scripting functions are defined in the
[PayrollEngine.Client.Scripting.Function](api/PayrollEngine.Client.Scripting.Function.html) namespace
and inherit from a common base hierarchy.

```
Function
├── PayrollFunction
│   ├── CaseFunction
│   │   ├── CaseAvailableFunction
│   │   └── CaseChangeFunction
│   │       ├── CaseBuildFunction
│   │       └── CaseValidateFunction
│   ├── CaseRelationFunction
│   │   ├── CaseRelationBuildFunction
│   │   └── CaseRelationValidateFunction
│   └── PayrunFunction
│       ├── PayrunStartFunction
│       ├── PayrunEndFunction
│       ├── PayrunEmployeeAvailableFunction
│       ├── PayrunEmployeeStartFunction
│       ├── PayrunEmployeeEndFunction
│       ├── PayrunWageTypeAvailableFunction
│       ├── CollectorFunction
│       │   ├── CollectorStartFunction
│       │   ├── CollectorApplyFunction
│       │   └── CollectorEndFunction
│       └── WageTypeFunction
│           ├── WageTypeValueFunction
│           └── WageTypeResultFunction
└── ReportFunction
    ├── ReportBuildFunction
    ├── ReportStartFunction
    └── ReportEndFunction
```

### Case Functions

Case functions control the lifecycle of a case input form — from visibility through field population
to final validation.

| Function | Base | Description |
|:--|:--|:--|
| [CaseAvailableFunction](api/PayrollEngine.Client.Scripting.Function.CaseAvailableFunction.html) | `CaseFunction` | Determines whether a case is offered for input. Returning `false` hides the case from the user entirely. Typical use: role-based or condition-based availability. |
| [CaseBuildFunction](api/PayrollEngine.Client.Scripting.Function.CaseBuildFunction.html) | `CaseChangeFunction` | Populates or pre-fills case fields before the form is displayed. Used to set default values, apply lookups, or derive field values from existing case data. |
| [CaseValidateFunction](api/PayrollEngine.Client.Scripting.Function.CaseValidateFunction.html) | `CaseChangeFunction` | Validates case field values when the user submits the form. Can add validation issues that are shown inline. |

### Case Relation Functions

Case relation functions control how values are transferred and validated when one case references
another via a relation.

| Function | Base | Description |
|:--|:--|:--|
| [CaseRelationBuildFunction](api/PayrollEngine.Client.Scripting.Function.CaseRelationBuildFunction.html) | `CaseRelationFunction` | Populates target case fields based on the source case values when a relation is applied. |
| [CaseRelationValidateFunction](api/PayrollEngine.Client.Scripting.Function.CaseRelationValidateFunction.html) | `CaseRelationFunction` | Validates the combined source/target case values within the context of the relation. |

### Payrun Functions

Payrun functions cover the full execution lifecycle of a payrun — from start/end events at the
payrun and employee level through wage type calculation and collector aggregation.

#### Payrun Lifecycle

| Function | Base | Description |
|:--|:--|:--|
| [PayrunStartFunction](api/PayrollEngine.Client.Scripting.Function.PayrunStartFunction.html) | `PayrunFunction` | Executes once at the start of the entire payrun, before any employee is processed. Used for payrun-level initialization. |
| [PayrunEndFunction](api/PayrollEngine.Client.Scripting.Function.PayrunEndFunction.html) | `PayrunFunction` | Executes once at the end of the entire payrun, after all employees have been processed. Used for payrun-level finalization. |
| [PayrunEmployeeAvailableFunction](api/PayrollEngine.Client.Scripting.Function.PayrunEmployeeAvailableFunction.html) | `PayrunFunction` | Determines whether an employee participates in the current payrun. Returning `false` skips the employee entirely. |
| [PayrunEmployeeStartFunction](api/PayrollEngine.Client.Scripting.Function.PayrunEmployeeStartFunction.html) | `PayrunFunction` | Executes at the start of each employee's payrun processing. Used for per-employee initialization. |
| [PayrunEmployeeEndFunction](api/PayrollEngine.Client.Scripting.Function.PayrunEmployeeEndFunction.html) | `PayrunFunction` | Executes at the end of each employee's payrun processing. Used for per-employee finalization or result enrichment. |
| [PayrunWageTypeAvailableFunction](api/PayrollEngine.Client.Scripting.Function.PayrunWageTypeAvailableFunction.html) | `PayrunFunction` | Determines whether a specific wage type is evaluated for an employee in this payrun. Returning `false` skips the wage type calculation. |

#### Wage Type Functions

| Function | Base | Description |
|:--|:--|:--|
| [WageTypeValueFunction](api/PayrollEngine.Client.Scripting.Function.WageTypeValueFunction.html) | `WageTypeFunction` | Calculates the numeric value of a wage type for an employee in the current period. The core calculation script — reads case values, applies rates, performs arithmetic. |
| [WageTypeResultFunction](api/PayrollEngine.Client.Scripting.Function.WageTypeResultFunction.html) | `WageTypeFunction` | Post-processes the calculated wage type result. Used to add custom result attributes, split results, or trigger side effects after the value is determined. |

#### Collector Functions

Collectors aggregate wage type values across a payrun. Their functions run around the collection
process for each contributing wage type.

| Function | Base | Description |
|:--|:--|:--|
| [CollectorStartFunction](api/PayrollEngine.Client.Scripting.Function.CollectorStartFunction.html) | `CollectorFunction` | Executes when the collector is first activated in a payrun. Used to initialize collector state or set starting values. |
| [CollectorApplyFunction](api/PayrollEngine.Client.Scripting.Function.CollectorApplyFunction.html) | `CollectorFunction` | Executes each time a wage type value is applied to the collector. Can modify or veto the value before it is accumulated. |
| [CollectorEndFunction](api/PayrollEngine.Client.Scripting.Function.CollectorEndFunction.html) | `CollectorFunction` | Executes after the last wage type has been applied. Used for final adjustments, caps, or result enrichment on the accumulated value. |

### Report Functions

Report functions control the three-phase execution of a report: structure definition, data retrieval,
and post-processing.

| Function | Base | Description |
|:--|:--|:--|
| [ReportBuildFunction](api/PayrollEngine.Client.Scripting.Function.ReportBuildFunction.html) | `ReportFunction` | Defines the report structure: adds or removes report parameters, adjusts queries, and controls which data sets are included based on input parameters. |
| [ReportStartFunction](api/PayrollEngine.Client.Scripting.Function.ReportStartFunction.html) | `ReportFunction` | Executes at report start after parameters are resolved. Used to prepare or transform data before the main report queries run. |
| [ReportEndFunction](api/PayrollEngine.Client.Scripting.Function.ReportEndFunction.html) | `ReportFunction` | Executes after all report queries have completed. Used to post-process result sets, merge tables, compute derived columns, or apply final formatting. |

---

## No-Code Actions

| Namespace | Description |
|:--|:--|
| [PayrollEngine.Client.Scripting](api/PayrollEngine.Client.Scripting.html) | Action attributes (`CaseAvailableActionAttribute`, `CaseBuildActionAttribute`, …) and the `ActionReflector` for parsing action expressions |
| [PayrollEngine.Action](api/PayrollEngine.Action.html) | Core action infrastructure: `ActionInfo`, `ActionIssue`, `ActionMethodInfo`, parameter and property metadata |

Actions allow payroll specialists to control object behaviour using text expressions in regulation
JSON — no C# programming required. See
[No-Code / Low-Code Development](https://payrollengine.org/roles/regulator/no-code-low-code-development)
for the full reference.

---

## Core Scripting Types

| Namespace | Description |
|:--|:--|
| [PayrollEngine.Client.Scripting](api/PayrollEngine.Client.Scripting.html) | Central scripting types: `CaseObject`, `PayrollValue`, `CasePayrollValue`, `DatePeriod`, function and script attributes |
| [PayrollEngine.Client.Scripting.Report](api/PayrollEngine.Client.Scripting.Report.html) | Report-specific types used within report functions |

---

## Client Infrastructure

| Namespace | Description |
|:--|:--|
| [PayrollEngine.Client](api/PayrollEngine.Client.html) | HTTP client (`PayrollHttpClient`), API endpoint definitions, console base classes, configuration |
| [PayrollEngine.Client.Model](api/PayrollEngine.Client.Model.html) | Payroll domain model: `Tenant`, `Employee`, `Case`, `CaseField`, `WageType`, `Collector`, `Payrun`, … |
| [PayrollEngine.Client.Exchange](api/PayrollEngine.Client.Exchange.html) | Exchange import/export model and visitor pattern for regulation data |
| [PayrollEngine.Client.QueryExpression](api/PayrollEngine.Client.QueryExpression.html) | Fluent query expression builder for REST API filter parameters |
| [PayrollEngine.Client.Command](api/PayrollEngine.Client.Command.html) | CLI command base classes |
| [PayrollEngine.Client.Script](api/PayrollEngine.Client.Script.html) | Script parsers for extracting function code from regulation objects during import/export |

---

## Shared Core

| Namespace | Description |
|:--|:--|
| [PayrollEngine](api/PayrollEngine.html) | Fundamental shared types: enumerations, extensions, calendar utilities, value types |
| [PayrollEngine.Data](api/PayrollEngine.Data.html) | Data handling utilities |
| [PayrollEngine.Document](api/PayrollEngine.Document.html) | Document model types |
| [PayrollEngine.IO](api/PayrollEngine.IO.html) | IO utilities |
| [PayrollEngine.Serialization](api/PayrollEngine.Serialization.html) | JSON serialization helpers |

---

## Links

- [Scripting Documentation](https://payrollengine.org/roles/regulator/no-code-low-code-development)
- [Repository](https://github.com/Payroll-Engine/PayrollEngine.Client.Scripting)
- [NuGet](https://www.nuget.org/packages/PayrollEngine.Client.Scripting)
