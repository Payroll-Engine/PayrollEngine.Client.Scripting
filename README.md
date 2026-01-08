# Payroll Engine Client Scripting
👉 This library is part of the [Payroll Engine](https://github.com/Payroll-Engine/PayrollEngine/wiki).

The scripting library that defines the commonality between the backend and the clients:
- Low-Code Scripting functions
- Scripting runtime
- Script parsers
- System scripts
- No-Code Actions

## No-Code Actions
<p align="center">
    <img src="https://github.com/Payroll-Engine/PayrollEngine/blob/main/images/ActionSyntax.png" width="640" alt="Action syntax" />.
</p>

Actions can be used to determine the behaviour of payroll objects, even without any programming knowledge. The following events can be controlled:
- `Case`
    - `Available` - the availability of a case
    - `Build` - Case creation
    - `Validate` - Case validation
- `CaseRelation`
    - `Build` - Creation of the case relationship
    - `Validate` - Validation of the case relationship creation
- `Collector`
    - `Start` - Initialization of the collector
    - `Apply` - Application of wage type result <sup>1)</sup>
    - `End` - Completion of the collector
- `WageType`
    - `Value` - Wage type result <sup>1)</sup>
    - `Result` - Additional wage type result

<sup>1)</sup> Function event with return value<br/>

A list of actions is executed sequentially for each event. For events that expect a return value, the final action calculates the result. The following types of action exist:

| Type           | Line start    | Description                  |
|:--|:--|:--|
| Comment        | `#`           | Comment action, not executed |
| Condition      | `?`           | Conditional action           |
| Instructions   | Any other     | Executiuon action            |

The following example shows how the wage types 'WageTypeValue' are calculated when three entry conditions are met.

```yaml
# Boolean entry status condition
? ^^EntryStatus
# Salary limits condition
? ^^Salary >= 1000 && ^^Salary <= 10000
# Salary tax rate limits condition
? ^^SalaryTasRate >= 0.01 && ^^SalaryTasRate <= 0.03
# Wage type result (last action)
^^Salary * ^^SalaryTasRate
```

> Changes in case value that occur during the pay period are taken into account in the calculation.

### Conditional Actions
The following conditions can be set for actions and calculations:
| Syntax                           | Description                                  | Example                                      |
|:--|:--|:--|
| `? <cond>`                       | Continue condition for the next action       | `? ^^Salary < 1000`                          |
| `? <cond> ?= <true>`             | Continonal action result                     | `? ^^Salary < 1000 ?= 0.5`                   |
| `? <cond> ?= <true> ?! <false>`  | Continonal action result with fallback value | `? ^^Salary < 1000 ?= 0.5 ?! 0.25`           |

The following conditions can be included in an action expression:
| Syntax        | Description                                                            | Example                                            |
|:--|:--|:--|
| `x && y`      | Logical AND of two boolean values                                      | `? ^^Salary > 1000 && ^^Salary < 5000`             |
| `x \|\| y`    | Logical OR of two boolean values                                       | `? ^^Salary < 1000 \|\| ^^Salary > 5000`           |
| `x ?? y`      | Null-coalescing: use `y` when `x` is undefined                         | `? ^^Salary ?? 5000`                               |
| `x ??= y`     | Null-coalescing assigment: assign `y` to `x` when `x` is undefined     | `^\|CalcSalary ??= ^^Salary * ^^TaxSalary`         |
| `x ? y : z`   | Ternary conditional operator: use `y` when `x` is true, else use `z`   | `^\|SalaryFactor = ^^Salary > 10000 ? 0.05 : 0.03` |


### Action References
The following values can be referenced when performing actions:

| Syntax | Target                       | Example                                      | Objects                    | Access       |
|:--|:--|:--|:--|:--|
| `^#`   | Lookup value                 | `^#TaxRate('A')`                             | All                        | Read         |
| `^^`   | Case value                   | `? ^^Salary < 1000`                          | All                        | Read         |
| `^:`   | Case field <sup>1)           | `^:Salary.Start < PeriodStart`               | `Case`                     | Read--write  |
| `^<`   | Source case field <sup>1)    | `^<Salary < 1000`                            | `CaseRelation`             | Read--write  |
| `^>`   | Target case field <sup>1)    | `^>Salary = (5 * 100)`                       | `CaseRelation`             | Read--write  |
| `^\|`  | Runtime value <sup>2)</sup>  | `^\|SalaryWithTax = ^^Salary * ^#TaxRate('A')`| `Collector`, `WageType`   | Read--write  |
| `^@`   | Payrun result <sup>3)</sup>  | `^@SalaryFactor = ^^Salary > 1000 = 1 : 0`   | `Collector`, `WageType`    | Read--write  |
| `^$`   | Wage type value <sup>4)      | `^&MyCollector * 0.2`                        | `WageType`                 | Read         |
| `^&`   | Collector value <sup>4)      | `^&Deductions.Cycle`                         | `WageType`                 | Read         |

