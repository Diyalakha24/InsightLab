namespace InsightLab.Web.ViewModels
{
    /// <summary>
    /// A lightweight summary of one experiment's A/B test result.
    /// Used by the Dashboard results table, the Dashboard charts and the
    /// Experiments card grid so we don't repeat the same shape three times.
    /// </summary>
    public class ExperimentSummaryViewModel
    {
        public int ExperimentId { get; set; }
        public string ExperimentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TotalParticipants { get; set; }

        public double ConversionRateA { get; set; }
        public double ConversionRateB { get; set; }

        public decimal AverageOrderValueA { get; set; }
        public decimal AverageOrderValueB { get; set; }

        /// <summary>Difference in percentage points (B - A).</summary>
        public double ConversionDifference { get; set; }

        public string WinningVariant { get; set; } = "Tie"; // "A", "B" or "Tie"

        public bool IsStatisticallySignificant { get; set; }

        public double PValue { get; set; }

        public string ResultLabel => IsStatisticallySignificant ? "Significant" : "Not Significant";
    }
}
