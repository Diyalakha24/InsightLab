using InsightLab.Web.Models;

namespace InsightLab.Web.Data
{
    /// <summary>
    /// Populates the database with three realistic A/B testing experiments the
    /// very first time the application runs. Everything is generated with a
    /// FIXED random seed (42) so the numbers you see are reproducible every
    /// time you drop and recreate the database — handy for demos and
    /// screenshots.
    ///
    /// Rather than flipping a random coin per-row and hoping the totals land
    /// close to the target conversion rate, we calculate the exact number of
    /// conversions each variant needs up front, build a list of true/false
    /// "did this session convert" flags of the right size, and shuffle it
    /// with the seeded Random instance. This guarantees the seeded data
    /// always matches the intended story (e.g. Checkout Redesign really is
    /// statistically significant) instead of being at the mercy of chance.
    /// </summary>
    public static class DbSeeder
    {
        private const int RandomSeed = 42;

        public static void Seed(AppDbContext context)
        {
            // Idempotent: never seed twice.
            if (context.Experiments.Any())
            {
                return;
            }

            var random = new Random(RandomSeed);

            var experiments = new List<Experiment>
            {
                BuildCheckoutRedesign(random),
                BuildEmailCampaign(random),
                BuildPricingPage(random)
            };

            context.Experiments.AddRange(experiments);
            context.SaveChanges();
        }

        private static Experiment BuildCheckoutRedesign(Random random)
        {
            var experiment = new Experiment
            {
                ExperimentName = "Checkout Redesign",
                Description = "Testing a simplified, single-page checkout flow against the original multi-step checkout to see if it reduces cart abandonment.",
                StartDate = new DateTime(2026, 7, 1),
                EndDate = new DateTime(2026, 7, 30)
            };

            // Variant A (Original Checkout): 2,500 participants, 205 conversions -> 8.2%
            // Variant B (Simplified Checkout): 2,500 participants, 268 conversions -> 10.72%
            experiment.Participants.AddRange(GenerateParticipants(
                random, experiment,
                participantsA: 2500, conversionsA: 205, orderMeanA: 420m, orderStdDevA: 80m,
                participantsB: 2500, conversionsB: 268, orderMeanB: 435m, orderStdDevB: 85m));

            return experiment;
        }

        private static Experiment BuildEmailCampaign(Random random)
        {
            var experiment = new Experiment
            {
                ExperimentName = "Email Campaign",
                Description = "Comparing the original promotional email subject line and layout (A) against a personalised, shorter version (B).",
                StartDate = new DateTime(2026, 7, 15),
                EndDate = new DateTime(2026, 8, 14)
            };

            // Variant A: 2,000 participants, 116 conversions -> 5.8%
            // Variant B: 2,000 participants, 124 conversions -> 6.2%
            experiment.Participants.AddRange(GenerateParticipants(
                random, experiment,
                participantsA: 2000, conversionsA: 116, orderMeanA: 300m, orderStdDevA: 60m,
                participantsB: 2000, conversionsB: 124, orderMeanB: 310m, orderStdDevB: 65m));

            return experiment;
        }

        private static Experiment BuildPricingPage(Random random)
        {
            var experiment = new Experiment
            {
                ExperimentName = "Pricing Page",
                Description = "Testing a new three-tier pricing layout (B) against the original single-price page (A) to see which drives more upgrades.",
                StartDate = new DateTime(2026, 8, 1),
                EndDate = new DateTime(2026, 8, 30)
            };

            // Variant A: 2,500 participants, 313 conversions -> 12.52%
            // Variant B: 2,500 participants, 235 conversions -> 9.4%
            experiment.Participants.AddRange(GenerateParticipants(
                random, experiment,
                participantsA: 2500, conversionsA: 313, orderMeanA: 650m, orderStdDevA: 120m,
                participantsB: 2500, conversionsB: 235, orderMeanB: 720m, orderStdDevB: 130m));

            return experiment;
        }

        private static List<ExperimentParticipant> GenerateParticipants(
            Random random,
            Experiment experiment,
            int participantsA, int conversionsA, decimal orderMeanA, decimal orderStdDevA,
            int participantsB, int conversionsB, decimal orderMeanB, decimal orderStdDevB)
        {
            var participants = new List<ExperimentParticipant>();

            participants.AddRange(GenerateVariantGroup(
                random, experiment, Variant.A, participantsA, conversionsA, orderMeanA, orderStdDevA));

            participants.AddRange(GenerateVariantGroup(
                random, experiment, Variant.B, participantsB, conversionsB, orderMeanB, orderStdDevB));

            return participants;
        }

        private static List<ExperimentParticipant> GenerateVariantGroup(
            Random random,
            Experiment experiment,
            Variant variant,
            int participantCount,
            int conversionCount,
            decimal orderValueMean,
            decimal orderValueStdDev)
        {
            // Build the exact set of true/false conversion outcomes, then shuffle.
            var convertedFlags = new List<bool>(participantCount);
            convertedFlags.AddRange(Enumerable.Repeat(true, conversionCount));
            convertedFlags.AddRange(Enumerable.Repeat(false, participantCount - conversionCount));
            Shuffle(random, convertedFlags);

            int totalDays = Math.Max(1, (experiment.EndDate - experiment.StartDate).Days);

            var result = new List<ExperimentParticipant>(participantCount);

            for (int i = 0; i < participantCount; i++)
            {
                bool converted = convertedFlags[i];

                int dayOffset = random.Next(0, totalDays + 1);
                var sessionDate = experiment.StartDate.AddDays(dayOffset).AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));

                decimal orderValue = 0m;
                if (converted)
                {
                    orderValue = GenerateRealisticOrderValue(random, orderValueMean, orderValueStdDev);
                }

                result.Add(new ExperimentParticipant
                {
                    SessionId = $"SES-{experiment.ExperimentName[..3].ToUpperInvariant()}-{variant}-{i + 1:00000}",
                    Variant = variant,
                    SessionDate = sessionDate,
                    Converted = converted,
                    OrderValue = orderValue
                });
            }

            return result;
        }

        /// <summary>
        /// Generates a realistic, positive order value using the Box-Muller
        /// transform to sample from a normal distribution with the given
        /// mean and standard deviation, then rounds to two decimal places.
        /// </summary>
        private static decimal GenerateRealisticOrderValue(Random random, decimal mean, decimal stdDev)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double standardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            double value = (double)mean + (double)stdDev * standardNormal;

            // Keep order values realistic (no negative or near-zero orders).
            value = Math.Max(value, (double)mean * 0.3);

            return Math.Round((decimal)value, 2);
        }

        private static void Shuffle<T>(Random random, IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
