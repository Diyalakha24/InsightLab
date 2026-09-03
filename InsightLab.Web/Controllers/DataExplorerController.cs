using InsightLab.Web.Data;
using InsightLab.Web.Models;
using InsightLab.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsightLab.Web.Controllers
{
    /// <summary>
    /// A simple, filterable, searchable table of the raw participant-level
    /// data behind every experiment.
    /// </summary>
    public class DataExplorerController : Controller
    {
        private readonly AppDbContext _context;

        public DataExplorerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            int? experimentId,
            string? variant,
            string? conversionStatus,
            string? search,
            int page = 1)
        {
            const int pageSize = 100;

            var query = _context.ExperimentParticipants
                .Include(p => p.Experiment)
                .AsQueryable();

            if (experimentId.HasValue)
            {
                query = query.Where(p => p.ExperimentId == experimentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(variant) && Enum.TryParse<Variant>(variant, true, out var parsedVariant))
            {
                query = query.Where(p => p.Variant == parsedVariant);
            }

            if (!string.IsNullOrWhiteSpace(conversionStatus))
            {
                if (conversionStatus.Equals("Converted", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Converted);
                }
                else if (conversionStatus.Equals("NotConverted", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => !p.Converted);
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();
                query = query.Where(p =>
                    p.SessionId.Contains(term) ||
                    (p.Experiment != null && p.Experiment.ExperimentName.Contains(term)));
            }

            int totalMatchingRows = await query.CountAsync();

            var rows = await query
                .OrderByDescending(p => p.SessionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ParticipantRowViewModel
                {
                    ParticipantId = p.ParticipantId,
                    SessionId = p.SessionId,
                    ExperimentName = p.Experiment != null ? p.Experiment.ExperimentName : string.Empty,
                    Variant = p.Variant.ToString(),
                    SessionDate = p.SessionDate,
                    Converted = p.Converted,
                    OrderValue = p.OrderValue
                })
                .ToListAsync();

            var viewModel = new DataExplorerViewModel
            {
                Participants = rows,
                AllExperiments = await _context.Experiments.OrderBy(e => e.ExperimentName).ToListAsync(),
                SelectedExperimentId = experimentId,
                SelectedVariant = variant,
                SelectedConversionStatus = conversionStatus,
                SearchTerm = search,
                TotalMatchingRows = totalMatchingRows,
                PageNumber = page,
                PageSize = pageSize
            };

            return View(viewModel);
        }
    }
}
