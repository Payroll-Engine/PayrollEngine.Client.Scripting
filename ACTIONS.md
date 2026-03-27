# Payroll Engine Client Scripting Actions

<p align="center">
    <img src="https://github.com/Payroll-Engine/PayrollEngine/blob/main/images/ActionSyntax.png" width="640" alt="Action syntax" />
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

For regulations with a namespace, this is added automatically to the name. If an object from another regulation is referenced, the relevant namespace must be provided. In the following example, the regulation has the `CH` namespace.

```yaml
# access to case field CH.Salary
^^Salary
# access to case field from the DE namespace
^^DE.Salary
```

### Action Type
The following types of action exist:

| Type          | Line start    | Description                  |
|:--|:--:|:--|
| Comment       | `#`           | Comment action, not executed |
| Condition     | `?`           | Conditional action           |
| Instruction   | Any other     | Execution action             |


The following example calculates overtime pay for full-time employees only.

```yaml
# Guard: skip for part-time employees
? ^^EmploymentLevel >= 1.0
# Guard: only calculate when overtime hours are entered
? ^^OvertimeHours > 0
# Result: hours x hourly rate (Salary / 160h) x 1.25 surcharge
^^OvertimeHours * (^^Salary / 160) * 1.25
```

> The result of the last action, `^^OvertimeHours * (^^Salary / 160) * 1.25` will be treated as the wage type value.

### Action Conditions
The following conditions can be used to control the action execution:

| Syntax                           | Description                                    | Example                                                    |
|:--|:--|:--|
| `? <cond>`                       | Continue condition for the next action         | `? ^^EmploymentLevel >= 1.0`                               |
| `? <cond> ?= <true>`             | Conditional action result                      | `? PeriodStart.Month != 12 ?= 0`                           |
| `? <cond> ?= <true> ?! <false>`  | Conditional action result with fallback value  | `? ^^TaxClass == 1 ?= 0.15 ?! 0.25`                       |

The following conditions can be included in an action expression:

| Syntax        | Description                                                            | Example                                                         |
|:--:|:--|:--|
| `x && y`      | Logical AND of two boolean values                                      | `? ^^Salary >= 500 && ^^Salary <= 25000`                        |
| `x \|\| y`    | Logical OR of two boolean values                                       | `? ^^EmploymentLevel < 0.1 \|\| ^^EmploymentLevel > 1.0`        |
| `x ? y : z`   | Ternary conditional operator: use `y` when `x` is true, else use `z`  | `^\|TaxRate = ^^GrossIncome > 10000 ? 0.25 : 0.15`              |


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

| Syntax | Target            | Context     | Object                  | Data type | Access | Example                                              |
|:--:|:--:|:--:|:--:|:--:|:--:|:--|
| `^#`   | Lookup value      | Anytime     | All                     | any       | r      | `^#IncomeTax(^&GrossIncome)`                         |
| `^^`   | Case value        | Anytime     | All                     | any       | r      | `^^Salary * ^^EmploymentLevel`                       |
| `^:`   | Case field        | Case change | `Case`                  | any       | r/w    | `^:Salary.Start < PeriodStart`                       |
| `^<`   | Source case field | Case change | `CaseRelation`          | any       | r/w    | `^<EmploymentLevel < 1.0`                            |
| `^>`   | Target case field | Case change | `CaseRelation`          | any       | r/w    | `^>Salary = ^<Salary * ^<EmploymentLevel`            |
| `^\|`  | Runtime value     | Payrun      | `Collector`, `WageType` | any       | r/w    | `^\|GrossPay = ^^Salary * ^^EmploymentLevel`         |
| `^@`   | Payrun result     | Payrun      | `Collector`, `WageType` | any       | r/w    | `^@AnnualBonus = ^^Salary * 12`                      |
| `^$`   | Wage type value   | Payrun      | `WageType`              | `decimal` | r      | `^$BaseSalary * 0.08`                                |
| `^&`   | Collector value   | Payrun      | `WageType`              | `decimal` | r      | `^&GrossIncome * ^#IncomeTax(^&GrossIncome)`         |


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

