using InsightLab.Web.Models;

namespace InsightLab.Web.Services
{
    /// <summary>
    /// Descriptive statistics calculated over a set of experiment participants
    /// (or over the order values of the users who converted).
    /// </summary>
    public class DescriptiveStatisticsResult
    {
        public int TotalParticipants { get; set; }
        public int TotalConversions { get; set; }
        public double ConversionRate { get; set; }          // e.g. 0.082 == 8.2%
        public decimal MeanOrderValue { get; set; }
        public decimal MedianOrderValue { get; set; }
        public decimal MinOrderValue { get; set; }
        public decimal MaxOrderValue { get; set; }
        public double StandardDeviation { get; set; }        // standard deviation of order values
    }

    /// <summary>
    /// The full result of comparing Variant A against Variant B using a
    /// two-proportion Z-test. This is the core "is B actually better than A"
    /// answer that drives the whole dashboard.
    /// </summary>
    public class AbTestResult
    {
        public int ParticipantsA { get; set; }
        public int ParticipantsB { get; set; }
        public int ConversionsA { get; set; }
        public int ConversionsB { get; set; }

        public double ConversionRateA { get; set; }   // proportion, e.g. 0.082
        public double ConversionRateB { get; set; }   // proportion, e.g. 0.107

        /// <summary>Variant B rate minus Variant A rate, in percentage points.</summary>
        public double ConversionDifference { get; set; }

        /// <summary>Percentage change of B relative to A: (B - A) / A * 100.</summary>
        public double RelativeImprovement { get; set; }

        public double ZScore { get; set; }
        public double PValue { get; set; }

        public double SignificanceLevel { get; set; } = 0.05;
        public double ConfidenceLevel { get; set; } = 95.0;

        public bool IsStatisticallySignificant { get; set; }

        /// <summary>"A", "B" or "Tie" — whichever variant has the higher conversion rate.</summary>
        public string WinningVariant { get; set; } = "Tie";

        public decimal MeanOrderValueA { get; set; }
        public decimal MeanOrderValueB { get; set; }
    }

    /// <summary>
    /// Central place for every statistical calculation used by the dashboard:
    /// descriptive statistics and the two-proportion Z-test used to decide
    /// whether Variant B beats Variant A.
    /// </summary>
    public interface IStatisticsService
    {
        DescriptiveStatisticsResult CalculateDescriptiveStatistics(IEnumerable<ExperimentParticipant> participants);

        AbTestResult RunTwoProportionZTest(IEnumerable<ExperimentParticipant> participants);
    }
}
