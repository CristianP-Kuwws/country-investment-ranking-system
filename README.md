````md
# Country Investment Analyzer

## Overview

Country Investment Analyzer is an ASP.NET Core MVC web application built with .NET 8/9 and Entity Framework Core (Code First).

The system allows analysts to register macroeconomic indicators for multiple countries and generate investment rankings using weighted scoring models and data normalization techniques.

Its purpose is to help evaluate the potential attractiveness of investing in a country based on economic performance indicators.

---

## Features

### Ranking Dashboard

- Generate country rankings by year
- Latest available year selected by default
- Displays:
  - Country name
  - ISO code
  - Final score (0–1)
  - Estimated return rate

#### Validation Rules

- Total macro indicator weights must equal `1`
- At least `2 eligible countries` are required
- Countries must contain all required indicators (excluding indicators with weight `0`)

---

### Country Management

Create, edit, and manage countries.

#### Fields

- Country name
- ISO code

---

### Macro Indicator Management

Manage macroeconomic indicators used in rankings.

#### Fields

- Name
- Weight
- `IsHighBetter` flag

#### Validation Rules

- Total indicator weights cannot exceed `1`
- If total weight equals `1`, additional indicators cannot be added

---

### Country Indicators

Register macroeconomic indicator values by country and year.

#### Fields

- Country
- Macro indicator
- Value
- Year

#### Validation Rules

- Duplicate indicators per country/year are not allowed
- All fields are required
- Only indicator values can be edited after creation

#### Filtering

- Filter by country
- Optional filtering by year

---

### Return Rate Configuration

Configure estimated investment return ranges.

#### Fields

- Minimum return rate
- Maximum return rate

#### Validation Rules

- Both values are required
- Minimum value must be lower than maximum value

#### Default Values

- Minimum: `2%`
- Maximum: `15%`

---

### Ranking Simulation

Create custom ranking simulations without modifying production data.

#### Features

- Select existing macro indicators
- Assign custom weights
- Run independent ranking simulations

#### Validation Rules

- Simulation weights must total `1`
- At least `2 eligible countries` are required
- Only selected indicators are considered in the simulation

---

## Scoring Algorithm

### 1. Data Normalization (Min-Max Scaling)

If higher values are better:

```text
(value - min) / (max - min)
````

If lower values are better:

```text
(max - value) / (max - min)
```

Fallback case:

```text
If min == max → 0.5
```

---

### 2. Weighted Score

```text
sub_score = normalized_value * weight
```

---

### 3. Final Score

```text
score = sum(sub_scores)
```

Final scores are always between `0` and `1`.

---

### 4. Return Rate Estimation

```text
r = r_min + (r_max - r_min) * score
```

---

## Architecture

The project follows a layered architecture approach:

* Presentation Layer (ASP.NET Core MVC)
* Business Logic Layer (Services + DTOs)
* Persistence Layer (Entity Framework Core - Code First)

---

## Business Rules

* Total indicator weights must never exceed `1`
* Countries must contain all required indicators
* Duplicate indicators per country/year are not allowed
* Rankings cannot be generated with fewer than `2 countries`

---

## Technologies

* C#
* ASP.NET Core MVC (.NET 8/9)
* Entity Framework Core
* MySQL
* Bootstrap

---

## Getting Started

### Prerequisites

* .NET 8/9 SDK
* MySQL Server

### Setup

1. Clone the repository

```bash
git clone https://github.com/your-username/your-repository.git
```

2. Configure `appsettings.json`

3. Apply migrations

```bash
dotnet ef database update
```

4. Run the application

```bash
dotnet run
```

---

## Notes

This project was developed as part of academic training with a focus on business rule implementation, layered architecture, and analytical system design.

```
```