Concrete examples:

```yaml
# Simple key lookup: holiday allowance rate for employment class A
^#HolidayAllowance('A')
# Range lookup: income tax rate for the current gross income bracket
^#IncomeTax(^&GrossIncome)
# Range lookup with JSON field: employee social security contribution rate
^#SocialSecurity(^^Salary, 'EmployeeRate')
```

#### Case Value Action Reference
The syntax `^^` can be used to access employee or company case data.
```yaml
# Scale monthly salary by employment level
^^Salary * ^^EmploymentLevel
```

If several case values are being calculated against each other as part of an action, any changes to the values occurring within the payroll period will be taken into account (for example, time values). Time values are only supported in `Period` or `CalendarPeriod` numeric fields.

> **Please note** that time values are not supported when calculating a case field value against the values of other payroll objects!

#### Case Field Action Reference
During a case change, you can access the case field using the `^:` syntax. As well as viewing the value, you can control the start and end dates, as well as the time period, of the field.

```yaml
# Validate that the salary start date is not in the past
^:Salary.Start < PeriodStart
```
> The symbol `^<` is used to access the source case and the symbol `^>` is used to access the target case in case relations.

#### Collector Action Reference

Collectors are referenced using the `^&` syntax. An optional scope selector controls the time range of the value:

| Syntax | Scope | Description |
|:-------|:------|:------------|
| `^&Name` | `Period` (default) | Collector value of the current payrun period |
| `^&Name.PrevPeriod` | `PrevPeriod` | Collector value of the previous period |
| `^&Name.NextPeriod` | `NextPeriod` | Collector value of the next period |
| `^&Name.Cycle` | `Cycle` | Year-to-date collector value across all previous payruns in the current cycle |
| `^&Name.PrevCycle` | `PrevCycle` | Total collector value across all payruns of the previous cycle |
| `^&Name.NextCycle` | `NextCycle` | Total collector value across all payruns of the next cycle |

> `PrevPeriod`, `NextPeriod`, `PrevCycle` and `NextCycle` read from persisted payrun results.
> The payrun job must have been executed with `storeEmptyResults: true` for the referenced periods.

```yaml
# Current period gross income
^&GrossIncome

# Previous period gross income
^&GrossIncome.PrevPeriod

# Year-to-date gross income (previous payruns in the current cycle)
^&GrossIncome.Cycle

# Total gross income of the previous cycle (e.g. for holiday pay based on prior year earnings)
^&GrossIncome.PrevCycle

# Income tax: gross income times bracket rate from range lookup
^&GrossIncome * ^#IncomeTax(^&GrossIncome)
```

#### Wage Type Action Reference

Wage types are referenced using the `^$` syntax. Wage types can be identified by name or by number. An optional scope selector controls which value is returned:

| Syntax | Scope | Description |
|:-------|:------|:------------|
| `^$Name` or `^$100` | `Period` (default) | Wage type value of the current payrun period (by name or number) |
| `^$Name.PrevPeriod` | `PrevPeriod` | Wage type value of the previous period |
| `^$Name.NextPeriod` | `NextPeriod` | Wage type value of the next period |
| `^$Name.Cycle` | `Cycle` | Year-to-date wage type value across all previous payruns in the current cycle |
| `^$Name.PrevCycle` | `PrevCycle` | Total wage type value across all payruns of the previous cycle |
| `^$Name.NextCycle` | `NextCycle` | Total wage type value across all payruns of the next cycle |
| `^$Name.RetroSum` | `RetroSum` | Net sum of all pending retro corrections for the wage type within the current cycle |

> `PrevPeriod`, `NextPeriod`, `PrevCycle` and `NextCycle` read from persisted payrun results.
> The payrun job must have been executed with `storeEmptyResults: true` for the referenced periods.

