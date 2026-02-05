# Payroll Engine Client Scripting
👉 This library is part of the [Payroll Engine](https://github.com/Payroll-Engine/PayrollEngine/wiki).

The scripting library that defines the commonality between the backend and the clients:
- Low-Code Scripting functions
- Scripting runtime
- Script parsers
- No-Code Actions

## No-Code Actions
<p align="center">
    <img src="https://github.com/Payroll-Engine/PayrollEngine/blob/main/images/ActionSyntax.png" width="640" alt="Action syntax" />.
</p>

Actions can be used to determine the behaviour of payroll objects, even without any programming knowledge. The following events can be controlled:
- `Case`
    - `Available` - Availability of a case
    - `Build` - Case creation
    - `Validate` - Case validation
- `CaseRelation`
    - `Build` - Creation of the case relation
    - `Validate` - Validation of the case relation creation
- `Collector`
    - `Start` - Initialization of the collector
    - `Apply` - Application of wage type result <sup>1)</sup>
    - `End` - Completion of the collector
- `WageType`
    - `Value` - Wage type result <sup>1)</sup>
    - `Result` - Additional wage type result

<sup>1)</sup> Function event with return value<br/>

A list of actions is executed sequentially for each event. For events that expect a return value, the final action calculates the result.

### Action Namespace
The relevant action uses the name of the object in order to access it. This could be the name of a case field or wage type, for example.
To distinguish between objects in different regulations, it is recommended that you define the regulation namespace.

For regulations with a namespace, this is added automatically to the name. If an object from another regulation is referenced, the relevant namespace must be provided. In the following example, the regulation has the `US` namespace.

```yaml
# access to case field US.Salary
^^Salary
# access to case field CA.Salary
^^CA.Salary
```

### Action Type
The following types of action exist:

| Type          | Line start    | Description                  |
|:--|:--:|:--|
| Comment       | `#`           | Comment, not executed        |
| Condition     | `?`           | Conditional action           |
| Instruction   | Any other     | Execution action             |


The following example illustrates how a wage type value is calculated under specific conditions.

```yaml
# Boolean entry status condition
? ^^EntryStatus
# Salary limits condition
? ^^Salary >= 1000 && ^^Salary <= 10000
# Salary tax rate limits condition
? ^^SalaryTaxRate >= 0.01 && ^^SalaryTaxRate <= 0.03
# Wage type result (last action)
^^Salary * ^^SalaryTaxRate
```

> The result of the last action, `^^Salary * ^^SalaryTaxRate` will be treated as wage type value.

### Action Conditions
The following conditions can be used to control the action execution:

| Syntax           | Description                                    | Example                                      |
|:--|:--|:--|
| `? x`            | Continue if condition `x` is true         | `? ^^Salary < 1000`                          |
| `? x ?= y`       | Conditinonal action result<br />*use `y` when `x` is true*  | `? ^^Salary < 1000 ?= 0.5`                   |
| `? x ?= y ?! z`  | Conditinonal action result with fallback value<br />*use `y` when `x` is true, else use `z`* | `? ^^Salary < 1000 ?= 0.5 ?! 0.25`           |

The following conditions can be included in an action expression:

| Syntax        | Description                                                                | Example                                            |
|:--:|:--|:--|
| `x && y`      | Logical AND of boolean values `x` and `y`                                  | `? ^^Salary > 1000 && ^^Salary < 5000`             |
| `x \|\| y`    | Logical OR of two boolean values `x` and `y`                               | `? ^^Salary < 1000 \|\| ^^Salary > 5000`           |
| `x ? y : z`   | Ternary conditional operator<br />*use `y` when `x` is true, else use `z`* | `^\|SalaryFactor = ^^Salary > 10000 ? 0.05 : 0.03` |


### Action Reference
The following payroll objects can be accessed when performing controls and calculations on actions:
- *Lookup value* - simple or JSON property.
- *Case value* - employee, company, national or global.
- *Case field* - value with start and end dates.
- *Runtime value* - a transient value per employee.
- *Payrun result* - stored as payroll result.
- *Wage type value* - value for the current and retro periods.
- *Collector value* - Value for the current and retro periods.