<sup>1)</sup> Access to the start date `Start`, the end date `End`, and value `Value` (default) of the dropdown field.<br/>
<sup>2)</sup> Transient value, not saved.<br/>
<sup>3)</sup> Value is saved in the payroll results.<br/>
<sup>4)</sup> Value for the current payroll `Period` (default) and payroll `Cycle` (e.g., year-to-date).<br/>

### Action Value
The following value types can be used in actions:
- `String`
- `Date`
- `Boolean`
- `Int`
- `Decimal`

The following operators exist for value types:
| Syntax   | Description                     | Data types                  | Example                                       |
|:--|:--|:--|:--|
| `+`      | Addition operator               | `String`, `Int`, `Decimal`  | `^^Salary + ^^Bonus`                          |
| `-`      | Subtraction operator            | `Int`, `Decimal`            | `^^Salary - ^&Deductions`                     |
| `*`      | Multiplication operator         | `Int`, `Decimal`            | `^^Salary * ^^TaxRate`                        |
| `/`      | Division operator               | `Int`, `Decimal`            | `^^Salary / ^^InsuranceFactor`                |
| `%`      | Remainder operator              | `Int`, `Decimal`            | `^^Salary % 1000`                             |
| `&`      | Logical AND operator            | `Boolean`                   | `^^Salary < 1000 & ^^InsuranceFactor < 0.1`   |
| `\|`     | Logical OR operator             | `Boolean`                   | `^^Salary > 10000 \| ^^InsuranceFactor > 0.1` |
| `<`      | Less than operator              | `Date`, `Int`, `Decimal`    | `^^Salary < 1000`                             |
| `<=`     | Less than or equal to operator  | `Date`, `Int`, `Decimal`    | `^^Salary <= 1000`                            |
| `==`     | Equal operator                  | All                         | `^^TaxLevel == 3`                             |
| `!=`     | Not equal operator              | All                         | `^^TaxLevel != 1`                             |
| `>=`     | Greater than or equal to operator | `Date`, `Int`, `Decimal`  | `^^Salary >= 3500`                            |
| `>`      | Greater than operator           | `Date`, `Int`, `Decimal`    | `^^Salary > 2800`                             |


The following mathematical operations are available for numeric values:
| Syntax                             | Description                     | Example                                          |
|:--|:--|:--|
| `Round(decimals?, rounding?)`      | Round decimal value             | `^^Salary.Round(2)`                              |
| `RoundUp(step?)`                   | Round decimal value up          | `^^Salary.RoundUp()`                             |
| `RoundDown(step?)`                 | Round decimal value down        | `^^Salary.RoundDown()`                           |
| `Truncate(step?)`                  | Truncate decimal value          | `^^Salary.Truncate()`                            |
| `Power(factor)`                    | Power factor to a decimal value | `^^TaxFactor.Power(2)`                           |
| `Abs()`                            | Absolute decimal value          | `^^Deduction.Abs()`                              |
| `Sqrt()`                           | Square root of decimal value    | `^^Deduction.Sqrt()`                             |


