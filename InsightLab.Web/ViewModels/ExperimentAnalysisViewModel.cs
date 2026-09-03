using InsightLab.Web.Models;
using InsightLab.Web.Services;

namespace InsightLab.Web.ViewModels
{
    /// <summary>
    /// One data point for the "daily conversions" line chart on the
    /// Experiment Analysis page.
    /// </summary>
    public class DailyConversionPoint
    {
        public DateTime Date { get; set; }
        public int VariantAConversions { get; set; }
        public int VariantBConversions { get; set; }
    }

    /// <summary>
    /// Everything the detailed Experiment Analysis page needs: the raw
    /// experiment, descriptive statistics for each variant, the full
    /// two-proportion Z-test result, and the data used to draw the three
    /// charts (conversion rate bar chart, daily conversions line chart,
    /// average order value chart).
    /// </summary>
    public class ExperimentAnalysisViewModel
    {
        public Experiment Experiment { get; set; } = null!;

        public DescriptiveStatisticsResult DescriptiveA { get; set; } = null!;
        public DescriptiveStatisticsResult DescriptiveB { get; set; } = null!;

        public AbTestResult AbTestResult { get; set; } = null!;

        public List<DailyConversionPoint> DailyConversions { get; set; } = new();
    }
}
