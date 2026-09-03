# 🧪 InsightLab

### A/B Testing & Statistical Analysis Dashboard

> **Turning experiment data into clear, statistically supported business decisions.**

InsightLab is a modern analytics dashboard that simulates real-world **A/B testing experiments** such as checkout redesigns, email campaigns, and pricing page tests.

It answers one of the most important questions for product and data teams:

> **Did Variant B actually perform better than Variant A — or did we just get lucky? 🤔**

The application combines **data analysis, descriptive statistics, hypothesis testing, SQL databases, and interactive visualisation** inside a full-stack ASP.NET Core MVC application.


---

## 🚀 Project Overview

```text
┌─────────────────────┐
│   Raw Experiment    │
│        Data         │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│    SQL Server       │
│      Database       │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Descriptive Analysis│
│                     │
│ • Conversion Rate   │
│ • Mean              │
│ • Median            │
│ • Standard Deviation│
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│    A/B Testing      │
│                     │
│ Two-Proportion      │
│      Z-Test         │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Statistical Result  │
│                     │
│ Significant?        │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Business Decision 💡│
└─────────────────────┘
```

The goal is not just to display numbers.

**InsightLab analyses the data, tests whether the results are statistically significant, and translates the findings into a clear business recommendation.**

---

# ✨ Features

## 📊 Interactive Dashboard

The main dashboard provides a high-level overview of experiment performance.

### KPI Cards

* 🧪 Total Experiments
* 👥 Total Participants
* 🎯 Overall Conversion Rate
* 🏆 Best-Performing Variant

### Visualisations

* 📊 Conversion Rate Comparison
* 🕸️ Experiment Performance Overview
* 📈 Interactive Chart.js visualisations
* 📋 Recent Experiment Results
* 🏷️ Statistical significance badges

The dashboard is designed to feel like a modern analytics platform rather than a traditional student CRUD application.

---

## 🧪 Experiments

Each experiment is displayed as a visual card containing:

* Experiment name
* Description
* Start and end dates
* Number of participants
* Variant A conversion rate
* Variant B conversion rate
* Performance difference
* Winning variant
* Statistical significance

Users can select:

> **View Analysis →**

to explore the complete statistical breakdown.

---

## 🔬 Experiment Analysis

The experiment analysis page provides a detailed comparison between **Variant A** and **Variant B**.

### Each variant includes:

* 👥 Number of participants
* 🎯 Number of conversions
* 📈 Conversion rate
* 💰 Average order value
* 📊 Mean
* 📍 Median
* 📉 Minimum and maximum
* 📐 Standard deviation

---

### 🧮 Statistical Test Results

InsightLab performs a **two-proportion Z-test** to determine whether the difference between conversion rates is statistically significant.

The analysis includes:

* Conversion rate difference
* Relative improvement
* Z-score
* P-value
* Significance level
* Confidence level
* Statistical significance result

Example:

```text
CHECKOUT REDESIGN EXPERIMENT
────────────────────────────────

                 VARIANT A     VARIANT B

Participants        2,500         2,500
Conversions           205           268
Conversion Rate      8.2%         10.72%

────────────────────────────────

Difference:        +2.52%
Relative Change:   +30.7%
P-Value:            0.03
Confidence Level:   95%

RESULT

✅ STATISTICALLY SIGNIFICANT

Variant B performed significantly better than
Variant A.

Recommendation:
Consider implementing Variant B.
```

---

## 📈 Interactive Charts

InsightLab uses **Chart.js** to make the statistical results easier to understand.

The application includes:

### 📊 Conversion Rate Comparison

Compare Variant A and Variant B side-by-side.

### 📈 Daily Conversions

Track conversion activity throughout the experiment.

### 💰 Average Order Value

Compare the spending behaviour of users in each variant.

### 🕸️ Performance Overview

Compare experiment performance across multiple metrics.

---

# 📂 Data Explorer

The **Data Explorer** allows users to explore the underlying experiment data.

Each participant record includes:

| Field         | Description                       |
| ------------- | --------------------------------- |
| `Session ID`  | Unique user session               |
| `Experiment`  | Experiment being analysed         |
| `Variant`     | Variant A or Variant B            |
| `Date`        | Session date                      |
| `Converted`   | Whether the participant converted |
| `Order Value` | Purchase value                    |

### Available Features

* 🔎 Search participant records
* 🧪 Filter by experiment
* 🅰️ Filter by Variant A or B
* 🎯 Filter by conversion status
* 📊 Explore the raw data behind the dashboard