```yaml
# Social security: 8% on current period base salary
^$BaseSalary * 0.08

# Reference by wage type number
^$100 * 0.08

# Year-to-date base salary (previous payruns in the current cycle)
^$BaseSalary.Cycle

# Total base salary of the previous cycle (e.g. for annual bonus based on prior year earnings)
^$BaseSalary.PrevCycle

# Holiday pay: 8% of prior year gross income, paid out in May
? PeriodStart.Month == 5
^&GrossIncome.PrevCycle * 0.08

# Retro delta wage type: net correction for BaseSalary since last closed period
^$BaseSalary.RetroSum
```

### Action Value
An Action supports the following value types:

| Type        | Format                                    |
|:--|:--|
| `String`    | Text with `"double"` or `'single'` quote  |
| `Date`      | [ISO 8601](https://www.iso.org/obp/ui/#iso:std:iso:8601:-1:ed-1:v1:en) UTC (`2026-01-16T16:06:00Z`) |
| `Boolean`   | `true` or `false`                         |
| `Int`       | signed 32-bit integer                     |
| `Decimal`   | 16 bytes floating-point number            |


The following operators exist for value types:

| Syntax   | Description                           | Supported by data type      | Example                                             |
|:--:|:--|:--:|:--|
| `+`      | Addition operator <sup>1)</sup>       | `String`, `Int`, `Decimal`  | `^^Salary + ^^Bonus`                                |
| `-`      | Subtraction operator <sup>1)</sup>    | `Int`, `Decimal`            | `^^Salary - ^&Deductions`                           |
| `*`      | Multiplication operator <sup>2)</sup> | `Int`, `Decimal`            | `^^Salary * ^^EmploymentLevel`                      |
| `/`      | Division operator <sup>2)</sup>       | `Int`, `Decimal`            | `^^Salary / 160`                                    |
| `%`      | Remainder operator <sup>2)</sup>      | `Int`, `Decimal`            | `^^OvertimeHours % 8`                               |
| `&`      | Binary logical AND operator           | `Boolean`                   | `^^IsActive & ^^HasContract`                        |
| `\|`     | Binary logical OR operator            | `Boolean`                   | `^^IsManager \| ^^IsTeamLead`                       |
| `<`      | Less than operator                    | `Date`, `Int`, `Decimal`    | `^^EmploymentLevel < 1.0`                           |
| `<=`     | Less than or equal to operator        | `Date`, `Int`, `Decimal`    | `^^Salary <= 25000`                                 |
| `==`     | Equal operator                        | All                         | `^^TaxClass == 1`                                   |
| `!=`     | Not equal operator                    | All                         | `^^TaxClass != 3`                                   |
| `>=`     | Greater than or equal to operator     | `Date`, `Int`, `Decimal`    | `^^EmploymentLevel >= 1.0`                          |
| `>`      | Greater than operator                 | `Date`, `Int`, `Decimal`    | `^^Salary > 5000`                                   |

<sup>1)</sup> Undefined value: `0`.<br/>
<sup>2)</sup> Undefined value: `1`.<br/>

The following properties can be used to test the action value:

| Name           | Description                              | Result type     | Example                                        |
|:--|:--|:--:|:--|
| `HasValue`     | Test if action has value                 | `Boolean`       | `^^BonusRate.HasValue`                         |
| `IsNull`       | Test if action value is undefined        | `Boolean`       | `^^SpecialAllowance.IsNull ? 0 : ^^SpecialAllowance` |
| `IsString`     | Test for string value                    | `Boolean`       | `^^CostCenter.IsString`                        |
| `IsInt`        | Test for integer value                   | `Boolean`       | `^^TaxClass.IsInt`                             |
| `IsDecimal`    | Test for decimal value                   | `Boolean`       | `^^EmploymentLevel.IsDecimal`                  |
| `IsNumeric`    | Test for numeric value (int or decimal)  | `Boolean`       | `^^ZipCode.IsNumeric`                          |
| `IsDateTime`   | Test for date value                      | `Boolean`       | `^^EntryDate.IsDateTime`                       |
| `IsBool`       | Test for boolean value                   | `Boolean`       | `^^IsActive.IsBool`                            |