The reference to the payroll object starts with the circumflex character `^`, followed by the object marker.

| Syntax | Target            | Context     | Object                  | Data type | Access | Source   |
|:--:|:--:|:--:|:--:|:--:|:--:|:--:|
| `^#`   | Lookup value      | Anytime     | All                     | any       | r      | Database |
| `^^`   | Case value        | Anytime     | All                     | any       | r      | Memory   |
| `^:`   | Case field        | Case change | `Case`                  | any       | r/w    | Memory   |
| `^<`   | Source case field | Case change | `CaseRelation`          | any       | r/w    | Memory   |
| `^>`   | Target case field | Case change | `CaseRelation`          | any       | r/w    | Memory   |
| `^\|`  | Runtime value     | Payrun      | `Collector`, `WageType` | any       | r/w    | Memory   |
| `^@`   | Payrun result     | Payrun      | `Collector`, `WageType` | any       | r/w    | Memory   |
| `^$`   | Wage type value   | Payrun      | `WageType`              | `decimal` | r      | Period: Memory<br /> Cycle: Database |
| `^&`   | Collector value   | Payrun      | `WageType`              | `decimal` | r      | Period: Memory<br /> Cycle: Database |


#### Lookup Action Reference
Lookup values are referenced using the `^#` syntax. The value access is possible through the lookup value `key`, and/or by `rangeValue`. In cases the lookup value is a JSON object, use the `field` parameter to access the JSON object property.

```yaml
^#LookupName(key)
^#LookupName(key, field)
^#LookupName(rangeValue)
^#LookupName(rangeValue, field)
^#LookupName(key, rangeValue)
^#LookupName(key, rangeValue, field)
```

The following example shows how income is calculated using the tax rate obtained from the `TaxRate` lookup table. The income-dependent tax rate is determined by the `^^Salary` case value.
```yaml
^^Salary * ^#TaxRate(^^Salary)
```

#### Case Value Action Reference
The syntax `^^` can be used to access employee or company case data.
```yaml
^^Salary * ^^Factor
```

If several case values are being calculated against each other as part of an action, any changes to the values occurring within the payroll period will be taken into account (time values). Time values are only supported in numeric case fields with time type `Period` or `CalendarPeriod`.

> **Please note** that time values are not supported when calculating a case field value against the values of other payroll objects!

#### Case Field Action Reference
During a case change, you can access the case field using the `^:` syntax. The symbol `^<` is used to access the source case and the symbol `^>` is used to access the target case in case relations.

There is access to the following case field data:
- `Value` - case field value aof any type (default)
- `Start` - case field start date
- `End` - case field end date
- `Duration` - duration between case field start end end date

Case field example actions:
```yaml
# access to cae field value
^:Salary < ^|MinSalary
# access to case field start
^:Salary.Start < PeriodStart
# access to case field duration
? ^:Salary.Duration.Days < 30
```

#### Collector and Wage Type Action Reference
The syntax used to reference collectors is `^&`, while the syntax used to reference wage types is `^$`. The scope of the value can be specified using the time selector:
- `Period`: collector or wage type value of the current payrun (default).
- `Cycle`: collector or wage type value of the previous calendar cycle payruns.

The following example shows how to calculate the year-to-date value of the `Deductions` wage type, which has a higher processing order:
```yaml
# consolidated deduction from previous payruns + deduction from current payrun
^$Deductions.Cycle + ^$Deductions
```

### Action Value
An Action supports the following value types:

