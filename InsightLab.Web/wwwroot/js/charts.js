// =========================================================
// InsightLab — Chart.js helper functions.
// Each view passes its own data (rendered server-side as JSON)
// into these small wrapper functions so the chart configuration
// lives in one place.
// =========================================================

const ilColors = {
    variantA: "#5b5bf6",
    variantAFaint: "rgba(91, 91, 246, 0.15)",
    variantB: "#d6337e",
    variantBFaint: "rgba(214, 51, 126, 0.15)",
    accent: "#10b981",
    grid: "#eef0f7",
    text: "#6b7280"
};

Chart.defaults.font.family = "'Inter', sans-serif";
Chart.defaults.color = ilColors.text;
Chart.defaults.plugins.legend.labels.usePointStyle = true;
Chart.defaults.plugins.legend.labels.boxWidth = 8;

/**
 * Dashboard — Chart 1: "Conversion Rate by Experiment"
 * Grouped bar chart comparing Variant A vs Variant B for every experiment.
 */
function createConversionRateBarChart(canvasId, labels, ratesA, ratesB) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    return new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "Variant A",
                    data: ratesA,
                    backgroundColor: ilColors.variantA,
                    borderRadius: 6,
                    maxBarThickness: 34
                },
                {
                    label: "Variant B",
                    data: ratesB,
                    backgroundColor: ilColors.variantB,
                    borderRadius: 6,
                    maxBarThickness: 34
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { position: "top", align: "end" },
                tooltip: {
                    callbacks: {
                        label: (item) => `${item.dataset.label}: ${item.raw.toFixed(2)}%`
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: { color: ilColors.grid },
                    ticks: { callback: (v) => v + "%" }
                },
                x: { grid: { display: false } }
            }
        }
    });
}

/**
 * Dashboard — Chart 2: "Experiment Performance Overview"
 * Radar chart giving a quick visual read of how every experiment's two
 * variants compare on the same axes.
 */
function createPerformanceRadarChart(canvasId, labels, ratesA, ratesB) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    return new Chart(ctx, {
        type: "radar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "Variant A",
                    data: ratesA,
                    borderColor: ilColors.variantA,
                    backgroundColor: ilColors.variantAFaint,
                    pointBackgroundColor: ilColors.variantA,
                    borderWidth: 2
                },
                {
                    label: "Variant B",
                    data: ratesB,
                    borderColor: ilColors.variantB,
                    backgroundColor: ilColors.variantBFaint,
                    pointBackgroundColor: ilColors.variantB,
                    borderWidth: 2
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { position: "top", align: "end" } },
            scales: {
                r: {
                    beginAtZero: true,
                    grid: { color: ilColors.grid },
                    angleLines: { color: ilColors.grid },
                    ticks: { callback: (v) => v + "%", backdropColor: "transparent" }
                }
            }
        }
    });
}

/**
 * Experiment Details — conversion rate comparison (single experiment).
 */
function createDetailConversionChart(canvasId, rateA, rateB) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    return new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Variant A", "Variant B"],
            datasets: [{
                label: "Conversion Rate",
                data: [rateA, rateB],
                backgroundColor: [ilColors.variantA, ilColors.variantB],
                borderRadius: 8,
                maxBarThickness: 70
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: { callbacks: { label: (item) => `${item.raw.toFixed(2)}%` } }
            },
            scales: {
                y: { beginAtZero: true, grid: { color: ilColors.grid }, ticks: { callback: (v) => v + "%" } },
                x: { grid: { display: false } }
            }
        }
    });
}

/**
 * Experiment Details — daily conversions over time (line chart).
 */
function createDailyConversionsChart(canvasId, dateLabels, seriesA, seriesB) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    return new Chart(ctx, {
        type: "line",
        data: {
            labels: dateLabels,
            datasets: [
                {
                    label: "Variant A conversions",
                    data: seriesA,
                    borderColor: ilColors.variantA,
                    backgroundColor: ilColors.variantAFaint,
                    tension: 0.35,
                    fill: true,
                    pointRadius: 2
                },
                {
                    label: "Variant B conversions",
                    data: seriesB,
                    borderColor: ilColors.variantB,
                    backgroundColor: ilColors.variantBFaint,
                    tension: 0.35,
                    fill: true,
                    pointRadius: 2
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { position: "top", align: "end" } },
            scales: {
                y: { beginAtZero: true, grid: { color: ilColors.grid } },
                x: { grid: { display: false } }
            }
        }
    });
}

/**
 * Experiment Details — average order value comparison.
 */
function createOrderValueChart(canvasId, valueA, valueB) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    return new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Variant A", "Variant B"],
            datasets: [{
                label: "Average Order Value",
                data: [valueA, valueB],
                backgroundColor: [ilColors.variantA, ilColors.variantB],
                borderRadius: 8,
                maxBarThickness: 70
            }]
        },
        options: {
            indexAxis: "y",
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: { callbacks: { label: (item) => "R" + item.raw.toFixed(2) } }
            },
            scales: {
                x: { beginAtZero: true, grid: { color: ilColors.grid }, ticks: { callback: (v) => "R" + v } },
                y: { grid: { display: false } }
            }
        }
    });
}
