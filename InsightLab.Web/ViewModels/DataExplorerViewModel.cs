using InsightLab.Web.Models;

namespace InsightLab.Web.ViewModels
{
    /// <summary>
    /// A single row shown in the Data Explorer table. We flatten
    /// ExperimentParticipant + Experiment name into one simple shape so the
    /// view doesn't need to worry about navigation properties.
    /// </summary>
    public class ParticipantRowViewModel
    {
        public int ParticipantId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string ExperimentName { get; set; } = string.Empty;
        public string Variant { get; set; } = string.Empty;
        public DateTime SessionDate { get; set; }
        public bool Converted { get; set; }
        public decimal OrderValue { get; set; }
    }

    /// <summary>
    /// The filterable/searchable table of raw participant data shown on the
    /// Data Explorer page, plus the currently selected filter values so the
    /// view can keep the dropdowns in sync after a search.
    /// </summary>
    public class DataExplorerViewModel
    {
        public List<ParticipantRowViewModel> Participants { get; set; } = new();

        public List<Experiment> AllExperiments { get; set; } = new();

        // --- Currently applied filters ---
        public int? SelectedExperimentId { get; set; }
        public string? SelectedVariant { get; set; }
        public string? SelectedConversionStatus { get; set; } // "Converted" / "NotConverted"
        public string? SearchTerm { get; set; }

        public int TotalMatchingRows { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling(TotalMatchingRows / (double)PageSize);
    }
}
