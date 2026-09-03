using InsightLab.Web.Data;
using InsightLab.Web.Services;
using InsightLab.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsightLab.Web.Controllers
{
    /// <summary>
    /// The main landing page: KPI cards, the "conversion rate by experiment"
    /// and "experiment performance overview" charts, and a summary table of
    /// every experiment's result.
    /// </summary>
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStatisticsService _statisticsService;

        public DashboardController(AppDbContext context, IStatisticsService statisticsService)
        {
            _context = context;
            _statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var experiments = await _context.Experiments
                .Include(e => e.Participants)
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            var summaries = new List<ExperimentSummaryViewModel>();

            foreach (var experiment in experiments)
            {
                var abResult = _statisticsService.RunTwoProportionZTest(experiment.Participants);

                summaries.Add(new ExperimentSummaryViewModel
                {
                    ExperimentId = experiment.ExperimentId,
                    ExperimentName = experiment.ExperimentName,
                    Description = experiment.Description,
                    StartDate = experiment.StartDate,
                    EndDate = experiment.EndDate,
                    TotalParticipants = experiment.Participants.Count,
                    ConversionRateA = abResult.ConversionRateA,
                    ConversionRateB = abResult.ConversionRateB,
                    AverageOrderValueA = abResult.MeanOrderValueA,
                    AverageOrderValueB = abResult.MeanOrderValueB,
                    ConversionDifference = abResult.ConversionDifference,
                    WinningVariant = abResult.WinningVariant,
                    IsStatisticallySignificant = abResult.IsStatisticallySignificant,
                    PValue = abResult.PValue
                });
            }

            int totalParticipants = experiments.Sum(e => e.Participants.Count);
            int totalConversions = experiments.Sum(e => e.Participants.Count(p => p.Converted));

            int bWins = summaries.Count(s => s.WinningVariant == "B");
            int aWins = summaries.Count(s => s.WinningVariant == "A");
            string bestVariant = bWins == aWins ? "Tie" : (bWins > aWins ? $"Variant B ({bWins}/{summaries.Count})" : $"Variant A ({aWins}/{summaries.Count})");

            var viewModel = new DashboardViewModel
            {
                TotalExperiments = experiments.Count,
                TotalParticipants = totalParticipants,
                OverallConversionRate = totalParticipants == 0 ? 0 : (double)totalConversions / totalParticipants,
                BestPerformingVariant = bestVariant,
                ExperimentSummaries = summaries
            };

            return View(viewModel);
        }
    }
}