| Type        | Format                                                                        | Format                 | Example                      |
|:--|:--|:--|:--|
| `String`    | Text with `"double"` or `'single'` quote                                      |                        | `"MyString"` or `'MyString'` |
| `Date`      | [ISO 8601](https://www.iso.org/obp/ui/#iso:std:iso:8601:-1:ed-1:v1:en) in UTC | `yyyy-MM-ddTHH:mm:ssZ` | `2026-01-16T16:06:00Z`       |
| `TimeSpan`  | [Time interval](https://learn.microsoft.com/en-us/dotnet/api/system.timespan) | `[-][d.]h:mm:ss`       | `0.12:00:00`                 |
| `Boolean`   | `true` or `false`                                                             |                        | `^^BoolField == true`        |
| `Int`       | Signed 32-bit integer                                                         |                        | `500`                        |
| `Decimal`   | 16 bytes floating-point number                                                |                        | `1.5`                        |

The following operators exist for action values:

| Syntax   | Description                           | Supported data type                    | Example                                       |
|:--:|:--|:--:|:--|
| `+`      | Addition operator <sup>1) 2)</sup>    | `Int`, `Decimal`, `TimeSpan`, `String` | `^^Salary + ^^Bonus`                          |
| `-`      | Subtraction operator <sup>1) 2)</sup> | `Int`, `Decimal`, `TimeSpan`           | `^^Salary - ^&Deductions`                     |
| `*`      | Multiplication operator <sup>3)</sup> | `Int`, `Decimal`                       | `^^Salary * ^^TaxRate`                        |
| `/`      | Division operator <sup>3)</sup>       | `Int`, `Decimal`                       | `^^Salary / ^^InsuranceFactor`                |
| `%`      | Remainder operator <sup>3)</sup>      | `Int`, `Decimal`                       | `^^Salary % 1000`                             |
| `&`      | Binary logical AND operator           | `Boolean`                              | `^^Salary < 1000 & ^^InsuranceFactor < 0.1`   |
| `\|`     | Binary logical OR operator            | `Boolean`                              | `^^Salary > 10000 \| ^^InsuranceFactor > 0.1` |
| `<`      | Less than operator                    | `Int`, `Decimal`, `Date`, `TimeSpan`   | `^^Salary < 1000`                             |
| `<=`     | Less than or equal to operator        | `Int`, `Decimal`, `Date`, `TimeSpan`   | `^^Salary <= 1000`                            |
| `==`     | Equal operator                        | All                                    | `^^TaxLevel == 3`                             |
| `!=`     | Not equal operator                    | All                                    | `^^TaxLevel != 1`                             |
| `>=`     | Greater than or equal to operator     | `Int`, `Decimal`, `Date`, `TimeSpan`   | `^^Salary >= 3500`                            |
| `>`      | Greater than operator                 | `Int`, `Decimal`, `Date`, `TimeSpan`   | `^^Salary > 2800`                             |

<sup>1)</sup> Adding or subtracting a `TimeSpan` from a `Date` results in a `Date` value.<br />
<sup>2)</sup> Undefined value: `0`.<br/>
<sup>3)</sup> Undefined value: `1`.<br/>

Action value operator examples:
```yaml
# multiple decimal values
^^Salary * ^^TaxFactor
# add time range to date
^|StartDate = :Salary.Start < PeriodStart
```

Properties to test action values:

| Name           | Description                              | Result type     | Example                                  |
|:--|:--|:--:|:--|
| `HasValue`     | Test if action has value                 | `Boolean`       | `^^TaxLevel.HasValue`                    |
| `IsNull`       | Test is action value is undefined        | `Boolean`       | `^^TaxLevel.IsNull ? 1 : 0.5`            |
| `IsString`     | Test for string value                    | `Boolean`       | `^^ZipCode.IsString`                     |
| `IsInt`        | Test for integer value                   | `Boolean`       | `^^TaxLevel.IsInt`                       |
| `IsDecimal`    | Test for decimal value                   | `Boolean`       | `^^TaxLevel.IsDecimal`                   |
| `IsNumeric`    | Test for numeric value (int or decimal)  | `Boolean`       | `^ZipCode.IsNumeric`                     |
| `IsDateTime`   | Test for date value                      | `Boolean`       | `^^TaxLevel.IsDateTime`                  |
| `IsTimeSpan`   | Test for timespan value                  | `Boolean`       | `(^^End - ^^Start).IsTimeSpan`           |
| `IsBool`       | Test for boolean value                   | `Boolean`       | `^^TaxLevel.IsBool`                      |


Properties to convert action values:

| Name           | Description                              | Result type     | Example                                  |
|:--|:--|:--:|:--|
| `AsString`     | Convert action value to string           | `String`        | `^^Salary.AsString`                      |
| `AsInt`        | Convert action value to integer          | `Int`           | `^^Salary.AsInt`                         |
| `AsDecimal`    | Convert action value to decimal          | `Decimal`       | `^^TaxLevel.AsDecimal`                   |
| `AsDateTime`   | Convert action value to date             | `Date`          | `^^EntryDate.AsDateTime`                 |
| `AsTimeSpan`   | Convert action value to timespan         | `TimeSpan`      | `^^Durationn.AsTimeSpan`                 |
| `AsBool`       | Convert action value to boolean          | `Boolean`       | `^^WeekendWork.AsBool`                   |


Mathematical operations for numeric action values:

| Method                        | Description                     | Result type  | Example                               |
|:--|:--|:--:|:--|
| `Round(decimals?, rounding?)` | Round decimal value             | `Decimal`    | `^^Salary.Round(2)`                   |
| `RoundUp(step?)`              | Round decimal value up          | `Decimal`    | `^^Salary.RoundUp()`                  |
| `RoundDown(step?)`            | Round decimal value down        | `Decimal`    | `^^Salary.RoundDown()`                |
| `Truncate(step?)`             | Truncate decimal value          | `Decimal`    | `^^Salary.Truncate()`                 |
| `Power(factor)`               | Power factor to a decimal value | `Decimal`    | `^^TaxFactor.Power(2)`                |
| `Abs()`                       | Absolute decimal value          | `Decimal`    | `^^Deduction.Abs()`                   |
| `Sqrt()`                      | Square root of decimal value    | `Decimal`    | `^^Deduction.Sqrt()`                  |


Properties and methods for date action values:

| Property/Method                  | Description                  | Source type | Result type | Example                               |
|:--|:--|:--:|:--|
| `Year`                           | Get the date year            | `Date`      | `Int`       | `^^EntryDate.Year`                    |
| `Month`                          | Get the date month           | `Date`      | `Int`       | `^^EntryDate.Month`                   |
| `Day`                            | Get the date day             | `Date`      | `Int`       | `^^EntryDate.Day`                     |
| `AddYears(count)` <sup>1)</sup>  | Add years the date           | `Date`      | `Date`      | `^^EntryDate.AddYears(1)`             |
| `AddMonths(count)` <sup>1)</sup> | Add months the date          | `Date`      | `Date`      | `^^PeriodEnd.AddMonths(-1)`           |
| `AddDays(count)` <sup>1)</sup>   | Add days the date            | `Date`      | `Date`      | `^^EntryDate.AddDays(90)`             |
| `Add(timeSpan)`                  | Add timespan to date         | `Date`      | `Date`      | `^^EntryDate.Add(PeriodDuration)`     |
| `Subtract(timeSpan)`             | Subtract timespan from date  | `Date`      | `Date`      | `^^CycleEnd.Subtract(PeriodDuration)` |
| `Days`                           | Get day count from timespan  | `TimeSpan`  | `Int`       | `^:Salary.Duration.Days`              |

<sup>1)</sup> Substraction with negative values.<br />

### Runtime Properties
The following function properties can be used in read mode in an action:

| Property              | Description                      | Data type   | Function                |
|:--|:--|:--:|:--:|
| `UserIdentifier`      | User identifier                  | `String`    | All                     |
| `UserCulture`         | User culture                     | `String`    | All                     |
| `SelfServiceUser`     | Test for self service user       | `Boolean`   | All                     |
| `EmployeeIdentifier`  | Employee identifier              | `String`    | `PayrollFunction`       |
| `Namespace`           | Regulation namespace             | `String`    | `PayrollFunction`       |
| `CycleStart`          | Payroll cycle start date         | `Date`      | `PayrollFunction`       |
| `CycleStartYear`      | Payroll cycle start year         | `Int`       | `PayrollFunction`       |
| `CycleStartMonth`     | Payroll cycle start month        | `Int`       | `PayrollFunction`       |
| `CycleStartDay`       | Payroll cycle start day          | `Int`       | `PayrollFunction`       |
| `CycleEnd`            | Payroll cycle end date           | `Date`      | `PayrollFunction`       |
| `CycleEndYear`        | Payroll cycle end year           | `Int`       | `PayrollFunction`       |
| `CycleEndMonth`       | Payroll cycle end month          | `Int`       | `PayrollFunction`       |
| `CycleEndDay`         | Payroll cycle end day            | `Int`       | `PayrollFunction`       |
| `CycleDuration`       | Payroll cycle duration           | `TimeSpan`  | `PayrollFunction`       |
| `CycleDays`           | Payroll cycle day count          | `Int`       | `PayrollFunction`       |
| `EvaluationDate`      | Payroll evaluation date          | `Date`      | `PayrollFunction`       |
| `PeriodName`          | Payrun period name               | `String`    | `PayrunFunction`        |
| `PeriodStart`         | Payroll period start date        | `Date`      | `PayrollFunction`       |
| `PeriodStartYear`     | Payroll period start year        | `Int`       | `PayrollFunction`       |
| `PeriodStartMonth`    | Payroll period start month       | `Int`       | `PayrollFunction`       |
| `PeriodStartDay`      | Payroll period start day         | `Int`       | `PayrollFunction`       |
| `PeriodEnd`           | Payroll period end date          | `Date`      | `PayrollFunction`       |
| `PeriodEndYear`       | Payroll period end year          | `Int`       | `PayrollFunction`       |
| `PeriodEndMonth`      | Payroll period end month         | `Int`       | `PayrollFunction`       |
| `PeriodEndDay`        | Payroll period end day           | `Int`       | `PayrollFunction`       |
| `PeriodDuration`      | Payroll period duration          | `TimeSpan`  | `PayrollFunction`       |
| `PeriodDays`          | Payroll period day count         | `Int`       | `PayrollFunction`       |
| `PayrunName`          | Payrun name                      | `String`    | `PayrunFunction`        |
| `IsRetroPayrun`       | Test for retro payrun            | `Boolean`   | `PayrunFunction`        |
| `IsCycleRetroPayrun`  | Test for cycle retro payrun      | `Boolean`   | `PayrunFunction`        |
| `Forecast`            | Forecast name                    | `String`    | `PayrunFunction`        |
| `IsForecast`          | Test for forecast payrun         | `Boolean`   | `PayrunFunction`        |
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
| `WageTypeCalendar`    | Wage type calendar               | `String`    | `WageTypeFunction`      |
| `ExecutionCount`      | Wage type value execution count  | `Int`       | `WageTypeValueFunction` |
| `WageTypeValue`       | Wage type value                  | `Decimal`   | `WageTypeResultFunction`|

> In accordance with the function [inheritance hierarchy](https://github.com/Payroll-Engine/PayrollEngine/wiki/Regulations#functions), all derived functions have access to the properties. For example, all functions except the reporting functions can access the `PayrollFunction` properties.

### Integrated Actions
The Payroll Engine offers a variety of predefined actions. Here are a few examples:
- `IIf(expression, onTrue, onFalse)` - Returns one of two parts, depending on the evaluation of an expression.
- `Concat(str1, ..., strN)` - Concat multiple strings.
- `Contains(test, sel, ..., selN)` - Test if value is from a specific value domain.
- `Min(left, rigtht)` - Get the minimum value.
- `Max(left, rigtht)` - Get the maximum value.
- `Within(value, min, max)` - Test value is within a value range.
- `Range(value, min, max)` - Ensure value is within a value range.
- `ApplyRangeLookupValue(key, rangeValue, field?)` - Apply a range value to the lookup ranges considering the lookup range mode.
- `GetTimeSpan(start, end)` - Get the time range between two days.
- `SameYear(left, rigtht)` - Test for same date year.
- `SameMonth(left, rigtht)` - Test for same date year/month.
- `SameDay(left, rigtht)` - Test for same date year/month/day.
- `YearDiff(start, end)` - Get year count between two dates.
- `Age(birthDate, testDate?)` - Get persons age.
- `Log(message, level?)` - Log a message.

The full list of predefined actions is listed on [Client.Scripting](https://github.com/Payroll-Engine/PayrollEngine/blob/66bf478587956b163cc14674e49e52bb25b01f02/docs/PayrollEngine.Client.Scripting.md) page.

In addition to these, you can create your own predefined actions using low-code; see [Custom Actions](https://github.com/Payroll-Engine/PayrollEngine/wiki/Custom-Actions).

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
