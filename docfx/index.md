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
[PayrollEngine.Client.Scripting.Function](xref:PayrollEngine.Client.Scripting.Function) namespace
and inherit from a common base hierarchy.

<pre>
<a href="xref:PayrollEngine.Client.Scripting.Function.Function">Function</a>
├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrollFunction">PayrollFunction</a>
│   ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseFunction">CaseFunction</a>
│   │   ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseAvailableFunction">CaseAvailableFunction</a>
│   │   └── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseChangeFunction">CaseChangeFunction</a>
│   │       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseBuildFunction">CaseBuildFunction</a>
│   │       └── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseValidateFunction">CaseValidateFunction</a>
│   ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseRelationFunction">CaseRelationFunction</a>
│   │   ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseRelationBuildFunction">CaseRelationBuildFunction</a>
│   │   └── <a href="xref:PayrollEngine.Client.Scripting.Function.CaseRelationValidateFunction">CaseRelationValidateFunction</a>
│   └── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunFunction">PayrunFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunStartFunction">PayrunStartFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunEndFunction">PayrunEndFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunEmployeeAvailableFunction">PayrunEmployeeAvailableFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunEmployeeStartFunction">PayrunEmployeeStartFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunEmployeeEndFunction">PayrunEmployeeEndFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.PayrunWageTypeAvailableFunction">PayrunWageTypeAvailableFunction</a>
│       ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CollectorFunction">CollectorFunction</a>
│       │   ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CollectorStartFunction">CollectorStartFunction</a>
│       │   ├── <a href="xref:PayrollEngine.Client.Scripting.Function.CollectorApplyFunction">CollectorApplyFunction</a>
│       │   └── <a href="xref:PayrollEngine.Client.Scripting.Function.CollectorEndFunction">CollectorEndFunction</a>
│       └── <a href="xref:PayrollEngine.Client.Scripting.Function.WageTypeFunction">WageTypeFunction</a>
│           ├── <a href="xref:PayrollEngine.Client.Scripting.Function.WageTypeValueFunction">WageTypeValueFunction</a>
│           └── <a href="xref:PayrollEngine.Client.Scripting.Function.WageTypeResultFunction">WageTypeResultFunction</a>
└── <a href="xref:PayrollEngine.Client.Scripting.Function.ReportFunction">ReportFunction</a>
    ├── <a href="xref:PayrollEngine.Client.Scripting.Function.ReportBuildFunction">ReportBuildFunction</a>
    ├── <a href="xref:PayrollEngine.Client.Scripting.Function.ReportStartFunction">ReportStartFunction</a>
    └── <a href="xref:PayrollEngine.Client.Scripting.Function.ReportEndFunction">ReportEndFunction</a>
</pre>

### Case Functions

Case functions control the lifecycle of a case input form — from visibility through field population
to final validation.

| Function | Base | Description |
|:--|:--|:--|
| [CaseAvailableFunction](xref:PayrollEngine.Client.Scripting.Function.CaseAvailableFunction) | `CaseFunction` | Determines whether a case is offered for input. Returning `false` hides the case from the user entirely. Typical use: role-based or condition-based availability. |
| [CaseBuildFunction](xref:PayrollEngine.Client.Scripting.Function.CaseBuildFunction) | `CaseChangeFunction` | Populates or pre-fills case fields before the form is displayed. Used to set default values, apply lookups, or derive field values from existing case data. |
| [CaseValidateFunction](xref:PayrollEngine.Client.Scripting.Function.CaseValidateFunction) | `CaseChangeFunction` | Validates case field values when the user submits the form. Can add validation issues that are shown inline. |

### Case Relation Functions

Case relation functions control how values are transferred and validated when one case references
another via a relation.

| Function | Base | Description |
|:--|:--|:--|
| [CaseRelationBuildFunction](xref:PayrollEngine.Client.Scripting.Function.CaseRelationBuildFunction) | `CaseRelationFunction` | Populates target case fields based on the source case values when a relation is applied. |
| [CaseRelationValidateFunction](xref:PayrollEngine.Client.Scripting.Function.CaseRelationValidateFunction) | `CaseRelationFunction` | Validates the combined source/target case values within the context of the relation. |

### Payrun Functions

Payrun functions cover the full execution lifecycle of a payrun — from start/end events at the
payrun and employee level through wage type calculation and collector aggregation.

#### Payrun Lifecycle

| Function | Base | Description |
|:--|:--|:--|
| [PayrunStartFunction](xref:PayrollEngine.Client.Scripting.Function.PayrunStartFunction) | `PayrunFunction` | Executes once at the start of the entire payrun, before any employee is processed. Used for payrun-level initialization. |
| [PayrunEndFunction](xref:PayrollEngine.Client.Scripting.Function.PayrunEndFunction) | `PayrunFunction` | Executes once at the end of the entire payrun, after all employees have been processed. Used for payrun-level finalization. |
| [PayrunEmployeeAvailableFunction](xref:PayrollEngine.Client.Scripting.Function.PayrunEmployeeAvailableFunction) | `PayrunFunction` | Determines whether an employee participates in the current payrun. Returning `false` skips the employee entirely. |
| [PayrunEmployeeStartFunction](xref:PayrollEngine.Client.Scripting.Function.PayrunEmployeeStartFunction) | `PayrunFunction` | Executes at the start of each employee's payrun processing. Used for per-employee initialization. |
| [PayrunEmployeeEndFunction](xref:PayrollEngine.Client.Scripting.Function.PayrunEmployeeEndFunction) | `PayrunFunction` | Executes at the end of each employee's payrun processing. Used for per-employee finalization or result enrichment. |
| [PayrunWageTypeAvailableFunction](xref:PayrollEngine.Client.Scripting.Function.PayrunWageTypeAvailableFunction) | `PayrunFunction` | Determines whether a specific wage type is evaluated for an employee in this payrun. Returning `false` skips the wage type calculation. |