### Runtime Properties
The following function properties can be used in read mode in an action:
| Property              | Description                      | Data type   | Function                |
|:--|:--|:--|:--|
| `UserIdentifier`      | User identifier                  | `String`    | All                     |
| `UserCulture`         | User culture                     | `String`    | All                     |
| `SelfServiceUser`     | Test for self service user       | `Boolean`   | All                     |
| `EmployeeIdentifier`  | Employee identifier              | `String`    | `PayrollFunction`       |
| `Namespace`           | Regulation namespace             | `String`    | `PayrollFunction`       |
| `CycleStart`          | Payroll cycle start date         | `Date`      | `PayrollFunction`       |
| `CycleEnd`            | Payroll cycle end date           | `Date`      | `PayrollFunction`       |
| `CycleDays`           | Payroll cycle day count          | `Decimal`   | `PayrollFunction`       |
| `EvaluationDate`      | Payroll evaluation date          | `Date`      | `PayrollFunction`       |
| `PeriodStart`         | Payroll period start date        | `Date`      | `PayrollFunction`       |
| `PeriodEnd`           | Payroll period end date          | `Date`      | `PayrollFunction`       |
| `PayrunName`          | Payrun name                      | `String`    | `PayrunFunction`        |
| `IsRetroPayrun`       | Test for retro payrun            | `Boolean`   | `PayrunFunction`        |
| `IsCycleRetroPayrun`  | Test for cycle retro payrun      | `Boolean`   | `PayrunFunction`        |
| `Forecast`            | Forecast name                    | `String`    | `PayrunFunction`        |
| `IsForecast`          | Test for forecast payrun         | `Boolean`   | `PayrunFunction`        |
| `PeriodName`          | Payrun perido name               | `String`    | `PayrunFunction`        |
| `CollectorName`       | Collector name                   | `String`    | `CollectorFunction`     |
| `CollectMode`         | Collect mode                     | `String`    | `CollectorFunction`     |
| `Negated`             | Test for negated collector       | `Boolean`   | `CollectorFunction`     |
| `CollectorThreshold`  | Threshold value                  | `Decimal`   | `CollectorFunction`     |
| `CollectorMinResult`  | Minimum allowed collector result | `Decimal`   | `CollectorFunction`     |
| `CollectorMaxResult`  | Maximum allowed collector result | `Decimal`   | `CollectorFunction`     |
| `CollectorResult`     | Collector result value           | `Decimal`   | `CollectorFunction`     |
| `CollectorCount`      | Collected values count           | `Decimal`   | `CollectorFunction`     |
| `CollectorSummary`    | Summary of collected values      | `Decimal`   | `CollectorFunction`     |
| `CollectorMinimum`    | Minimum collected value          | `Decimal`   | `CollectorFunction`     |
| `CollectorMaximum`    | Maximum collected value          | `Decimal`   | `CollectorFunction`     |
| `CollectorAverage`    | Average of collected values      | `Decimal`   | `CollectorFunction`     |
| `WageTypeNumber`      | Wage type number                 | `Decimal`   | `CollectorApplyFunction`|
| `WageTypeName`        | Wage type name                   | `String`    | `CollectorApplyFunction`|
| `WageTypeValue`       | Wage type value                  | `Decimal`   | `CollectorApplyFunction`|
| `WageTypeNumber`      | Wage type number                 | `Decimal`   | `WageTypeFunction`      |
| `WageTypeName`        | Wage type name                   | `String`    | `WageTypeFunction`      |
| `WageTypeDescription` | Wage type value                  | `String`    | `WageTypeFunction`      |
| `WageTypeCalendar`    | Wage type calendard              | `String`    | `WageTypeFunction`      |
| `ExecutionCount`      | Wage type value execution count  | `Int`       | `WageTypeValueFunction` |
| `WageTypeValue`       | Wage type value                  | `Decimal`   | `WageTypeResultFunction`|

### Integrated Actions
The Payroll Engine offers various predefined actions, see [Client.Scripting](https://github.com/Payroll-Engine/PayrollEngine/blob/66bf478587956b163cc14674e49e52bb25b01f02/docs/PayrollEngine.Client.Scripting.md). In addition to these, you can create your own predefined actions using low-code; see [Custom Actions](https://github.com/Payroll-Engine/PayrollEngine/wiki/Custom-Actions).

## HTML documentation
The client scripting library contains static HTML documentation for scripting developers. This is created using [docx](https://github.com/dotnet/docfx) [MIT]. The following commands are available in the `docfx` folder:
- `Static.Build` - builds the static HTML content (output is the `_site` subdirectory)
- `Static.Static` - opens the static help (`_site\index.html`)
- `Server.Start` - start the static help on http://localhost:5865/ (dark mode support)

## Third party components
- Documentation generation with [docfx](https://github.com/dotnet/docfx/) - license `MIT`
- Tests with [xunit](https://github.com/xunit) - license `Apache 2.0`

## Build
Supported runtime environment variables:
- *PayrollEnginePackageDir* - the NuGet package destination directory (optional)