This demonstrates an important analytics principle:

> **Good dashboards should allow users to move from high-level insights to the underlying data.**

---

# 🧠 Statistics in Plain English

InsightLab combines **descriptive statistics** and **inferential statistics**.

---

## 📊 Descriptive Statistics

Descriptive statistics describe the data collected during an experiment.

The application calculates:

### 🎯 Conversion Rate

The percentage of participants who completed the desired action.

```text
Conversions ÷ Total Participants
```

---

### 💰 Mean

The average order value.

---

### 📍 Median

The middle order value when all values are arranged from smallest to largest.

---

### 📉 Minimum and Maximum

The smallest and largest order values.

---

### 📐 Standard Deviation

Shows how spread out the order values are from the average.

A higher standard deviation generally indicates more variation in the data.

---

# 🧪 A/B Testing

The main statistical feature of InsightLab is a **two-proportion Z-test**.

The purpose is to compare the conversion rates of Variant A and Variant B.

The test answers:

> **Is the observed difference between the two variants likely to be real, or could it have happened by random chance?**

The statistical calculations are handled by:

```text
Services/StatisticsService.cs
```

The calculations are commented to make the logic easier to understand and explain during interviews.

---

## 🔬 How the Two-Proportion Z-Test Works

### Step 1 — Calculate Conversion Rates

Calculate the conversion rate for both variants.

```text
Variant A Conversion Rate

Variant B Conversion Rate
```

---

### Step 2 — Assume the Null Hypothesis

The null hypothesis assumes:

> **There is no real difference between Variant A and Variant B.**

Any observed difference is assumed to be caused by random variation.

---

### Step 3 — Calculate the Pooled Conversion Rate

The two groups are combined to estimate the expected conversion rate under the assumption that there is no real difference.

---

### Step 4 — Calculate the Standard Error

The standard error measures the expected amount of random variation between the two conversion rates.

---

### Step 5 — Calculate the Z-Score

The Z-score measures how far the observed difference is from the expected difference under the null hypothesis.

```text
Z-Score = Observed Difference ÷ Standard Error
```

---

### Step 6 — Calculate the P-Value

The Z-score is converted into a **two-tailed P-value**.

The P-value helps determine whether the result is statistically significant.

---

### Step 7 — Make a Decision

InsightLab uses:

```text
Significance Level: 0.05

Confidence Level: 95%
```

Decision rule:

```text
P-Value < 0.05
```

Result:

```text
✅ Statistically Significant
```

Otherwise:

```text
⚠️ Not Statistically Significant
```

---

# 🗄️ Database Design

InsightLab uses **SQL Server LocalDB** with **Entity Framework Core Code-First**.

The database contains two main tables.

```text
Experiments
│
│ 1
│
│
│ N
▼

ExperimentParticipants
```

---

## 🧪 Experiments

Stores information about each A/B test.

| Column           | Description                      |
| ---------------- | -------------------------------- |
| `ExperimentId`   | Primary Key                      |
| `ExperimentName` | Name of the experiment           |
| `Description`    | Description of the business test |
| `StartDate`      | Experiment start date            |
| `EndDate`        | Experiment end date              |

---

## 👥 ExperimentParticipants

Stores individual participant records.

| Column          | Description                       |
| --------------- | --------------------------------- |
| `ParticipantId` | Primary Key                       |
| `ExperimentId`  | Foreign Key                       |
| `SessionId`     | Unique session identifier         |
| `Variant`       | Variant A or B                    |
| `SessionDate`   | Date of the session               |
| `Converted`     | Whether the participant converted |
| `OrderValue`    | Value of the purchase             |

### Relationship

```text
One Experiment
      │
      │
      └──────< Many Participants
```

---

# 🌱 Seed Data

InsightLab automatically generates reproducible sample data using a fixed random seed.

```text
Random Seed = 42
```

The project includes three experiments.

---

## 🛒 Checkout Redesign

**Business Question:**

> Does a simplified checkout process increase completed purchases?

| Variant   | Conversion Rate |
| --------- | --------------: |
| Variant A |            8.2% |
| Variant B |          10.72% |

### Result

```text
✅ Statistically Significant
```

Variant B performs better.

---

## 📧 Email Campaign

**Business Question:**

> Does a personalised email campaign increase customer conversions?

| Variant   | Conversion Rate |
| --------- | --------------: |
| Variant A |            5.8% |
| Variant B |            6.2% |