#### Wage Type Functions

| Function | Base | Description |
|:--|:--|:--|
| [WageTypeValueFunction](xref:PayrollEngine.Client.Scripting.Function.WageTypeValueFunction) | `WageTypeFunction` | Calculates the numeric value of a wage type for an employee in the current period. The core calculation script — reads case values, applies rates, performs arithmetic. |
| [WageTypeResultFunction](xref:PayrollEngine.Client.Scripting.Function.WageTypeResultFunction) | `WageTypeFunction` | Post-processes the calculated wage type result. Used to add custom result attributes, split results, or trigger side effects after the value is determined. |

#### Collector Functions

Collectors aggregate wage type values across a payrun. Their functions run around the collection
process for each contributing wage type.

| Function | Base | Description |
|:--|:--|:--|
| [CollectorStartFunction](xref:PayrollEngine.Client.Scripting.Function.CollectorStartFunction) | `CollectorFunction` | Executes when the collector is first activated in a payrun. Used to initialize collector state or set starting values. |
| [CollectorApplyFunction](xref:PayrollEngine.Client.Scripting.Function.CollectorApplyFunction) | `CollectorFunction` | Executes each time a wage type value is applied to the collector. Can modify or veto the value before it is accumulated. |
| [CollectorEndFunction](xref:PayrollEngine.Client.Scripting.Function.CollectorEndFunction) | `CollectorFunction` | Executes after the last wage type has been applied. Used for final adjustments, caps, or result enrichment on the accumulated value. |

### Report Functions

Report functions control the three-phase execution of a report: structure definition, data retrieval,
and post-processing.

| Function | Base | Description |
|:--|:--|:--|
| [ReportBuildFunction](xref:PayrollEngine.Client.Scripting.Function.ReportBuildFunction) | `ReportFunction` | Defines the report structure: adds or removes report parameters, adjusts queries, and controls which data sets are included based on input parameters. |
| [ReportStartFunction](xref:PayrollEngine.Client.Scripting.Function.ReportStartFunction) | `ReportFunction` | Executes at report start after parameters are resolved. Used to prepare or transform data before the main report queries run. |
| [ReportEndFunction](xref:PayrollEngine.Client.Scripting.Function.ReportEndFunction) | `ReportFunction` | Executes after all report queries have completed. Used to post-process result sets, merge tables, compute derived columns, or apply final formatting. |

---

## No-Code Actions

| Namespace | Description |
|:--|:--|
| [PayrollEngine.Client.Scripting](xref:PayrollEngine.Client.Scripting) | Action attributes (`CaseAvailableActionAttribute`, `CaseBuildActionAttribute`, …) and the `ActionReflector` for parsing action expressions |
| [PayrollEngine.Action](xref:PayrollEngine.Action) | Core action infrastructure: `ActionInfo`, `ActionIssue`, `ActionMethodInfo`, parameter and property metadata |

Actions allow payroll specialists to control object behaviour using text expressions in regulation
JSON — no C# programming required. See
[No-Code / Low-Code Development](https://payrollengine.org/roles/regulator/no-code-low-code-development)
for the full reference.

---

## Core Scripting Types

| Namespace | Description |
|:--|:--|
| [PayrollEngine.Client.Scripting](xref:PayrollEngine.Client.Scripting) | Central scripting types: `CaseObject`, `PayrollValue`, `CasePayrollValue`, `DatePeriod`, function and script attributes |
| [PayrollEngine.Client.Scripting.Report](xref:PayrollEngine.Client.Scripting.Report) | Report-specific types used within report functions |

---

## Client Infrastructure

| Namespace | Description |
|:--|:--|
| [PayrollEngine.Client](xref:PayrollEngine.Client) | HTTP client (`PayrollHttpClient`), API endpoint definitions, console base classes, configuration |
| [PayrollEngine.Client.Model](xref:PayrollEngine.Client.Model) | Payroll domain model: `Tenant`, `Employee`, `Case`, `CaseField`, `WageType`, `Collector`, `Payrun`, … |
| [PayrollEngine.Client.Exchange](xref:PayrollEngine.Client.Exchange) | Exchange import/export model and visitor pattern for regulation data |
| [PayrollEngine.Client.QueryExpression](xref:PayrollEngine.Client.QueryExpression) | Fluent query expression builder for REST API filter parameters |
| [PayrollEngine.Client.Command](xref:PayrollEngine.Client.Command) | CLI command base classes |
| [PayrollEngine.Client.Script](xref:PayrollEngine.Client.Script) | Script parsers for extracting function code from regulation objects during import/export |

---

## Shared Core

| Namespace | Description |
|:--|:--|
| [PayrollEngine](xref:PayrollEngine) | Fundamental shared types: enumerations, extensions, calendar utilities, value types |
| [PayrollEngine.Data](xref:PayrollEngine.Data) | Data handling utilities |
| [PayrollEngine.Document](xref:PayrollEngine.Document) | Document model types |
| [PayrollEngine.IO](xref:PayrollEngine.IO) | IO utilities |
| [PayrollEngine.Serialization](xref:PayrollEngine.Serialization) | JSON serialization helpers |

---

## Links

- [Scripting Documentation](https://payrollengine.org/roles/regulator/no-code-low-code-development)
- [Repository](https://github.com/Payroll-Engine/PayrollEngine.Client.Scripting)
- [NuGet](https://www.nuget.org/packages/PayrollEngine.Client.Scripting)