The following properties can be used to convert an action value:

| Name           | Description                              | Result type     | Example                                  |
|:--|:--|:--:|:--|
| `AsString`     | Convert action value to string           | `String`        | `^^TaxClass.AsString`                    |
| `AsInt`        | Convert action value to integer          | `Int`           | `^^EmploymentLevel.AsInt`                |
| `AsDecimal`    | Convert action value to decimal          | `Decimal`       | `^^TaxClass.AsDecimal`                   |
| `AsDateTime`   | Convert action value to date             | `Date`          | `^^EntryDate.AsDateTime`                 |
| `AsBool`       | Convert action value to boolean          | `Boolean`       | `^^IsActive.AsBool`                      |


The following mathematical operations are available for numeric values:

| Method                        | Description                     | Result type  | Example                                       |
|:--|:--|:--:|:--|
| `Round(decimals?, rounding?)` | Round decimal value             | `Decimal`    | `^^Salary.Round(2)`                           |
| `RoundUp(step?)`              | Round decimal value up          | `Decimal`    | `^^Salary.RoundUp()`                          |
| `RoundDown(step?)`            | Round decimal value down        | `Decimal`    | `^^Salary.RoundDown()`                        |
| `Truncate(step?)`             | Truncate decimal value          | `Decimal`    | `^^OvertimeHours.Truncate()`                  |
| `Power(factor)`               | Power factor to a decimal value | `Decimal`    | `^^CompoundRate.Power(12)`                    |
| `Abs()`                       | Absolute decimal value          | `Decimal`    | `^^RetroCorrection.Abs()`                     |
| `Sqrt()`                      | Square root of decimal value    | `Decimal`    | `^^VarianceAmount.Sqrt()`                     |


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
| `PeriodName`          | Payrun period name               | `String`    | `PayrunFunction`        |
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
| `WageTypeNumber`      | Wage type number                 | `Decimal`   | `CollectorApplyFunction`, `WageTypeFunction` |
| `WageTypeName`        | Wage type name                   | `String`    | `CollectorApplyFunction`, `WageTypeFunction` |
| `WageTypeValue`       | Wage type value                  | `Decimal`   | `CollectorApplyFunction`, `WageTypeResultFunction` |
| `WageTypeDescription` | Wage type description            | `String`    | `WageTypeFunction`      |
| `WageTypeCalendar`    | Wage type calendar               | `String`    | `WageTypeFunction`      |
| `ExecutionCount`      | Wage type value execution count  | `Int`       | `WageTypeValueFunction` |

> In accordance with the function [inheritance hierarchy](https://github.com/Payroll-Engine/PayrollEngine/wiki/Regulations#functions), all derived functions have access to the properties. For example, all functions except the reporting functions can access the `PayrollFunction` properties.

### Integrated Actions
The Payroll Engine offers various predefined actions, see [Client.Scripting](https://github.com/Payroll-Engine/PayrollEngine/blob/66bf478587956b163cc14674e49e52bb25b01f02/docs/PayrollEngine.Client.Scripting.md).

Here are a few examples:
- `ApplyRangeLookupValue(key, rangeValue, field)` - Apply a range value to the lookup ranges considering the lookup range mode.
- `Concat(str1, ..., strN)` - Concat multiple strings.
- `Contains(test, sel, ..., selN)` - Test if value is from a specific value domain.
- `IIf(expression, onTrue, onFalse)` - Returns one of two parts, depending on the evaluation of an expression.
- `Log(message, level?)` - Log a message.
- `Min(left, right)` - Get the minimum value.
- `Max(left, right)` - Get the maximum value.
- `Range(value, min, max)` - Ensure value is within a value range.

In addition to these, you can create your own predefined actions using low-code; see [Custom Actions](https://github.com/Payroll-Engine/PayrollEngine/wiki/Custom-Actions).