### Result

```text
⚠️ Not Statistically Significant
```

There is not enough statistical evidence to confidently conclude that one variant performs better.

---

## 💳 Pricing Page

**Business Question:**

> Does changing the pricing page design improve subscription conversions?

| Variant   | Conversion Rate |
| --------- | --------------: |
| Variant A |          12.52% |
| Variant B |            9.4% |

### Result

```text
✅ Statistically Significant
```

Variant A performs better.

---

# 🛠️ Tech Stack

| Technology                  | Purpose                                          |
| --------------------------- | ------------------------------------------------ |
| **ASP.NET Core MVC**        | Full-stack web application                       |
| **.NET 8**                  | Application framework                            |
| **C#**                      | Backend development and statistical calculations |
| **SQL Server LocalDB**      | Database                                         |
| **Entity Framework Core 8** | ORM and database access                          |
| **EF Core Migrations**      | Database schema management                       |
| **Bootstrap 5**             | Responsive UI components                         |
| **Bootstrap Icons**         | Dashboard icons                                  |
| **Custom CSS**              | Visual design and styling                        |
| **JavaScript**              | Client-side functionality                        |
| **Chart.js 4**              | Interactive data visualisation                   |

---

# 🎨 Application Design

InsightLab is designed to look like a modern analytics platform.

### UI Features

* 🎨 Modern dashboard layout
* 📌 Sidebar navigation
* 🃏 KPI cards
* 📊 Interactive charts
* 🏷️ Status badges
* 📋 Responsive tables
* ✨ Hover effects
* 🔍 Search and filtering
* 📱 Responsive layout
* 🌙 Clean, professional visual styling

The focus is on making the data easy to understand while maintaining a polished portfolio appearance.

---

# 📁 Project Structure

```text
InsightLab/
│
├── InsightLab.sln
│
└── InsightLab.Web/
    │
    ├── Controllers/
    │   ├── DashboardController.cs
    │   ├── ExperimentsController.cs
    │   └── DataExplorerController.cs
    │
    ├── Models/
    │   ├── Experiment.cs
    │   └── ExperimentParticipant.cs
    │
    ├── ViewModels/
    │   ├── DashboardViewModel.cs
    │   ├── ExperimentAnalysisViewModel.cs
    │   ├── DataExplorerViewModel.cs
    │   └── ExperimentSummaryViewModel.cs
    │
    ├── Services/
    │   ├── IStatisticsService.cs
    │   └── StatisticsService.cs
    │
    ├── Data/
    │   ├── AppDbContext.cs
    │   └── DbSeeder.cs
    │
    ├── Views/
    │   ├── Dashboard/
    │   │   └── Index.cshtml
    │   │
    │   ├── Experiments/
    │   │   ├── Index.cshtml
    │   │   └── Details.cshtml
    │   │
    │   ├── DataExplorer/
    │   │   └── Index.cshtml
    │   │
    │   └── Shared/
    │       ├── _Layout.cshtml
    │       └── _Sidebar.cshtml
    │
    └── wwwroot/
        │
        ├── css/
        │   └── site.css
        │
        └── js/
            ├── charts.js
            └── site.js
```

---

# 🚀 Getting Started

## Prerequisites

Before running InsightLab, make sure you have:

* .NET 8 SDK
* Visual Studio 2022
* ASP.NET and Web Development workload
* SQL Server LocalDB

Check your .NET installation:

```powershell
dotnet --version
```

Check LocalDB:

```powershell
sqllocaldb info
```

---

# 💻 Running the Project

## 1️⃣ Open the Solution

Open:

```text
InsightLab.sln
```

in **Visual Studio 2022**.

Allow Visual Studio to restore the required NuGet packages.

---

## 2️⃣ Create the Database

Open:

```text
Tools
→ NuGet Package Manager
→ Package Manager Console
```

Run:

```powershell
Add-Migration InitialCreate
```

Then:

```powershell
Update-Database
```

This creates the InsightLab database using the Entity Framework Core models.

---

## 3️⃣ Run the Application

Press:

```text
F5
```

or select:

```text
▶ Run
```

The application will launch in your browser.

On first startup, sample experiment data is automatically generated.

---

# 📸 Screenshots

The following screenshots are recommended for your GitHub repository and portfolio.

### 1. 📊 Main Dashboard

Capture:

* KPI cards
* Conversion charts
* Experiment overview
* Results table

---

### 2. 🧪 Experiments Page

