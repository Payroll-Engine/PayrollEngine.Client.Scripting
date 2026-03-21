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
JSON — no C# programming required.

<p align="center">
    <img src="https://github.com/Payroll-Engine/PayrollEngine/blob/main/images/ActionSyntax.png" width="640" alt="Action syntax" />
</p>

Actions can be used to determine the behaviour of payroll objects, even without any programming
knowledge. The following events can be controlled:

- `Case`
  - `Available` — Availability of a case
  - `Build` — Case creation
  - `Validate` — Case validation
- `CaseRelation`
  - `Build` — Creation of the case relation
  - `Validate` — Validation of the case relation creation
- `Collector`
  - `Start` — Initialization of the collector
  - `Apply` — Application of wage type result <sup>1)</sup>
  - `End` — Completion of the collector
- `WageType`
  - `Value` — Wage type result <sup>1)</sup>
  - `Result` — Additional wage type result

<sup>1)</sup> Function event with return value.

A list of actions is executed sequentially for each event. For events that expect a return value,
the final action calculates the result.

### Expression Fields

Actions are placed in the corresponding expression field of the regulation object:

| Object | Expression field | When executed |
|:-------|:----------------|:--------------|
| `Case` | `availableActions` | Before form displays |
| `Case` | `buildActions` | While form is open |
| `Case` | `validateActions` | On form submit |
| `WageType` | `valueActions` | During payrun calculation |
| `Collector` | `startActions` / `applyActions` / `endActions` | Collector lifecycle |

> **Important:** `valueActions` and `valueExpression` are mutually exclusive.
> `valueActions` is for No-Code expressions. `valueExpression` is for Low-Code C# scripts.
> No-Code tokens (`^^`, `^$`, `^&`, `^#`) are not valid C# and must never appear in `valueExpression`.

### Action Namespace

Actions reference payroll objects by name. To distinguish objects across regulations, define a
regulation namespace — it is added automatically. Cross-regulation references require the explicit
namespace prefix:

```yaml
# access to case field CH.Salary (regulation namespace = CH)
^^Salary
# access to case field from the DE namespace
^^DE.Salary
```

### Action Types

| Type | Line start | Description |
|:-----|:----------:|:------------|
| Comment | `#` | Not executed |
| Condition | `?` | Conditional action — guards remaining actions |
| Instruction | *(any other)* | Execution action — last instruction = return value |

Example: overtime pay for full-time employees only:

```yaml
# Guard: skip for part-time employees
? ^^EmploymentLevel >= 1.0
# Guard: only calculate when overtime hours are entered
? ^^OvertimeHours > 0
# Result: hours x hourly rate (Salary / 160h) x 1.25 surcharge
^^OvertimeHours * (^^Salary / 160) * 1.25
```

### Action Conditions

| Syntax | Description | Example |
|:-------|:------------|:--------|
| `? <cond>` | Continue condition for the next action | `? ^^EmploymentLevel >= 1.0` |
| `? <cond> ?= <true>` | Conditional action result | `? PeriodStart.Month != 12 ?= 0` |
| `? <cond> ?= <true> ?! <false>` | Conditional action result with fallback value | `? ^^TaxClass == 1 ?= 0.15 ?! 0.25` |

Condition expressions:

| Syntax | Description | Example |
|:------:|:------------|:--------|
| `x && y` | Logical AND | `? ^^Salary >= 500 && ^^Salary <= 25000` |
| `x \|\| y` | Logical OR | `? ^^EmploymentLevel < 0.1 \|\| ^^EmploymentLevel > 1.0` |
| `x ? y : z` | Ternary conditional | `^\|TaxRate = ^^GrossIncome > 10000 ? 0.25 : 0.15` |

### Action References

The reference to a payroll object starts with the circumflex character `^`, followed by the object
marker.

