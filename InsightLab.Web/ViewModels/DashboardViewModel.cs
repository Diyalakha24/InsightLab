namespace InsightLab.Web.ViewModels
{
    /// <summary>
    /// Everything the Dashboard/Index view needs: the four KPI cards, the
    /// two overview charts, and the recent experiment results table.
    /// </summary>
    public class DashboardViewModel
    {
        // --- KPI cards ---
        public int TotalExperiments { get; set; }
        public int TotalParticipants { get; set; }
        public double OverallConversionRate { get; set; }
        public string BestPerformingVariant { get; set; } = string.Empty;

        // --- Charts + table share the same summary rows ---
        public List<ExperimentSummaryViewModel> ExperimentSummaries { get; set; } = new();
    }
}