Capture:

* Experiment cards
* Variant comparison
* Significance badges

---

### 3. 🔬 Statistical Analysis

Capture the detailed analysis page showing:

* Variant A vs Variant B
* Conversion rates
* P-value
* Z-score
* Statistical significance
* Business recommendation

---

### 4. 📂 Data Explorer

Capture:

* Search functionality
* Filters
* Raw participant data

---

## Screenshots:
<img width="1600" height="722" alt="image" src="https://github.com/user-attachments/assets/e253d91c-2ad8-4930-a991-c46ea6a7ada6" />

<img width="1600" height="726" alt="image" src="https://github.com/user-attachments/assets/781ce06b-268a-45b8-ac83-db0a5ca59953" />


## 🧪 Experiment Analysis

<img width="1600" height="717" alt="image" src="https://github.com/user-attachments/assets/8d61ac41-509b-4430-9f49-ba6f3c9e0ca6" />

<img width="1600" height="728" alt="image" src="https://github.com/user-attachments/assets/35ce956d-9e05-4cb0-b4e3-025483aaee0a" />

<img width="1600" height="728" alt="image" src="https://github.com/user-attachments/assets/069e3817-d40c-437b-a9d2-81871dc639bc" />

<img width="1600" height="719" alt="image" src="https://github.com/user-attachments/assets/775d2ee5-c4c7-49bb-95f2-7f88c669c439" />







```

---

# 🎯 Skills Demonstrated

This project demonstrates practical skills relevant to **Junior Data Analyst and Junior BI Analyst roles**.

### 📊 Data Analysis

* Conversion analysis
* Performance comparison
* KPI calculation
* Data exploration
* Business insight generation

### 🧮 Statistics

* Descriptive statistics
* Mean
* Median
* Standard deviation
* Hypothesis testing
* Two-proportion Z-test
* P-values
* Statistical significance
* Confidence levels

### 🗄️ Databases

* SQL Server
* Relational database design
* Primary and foreign keys
* Entity relationships
* Entity Framework Core
* Code-First development
* Database migrations

### 💻 Development

* C#
* ASP.NET Core MVC
* Controllers
* ViewModels
* Service architecture
* LINQ
* Dependency Injection

### 📈 Data Visualisation

* Chart.js
* KPI dashboards
* Conversion comparison
* Trend analysis
* Interactive visualisations

---

# 💼 Portfolio Value

InsightLab was created to demonstrate an important part of data analysis that is often missing from beginner portfolios:

> **Making a business decision based on statistical evidence rather than simply displaying data.**

The project demonstrates the complete analytics process:

```text
Business Question
       ↓
Collect Experiment Data
       ↓
Explore & Describe the Data
       ↓
Compare Variants
       ↓
Perform Statistical Test
       ↓
Calculate P-Value
       ↓
Determine Statistical Significance
       ↓
Generate Business Recommendation
```

---

# 🧠 Example Business Insight

Instead of simply saying:

> Variant B has a higher conversion rate.

InsightLab asks:

> **Is the difference large enough to be statistically meaningful?**

Example:

```text
Variant A: 8.2%
Variant B: 10.72%

Difference: +2.52%

P-Value: 0.03

Significance Level: 0.05
```

Because:

```text
0.03 < 0.05
```

The result is:

> **Statistically Significant ✅**

The dashboard can therefore provide the recommendation:

> **Variant B demonstrated a statistically significant improvement in conversion rate and should be considered for implementation.**

---

# 🔮 Future Improvements

Possible future improvements include:

* 📤 CSV data import
* 📥 Export experiment results to Excel
* 👤 User authentication
* 🔐 Role-based access
* 📊 Additional statistical tests
* 🔄 Confidence intervals
* 📈 Statistical power analysis
* 🧪 Create experiments through the dashboard
* ☁️ Deploy to Azure
* 🐳 Docker support
* 🧾 Automated PDF reporting
* 🔔 Experiment result notifications

---

# 👨‍💻 Author

**Diya Lakha**

Aspiring **Software Developer | Junior Data Analyst | Junior BI Analyst**

---

# ⭐ Final Project Summary

InsightLab combines:

```text
📊 Data Analysis
      +
🧮 Statistics
      +
🧪 A/B Testing
      +
🗄️ SQL
      +
💻 C#
      +
📈 Data Visualisation
      =
🚀 InsightLab


---

⭐ If you found this project interesting, feel free to explore the code and experiment with the data!
