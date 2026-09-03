using InsightLab.Web.Models;

namespace InsightLab.Web.Services
{
    /// <summary>
    /// Implements the statistical calculations used across the dashboard.
    ///
    /// The two big ideas used here (good to remember for interviews):
    ///
    /// 1. DESCRIPTIVE STATISTICS simply describe the data we already have
    ///    (averages, spread, min/max) — they don't tell us anything about
    ///    whether a difference is "real" or just random noise.
    ///
    /// 2. THE TWO-PROPORTION Z-TEST is an INFERENTIAL statistic. It answers
    ///    the question: "If Variant A and Variant B actually had the SAME
    ///    true conversion rate, how likely would it be to see a difference
    ///    this big (or bigger) just by chance?" If that probability
    ///    (the P-Value) is very small (below 0.05), we conclude the
    ///    difference we observed is unlikely to be random noise, and call
    ///    it "statistically significant".
    /// </summary>
    public class StatisticsService : IStatisticsService
    {
        public DescriptiveStatisticsResult CalculateDescriptiveStatistics(IEnumerable<ExperimentParticipant> participants)
        {
            var list = participants.ToList();

            var result = new DescriptiveStatisticsResult
            {
                TotalParticipants = list.Count,
                TotalConversions = list.Count(p => p.Converted)
            };

            result.ConversionRate = result.TotalParticipants == 0
                ? 0
                : (double)result.TotalConversions / result.TotalParticipants;

            // Order value statistics only make sense for participants who
            // actually converted (non-converters have an OrderValue of 0).
            var orderValues = list.Where(p => p.Converted).Select(p => p.OrderValue).ToList();

            if (orderValues.Count > 0)
            {
                result.MeanOrderValue = Math.Round(orderValues.Average(), 2);
                result.MinOrderValue = orderValues.Min();
                result.MaxOrderValue = orderValues.Max();
                result.MedianOrderValue = CalculateMedian(orderValues);
                result.StandardDeviation = CalculateStandardDeviation(orderValues);
            }

            return result;
        }

        public AbTestResult RunTwoProportionZTest(IEnumerable<ExperimentParticipant> participants)
        {
            var list = participants.ToList();

            var groupA = list.Where(p => p.Variant == Variant.A).ToList();
            var groupB = list.Where(p => p.Variant == Variant.B).ToList();

            int n1 = groupA.Count;                      // participants in Variant A
            int n2 = groupB.Count;                       // participants in Variant B
            int x1 = groupA.Count(p => p.Converted);      // conversions in Variant A
            int x2 = groupB.Count(p => p.Converted);      // conversions in Variant B

            double p1 = n1 == 0 ? 0 : (double)x1 / n1;    // conversion rate, Variant A
            double p2 = n2 == 0 ? 0 : (double)x2 / n2;    // conversion rate, Variant B

            var result = new AbTestResult
            {
                ParticipantsA = n1,
                ParticipantsB = n2,
                ConversionsA = x1,
                ConversionsB = x2,
                ConversionRateA = p1,
                ConversionRateB = p2,
                ConversionDifference = (p2 - p1) * 100.0,                       // percentage points
                RelativeImprovement = p1 == 0 ? 0 : (p2 - p1) / p1 * 100.0,      // % change vs A
                SignificanceLevel = 0.05,
                ConfidenceLevel = 95.0
            };

            var orderA = groupA.Where(p => p.Converted).Select(p => p.OrderValue).ToList();
            var orderB = groupB.Where(p => p.Converted).Select(p => p.OrderValue).ToList();
            result.MeanOrderValueA = orderA.Count == 0 ? 0 : Math.Round(orderA.Average(), 2);
            result.MeanOrderValueB = orderB.Count == 0 ? 0 : Math.Round(orderB.Average(), 2);

            // --- Two-Proportion Z-Test ---
            //
            // Step 1: Pooled proportion. Under the "null hypothesis" that A and B
            // are really identical, the best estimate of the single, shared
            // conversion rate is simply total conversions / total participants.
            double pooledP = (n1 + n2) == 0 ? 0 : (double)(x1 + x2) / (n1 + n2);

            // Step 2: Standard error of the DIFFERENCE between two proportions,
            // assuming the null hypothesis (pooled proportion) is true.
            double standardError = (n1 == 0 || n2 == 0)
                ? 0
                : Math.Sqrt(pooledP * (1 - pooledP) * ((1.0 / n1) + (1.0 / n2)));

            // Step 3: Z-score = how many standard errors apart the two observed
            // rates are. A bigger |Z| means the observed gap is less likely to
            // be random noise.
            double zScore = standardError == 0 ? 0 : (p2 - p1) / standardError;

            // Step 4: Convert the Z-score into a two-tailed P-Value using the
            // standard normal distribution. "Two-tailed" because we care about
            // A vs B being different in EITHER direction, not just B > A.
            double pValue = TwoTailedPValueFromZ(zScore);

            result.ZScore = Math.Round(zScore, 4);
            result.PValue = Math.Round(pValue, 4);

            // Step 5: Compare the P-Value to our chosen significance level (0.05 / 95% confidence).
            result.IsStatisticallySignificant = pValue < result.SignificanceLevel;

            result.WinningVariant = p2 > p1 ? "B" : (p1 > p2 ? "A" : "Tie");

            return result;
        }

        /// <summary>
        /// Converts a Z-score into a two-tailed P-Value using the standard
        /// normal cumulative distribution function (CDF).
        /// P-Value = 2 * (1 - CDF(|z|))
        /// </summary>
        private static double TwoTailedPValueFromZ(double z)
        {
            double absZ = Math.Abs(z);
            double cdf = StandardNormalCdf(absZ);
            double pValue = 2.0 * (1.0 - cdf);

            // Clamp to a sensible [0, 1] range to avoid floating point artefacts.
            return Math.Max(0.0, Math.Min(1.0, pValue));
        }

        /// <summary>
        /// Standard normal cumulative distribution function CDF(x), i.e. the
        /// probability that a standard normal random variable is less than x.
        /// Implemented using the Abramowitz & Stegun approximation of the
        /// error function (erf), which is accurate to about 7 decimal places
        /// and needs no external statistics library.
        /// </summary>
        private static double StandardNormalCdf(double x)
        {
            return 0.5 * (1.0 + Erf(x / Math.Sqrt(2.0)));
        }

        private static double Erf(double x)
        {
            // Abramowitz and Stegun formula 7.1.26
            double sign = x < 0 ? -1.0 : 1.0;
            x = Math.Abs(x);

            const double a1 = 0.254829592;
            const double a2 = -0.284496736;
            const double a3 = 1.421413741;
            const double a4 = -1.453152027;
            const double a5 = 1.061405429;
            const double p = 0.3275911;

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }

        private static decimal CalculateMedian(List<decimal> values)
        {
            var sorted = values.OrderBy(v => v).ToList();
            int count = sorted.Count;
            if (count == 0) return 0;

            if (count % 2 == 1)
            {
                return sorted[count / 2];
            }

            return Math.Round((sorted[(count / 2) - 1] + sorted[count / 2]) / 2m, 2);
        }

        private static double CalculateStandardDeviation(List<decimal> values)
        {
            if (values.Count < 2) return 0;

            double mean = (double)values.Average();
            double sumOfSquares = values.Sum(v => Math.Pow((double)v - mean, 2));
            double variance = sumOfSquares / (values.Count - 1); // sample standard deviation

            return Math.Round(Math.Sqrt(variance), 2);
        }
    }
}
