# InsightLab — A/B Testing & Statistical Analysis Dashboard

InsightLab is a modern analytics dashboard that simulates a business running A/B testing experiments (checkout redesigns, email campaigns, pricing pages) and answers the question every product/data team asks: **"did Variant B actually beat Variant A, or did we just get lucky?"**

Built as a portfolio project to demonstrate practical statistical analysis (descriptive statistics + two-proportion Z-test hypothesis testing) inside a real, working full-stack ASP.NET Core MVC application.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4) ![EF Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4) ![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3) ![Chart.js](https://img.shields.io/badge/Chart.js-4.4-FF6384)

## Features

- **Dashboard** — KPI cards (total experiments, participants, overall conversion rate, best-performing variant), a conversion-rate comparison chart, a performance overview radar chart, and a results table with significance badges.
- **Experiments** — every experiment shown as a visual card: variant conversion rates, improvement, and a "View Analysis" link.
- **Experiment Analysis** — full statistical breakdown per experiment: descriptive statistics for each variant, a two-proportion Z-test (Z-score, P-value, significance, confidence level), a plain-English verdict and recommendation, and three Chart.js visualizations (conversion comparison, daily conversions, average order value).
- **Data Explorer** — a searchable, filterable table of every raw participant record (session id, experiment, variant, date, converted, order value).

## Tech Stack

- ASP.NET Core MVC on **.NET 8**
- **Entity Framework Core 8** (Code-First + Migrations) against **SQL Server LocalDB**
- Bootstrap 5 + custom CSS design system + Bootstrap Icons
- Chart.js 4 for all charts
- Vanilla JavaScript (no SPA framework)

## Statistics, in plain English

- **Descriptive statistics** (`StatisticsService.CalculateDescriptiveStatistics`) describe the data you already have: conversion rate, mean/median/min/max order value, standard deviation.
- **The two-proportion Z-test** (`StatisticsService.RunTwoProportionZTest`) is the inferential test that answers whether the difference between Variant A and Variant B is likely to be real or just random noise:
  1. Calculate each variant's conversion rate.
  2. Calculate a *pooled* conversion rate assuming there is really no difference (the null hypothesis).
  3. Calculate the standard error of the difference between the two proportions.
  4. Z-score = observed difference ÷ standard error.
  5. Convert the Z-score into a two-tailed **P-value** using the standard normal distribution.
  6. If P-value < 0.05 (95% confidence level), the result is **statistically significant**.

Every step is commented in `Services/StatisticsService.cs` so it's easy to explain in an interview.

## Database Schema

```
Experiments                      ExperimentParticipants
------------------------         ------------------------------
ExperimentId (PK)         1───N  ParticipantId (PK)
ExperimentName                    ExperimentId (FK)
Description                       SessionId
StartDate                         Variant (A / B)
EndDate                           SessionDate
                                   Converted
                                   OrderValue
```

Seed data ships with three reproducible experiments (fixed random seed = 42):

| Experiment        | Variant A | Variant B | Result              |
|--------------------|-----------|-----------|----------------------|
| Checkout Redesign  | 8.2%      | 10.72%    | Statistically Significant |
| Email Campaign     | 5.8%      | 6.2%      | Not Statistically Significant |
| Pricing Page       | 12.52%    | 9.4%      | Statistically Significant |

## Getting Started

See **SETUP_AND_RUN.md** for full step-by-step instructions (opening in Visual Studio 2022, running migrations, pressing F5).

Quick version:

```
1. Open InsightLab.sln in Visual Studio 2022
2. Package Manager Console: Add-Migration InitialCreate
3. Package Manager Console: Update-Database
4. Press F5
```

## Project Structure

```
InsightLab/
├── InsightLab.sln
└── InsightLab.Web/
    ├── Controllers/        DashboardController, ExperimentsController, DataExplorerController
    ├── Models/              Experiment, ExperimentParticipant
    ├── ViewModels/          DashboardViewModel, ExperimentAnalysisViewModel, DataExplorerViewModel, ExperimentSummaryViewModel
    ├── Services/            IStatisticsService, StatisticsService
    ├── Data/                AppDbContext, DbSeeder
    ├── Views/               Dashboard/, Experiments/, DataExplorer/, Shared/
    └── wwwroot/             css/site.css, js/charts.js, js/site.js
```

## Screenshots

See **SETUP_AND_RUN.md** for a suggested screenshot list for your CV/portfolio.
