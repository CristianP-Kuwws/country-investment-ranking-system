# 🌍 Country Investment Ranking System

## 📌 Overview

This project is an **ASP.NET Core MVC application** built with **.NET
8/9** and **Entity Framework Core (Code First)**.

The system allows analysts to **manually register macroeconomic
indicators for multiple countries** and calculate a **ranking based on a
weighted scoring model and data normalization**.

The goal is to help determine whether it is **feasible to invest in a
country** based on its economic indicators.

---

## 🎯 Main Features

### 🏠 Home / Ranking

- Select a year (latest year selected by default)
- Generate a **country ranking** based on registered indicators

Displays: - Country Name - ISO Code - Scoring (0--1) - Estimated Return
Rate

#### ✔ Validations:

- Total weight of macro indicators must equal **1**
- At least **2 eligible countries** are required
- A country is eligible only if it has all required indicators
  (excluding weight = 0)

---

### 🌎 Country Management

- Create, edit, and delete countries

Fields: - Name (required) - ISO Code (required)

---

### 📊 Macro Indicator Management

Manage macroeconomic indicators.

Fields: - Name - Weight - IsHighBetter (boolean)

#### ✔ Rules:

- Total weight cannot exceed **1**
- If total weight = 1 → no more indicators can be added

---

### 📈 Indicators by Country

Register indicators per country and year.

Fields: - Country (select) - Macro Indicator (select) - Value
(decimal) - Year (integer)

#### ✔ Validations:

- No duplicate indicator per country/year/macro
- All fields are required
- Only **value** can be edited later

#### 🔍 Filtering:

- Filter by country and optionally by year

---

### 💰 Return Rate Configuration

Configure: - Minimum return rate - Maximum return rate

#### ✔ Rules:

- Both fields required
- Min \< Max

Defaults: - Min = 2% - Max = 15%

---

### 🧪 Ranking Simulation

Create custom configurations of macro indicators for simulation.

Features: - Select existing macro indicators - Assign custom weights
(independent from original ones) - Run ranking simulations without
modifying real data

#### ✔ Validations:

- Total simulation weights must equal **1**
- At least **2 eligible countries** required
- Only selected indicators are used in simulation

---

## 🧠 Scoring Algorithm

### 1. Normalization (Min-Max Scaling)

If **higher is better**: (value - min) / (max - min)

If **lower is better**: (max - value) / (max - min)

If min == max: 0.5

---

### 2. Weighted Score

sub_score = normalized_value \* weight

### 3. Final Scoring

score = sum(sub_scores)

- Must be between **0 and 1**

---

### 4. Return Rate Calculation

r = r_min + (r_max - r_min) \* score

---

## 🏗️ Architecture

- Web Layer (MVC)
- Application/Business Logic Layer (Services + DTOs)
- Persistence Layer (EF Core - Code First)

---

## ⚠️ Important Rules

- Total weights must never exceed 1
- A country must have all required indicators
- No duplicate indicators per country/year
- Ranking cannot be generated with less than 2 countries

---

## 🚀 Technologies Used

- ASP.NET Core MVC (.NET 8/9)
- Entity Framework Core
- MySQL
- Bootstrap

---

## ▶️ How to Run

1.  Clone the repository: git clone
    https://github.com/your-username/your-repo.git

2.  Configure your database in appsettings.json

3.  Apply migrations: dotnet ef database update

4.  Run the project: dotnet run

---