| Syntax | Target | Context | Object | Data type | Access | Example |
|:------:|:------:|:-------:|:------:|:---------:|:------:|:--------|
| `^#` | Lookup value | Anytime | All | any | r | `^#IncomeTax(^&GrossIncome)` |
| `^^` | Case value | Anytime | All | any | r | `^^Salary * ^^EmploymentLevel` |
| `^:` | Case field | Case change | `Case` | any | r/w | `^:Salary.Start < PeriodStart` |
| `^<` | Source case field | Case change | `CaseRelation` | any | r/w | `^<EmploymentLevel < 1.0` |
| `^>` | Target case field | Case change | `CaseRelation` | any | r/w | `^>Salary = ^<Salary * ^<EmploymentLevel` |
| `^\|` | Runtime value | Payrun | `Collector`, `WageType` | any | r/w | `^\|GrossPay = ^^Salary * ^^EmploymentLevel` |
| `^@` | Payrun result | Payrun | `Collector`, `WageType` | any | r/w | `^@AnnualBonus = ^^Salary * 12` |
| `^$` | Wage type value | Payrun | `WageType` | `decimal` | r | `^$BaseSalary * 0.08` |
| `^&` | Collector value | Payrun | `WageType` | `decimal` | r | `^&GrossIncome * ^#IncomeTax(^&GrossIncome)` |

#### Lookup Reference

Lookup values are referenced using `^#`. Access is possible by `key`, `rangeValue`, or both.
For JSON object lookup values, use the `field` parameter to access a property:

```yaml
^#LookupName(key)
^#LookupName(key, field)
^#LookupName(rangeValue)
^#LookupName(rangeValue, field)
^#LookupName(key, rangeValue)
^#LookupName(key, rangeValue, field)
```

Concrete examples:

```yaml
# Simple key lookup: holiday allowance rate for employment class A
^#HolidayAllowance('A')
# Range lookup: income tax rate for the current gross income bracket
^#IncomeTax(^&GrossIncome)
# Range lookup with JSON field: employee social security contribution rate
^#SocialSecurity(^^Salary, 'EmployeeRate')
```

#### Case Value Reference

The `^^` syntax accesses employee or company case data. When several time values are multiplied,
overlapping periods within the payroll period are handled automatically:

```yaml
# Scale monthly salary by employment level
^^Salary * ^^EmploymentLevel
```

> Time values are not supported when calculating a case field value against other payroll objects.

#### Collector Reference

Collectors are referenced using the `^&` syntax. An optional scope selector controls the time range
of the value:

| Syntax | Scope | Description |
|:-------|:------|:------------|
| `^&Name` | `Period` (default) | Collector value of the current payrun period |
| `^&Name.Cycle` | `Cycle` | Year-to-date collector value across all previous payruns in the current cycle |

```yaml
# Current period gross income
^&GrossIncome

# Year-to-date gross income (previous payruns in the cycle)
^&GrossIncome.Cycle

# Income tax: gross income times bracket rate from range lookup
^&GrossIncome * ^#IncomeTax(^&GrossIncome)
```

#### Wage Type Reference

Wage types are referenced using the `^$` syntax. Wage types can be identified by name or by number.
An optional scope selector controls which value is returned:

| Syntax | Scope | Description |
|:-------|:------|:------------|
| `^$Name` or `^$100` | `Period` (default) | Wage type value of the current payrun period (by name or number) |
| `^$Name.Cycle` | `Cycle` | Year-to-date wage type value across all previous payruns in the current cycle |
| `^$Name.RetroSum` | `RetroSum` | Net sum of all pending retro corrections for the wage type within the current cycle |

```yaml
# Social security: 8% on current period base salary
^$BaseSalary * 0.08

# Reference by wage type number
^$100 * 0.08

# Year-to-date base salary (previous payruns in the cycle)
^$BaseSalary.Cycle

# Retro delta wage type: net correction for BaseSalary since last closed period
^$BaseSalary.RetroSum
```

### Action Values

Supported value types:

| Type | Format |
|:-----|:-------|
| `String` | Text with `"double"` or `'single'` quotes |
| `Date` | ISO 8601 UTC — e.g. `2026-01-16T16:06:00Z` |
| `Boolean` | `true` or `false` |
| `Int` | Signed 32-bit integer |
| `Decimal` | 16-byte floating-point number |

Operators:

| Syntax | Description | Supported types | Example |
|:------:|:------------|:----------------|:--------|
| `+` | Addition <sup>1)</sup> | `String`, `Int`, `Decimal` | `^^Salary + ^^Bonus` |
| `-` | Subtraction <sup>1)</sup> | `Int`, `Decimal` | `^^Salary - ^&Deductions` |
| `*` | Multiplication <sup>2)</sup> | `Int`, `Decimal` | `^^Salary * ^^EmploymentLevel` |
| `/` | Division <sup>2)</sup> | `Int`, `Decimal` | `^^Salary / 160` |
| `%` | Remainder <sup>2)</sup> | `Int`, `Decimal` | `^^OvertimeHours % 8` |
| `&` | Binary logical AND | `Boolean` | `^^IsActive & ^^HasContract` |
| `\|` | Binary logical OR | `Boolean` | `^^IsManager \| ^^IsTeamLead` |
| `<` | Less than | `Date`, `Int`, `Decimal` | `^^EmploymentLevel < 1.0` |
| `<=` | Less than or equal | `Date`, `Int`, `Decimal` | `^^Salary <= 25000` |
| `==` | Equal | All | `^^TaxClass == 1` |
| `!=` | Not equal | All | `^^TaxClass != 3` |
| `>=` | Greater than or equal | `Date`, `Int`, `Decimal` | `^^EmploymentLevel >= 1.0` |
| `>` | Greater than | `Date`, `Int`, `Decimal` | `^^Salary > 5000` |

<sup>1)</sup> Undefined operand value: `0`. <sup>2)</sup> Undefined operand value: `1`.

Value test properties:

| Property | Description | Result type | Example |
|:---------|:------------|:-----------:|:--------|
| `HasValue` | Test if value is defined | `Boolean` | `^^BonusRate.HasValue` |
| `IsNull` | Test if value is undefined | `Boolean` | `^^SpecialAllowance.IsNull ? 0 : ^^SpecialAllowance` |
| `IsString` | Test for string value | `Boolean` | `^^CostCenter.IsString` |
| `IsInt` | Test for integer value | `Boolean` | `^^TaxClass.IsInt` |
| `IsDecimal` | Test for decimal value | `Boolean` | `^^EmploymentLevel.IsDecimal` |
| `IsNumeric` | Test for numeric value (int or decimal) | `Boolean` | `^^ZipCode.IsNumeric` |
| `IsDateTime` | Test for date value | `Boolean` | `^^EntryDate.IsDateTime` |
| `IsBool` | Test for boolean value | `Boolean` | `^^IsActive.IsBool` |

Value conversion properties:

| Property | Description | Result type | Example |
|:---------|:------------|:-----------:|:--------|
| `AsString` | Convert to string | `String` | `^^TaxClass.AsString` |
| `AsInt` | Convert to integer | `Int` | `^^EmploymentLevel.AsInt` |
| `AsDecimal` | Convert to decimal | `Decimal` | `^^TaxClass.AsDecimal` |
| `AsDateTime` | Convert to date | `Date` | `^^EntryDate.AsDateTime` |
| `AsBool` | Convert to boolean | `Boolean` | `^^IsActive.AsBool` |

Math methods (numeric values):

| Method | Description | Result type | Example |
|:-------|:------------|:-----------:|:--------|
| `Round(decimals?, rounding?)` | Round decimal value | `Decimal` | `^^Salary.Round(2)` |
| `RoundUp(step?)` | Round up | `Decimal` | `^^Salary.RoundUp()` |
| `RoundDown(step?)` | Round down | `Decimal` | `^^Salary.RoundDown()` |
| `Truncate(step?)` | Truncate decimal | `Decimal` | `^^OvertimeHours.Truncate()` |
| `Power(factor)` | Power | `Decimal` | `^^CompoundRate.Power(12)` |
| `Abs()` | Absolute value | `Decimal` | `^^RetroCorrection.Abs()` |
| `Sqrt()` | Square root | `Decimal` | `^^VarianceAmount.Sqrt()` |

### Runtime Properties

The following function properties are available in read mode within an action:

