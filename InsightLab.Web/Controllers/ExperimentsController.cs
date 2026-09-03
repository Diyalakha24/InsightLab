using InsightLab.Web.Data;
using InsightLab.Web.Models;
using InsightLab.Web.Services;
using InsightLab.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsightLab.Web.Controllers
{
    /// <summary>
    /// Lists every experiment as a card (Index) and shows the full
    /// statistical breakdown for one experiment (Details).
    /// </summary>
    public class ExperimentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStatisticsService _statisticsService;

        public ExperimentsController(AppDbContext context, IStatisticsService statisticsService)
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

            var summaries = experiments.Select(experiment =>
            {
                var abResult = _statisticsService.RunTwoProportionZTest(experiment.Participants);

                return new ExperimentSummaryViewModel
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
                };
            }).ToList();

            return View(summaries);
        }

        public async Task<IActionResult> Details(int id)
        {
            var experiment = await _context.Experiments
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.ExperimentId == id);

            if (experiment == null)
            {
                return NotFound();
            }

            var groupA = experiment.Participants.Where(p => p.Variant == Variant.A);
            var groupB = experiment.Participants.Where(p => p.Variant == Variant.B);

            var descriptiveA = _statisticsService.CalculateDescriptiveStatistics(groupA);
            var descriptiveB = _statisticsService.CalculateDescriptiveStatistics(groupB);
            var abResult = _statisticsService.RunTwoProportionZTest(experiment.Participants);

            var dailyConversions = experiment.Participants
                .GroupBy(p => p.SessionDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new DailyConversionPoint
                {
                    Date = g.Key,
                    VariantAConversions = g.Count(p => p.Variant == Variant.A && p.Converted),
                    VariantBConversions = g.Count(p => p.Variant == Variant.B && p.Converted)
                })
                .ToList();

            var viewModel = new ExperimentAnalysisViewModel
            {
                Experiment = experiment,
                DescriptiveA = descriptiveA,
                DescriptiveB = descriptiveB,
                AbTestResult = abResult,
                DailyConversions = dailyConversions
            };

            return View(viewModel);
        }
    }
}