| Property | Description | Data type | Function |
|:---------|:------------|:---------:|:--------:|
| `UserIdentifier` | User identifier | `String` | All |
| `UserCulture` | User culture | `String` | All |
| `SelfServiceUser` | Test for self-service user | `Boolean` | All |
| `EmployeeIdentifier` | Employee identifier | `String` | `PayrollFunction` |
| `Namespace` | Regulation namespace | `String` | `PayrollFunction` |
| `CycleStart` | Payroll cycle start date | `Date` | `PayrollFunction` |
| `CycleEnd` | Payroll cycle end date | `Date` | `PayrollFunction` |
| `CycleDays` | Payroll cycle day count | `Decimal` | `PayrollFunction` |
| `EvaluationDate` | Payroll evaluation date | `Date` | `PayrollFunction` |
| `PeriodStart` | Payroll period start date | `Date` | `PayrollFunction` |
| `PeriodEnd` | Payroll period end date | `Date` | `PayrollFunction` |
| `PayrunName` | Payrun name | `String` | `PayrunFunction` |
| `IsRetroPayrun` | Test for retro payrun | `Boolean` | `PayrunFunction` |
| `IsCycleRetroPayrun` | Test for cycle retro payrun | `Boolean` | `PayrunFunction` |
| `Forecast` | Forecast name | `String` | `PayrunFunction` |
| `IsForecast` | Test for forecast payrun | `Boolean` | `PayrunFunction` |
| `PeriodName` | Payrun period name | `String` | `PayrunFunction` |
| `CollectorName` | Collector name | `String` | `CollectorFunction` |
| `CollectMode` | Collect mode | `String` | `CollectorFunction` |
| `Negated` | Test for negated collector | `Boolean` | `CollectorFunction` |
| `CollectorThreshold` | Threshold value | `Decimal` | `CollectorFunction` |
| `CollectorMinResult` | Minimum allowed result | `Decimal` | `CollectorFunction` |
| `CollectorMaxResult` | Maximum allowed result | `Decimal` | `CollectorFunction` |
| `CollectorResult` | Collector result value | `Decimal` | `CollectorFunction` |
| `CollectorCount` | Collected values count | `Decimal` | `CollectorFunction` |
| `CollectorSummary` | Summary of collected values | `Decimal` | `CollectorFunction` |
| `CollectorMinimum` | Minimum collected value | `Decimal` | `CollectorFunction` |
| `CollectorMaximum` | Maximum collected value | `Decimal` | `CollectorFunction` |
| `CollectorAverage` | Average of collected values | `Decimal` | `CollectorFunction` |
| `WageTypeNumber` | Wage type number | `Decimal` | `CollectorApplyFunction`, `WageTypeFunction` |
| `WageTypeName` | Wage type name | `String` | `CollectorApplyFunction`, `WageTypeFunction` |
| `WageTypeValue` | Wage type value | `Decimal` | `CollectorApplyFunction`, `WageTypeResultFunction` |
| `WageTypeDescription` | Wage type description | `String` | `WageTypeFunction` |
| `WageTypeCalendar` | Wage type calendar | `String` | `WageTypeFunction` |
| `ExecutionCount` | Wage type value execution count | `Int` | `WageTypeValueFunction` |

> All derived functions inherit access to their parent function's properties. For example, all
> payrun functions can access `PayrollFunction` properties.

### Integrated Actions

The Payroll Engine provides a library of predefined actions. Selected examples:

| Action | Description |
|:-------|:------------|
| `ApplyRangeLookupValue(key, rangeValue, field)` | Apply a range value to lookup brackets considering the lookup range mode |
| `Concat(str1, ..., strN)` | Concatenate multiple strings |
| `Contains(test, sel, ..., selN)` | Test if a value belongs to a specific value domain |
| `IIf(expression, onTrue, onFalse)` | Return one of two values depending on an expression |
| `Log(message, level?)` | Log a message |
| `Min(left, right)` | Get the minimum of two values |
| `Max(left, right)` | Get the maximum of two values |
| `Range(value, min, max)` | Clamp a value within a range |

See the [full integrated action reference](https://github.com/Payroll-Engine/PayrollEngine/blob/main/docs/PayrollEngine.Client.Scripting.md)
for all available actions. Custom actions can be added via Low-Code;
see [Custom Actions](https://github.com/Payroll-Engine/PayrollEngine/wiki/Custom-Actions).

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
