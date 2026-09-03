using System.ComponentModel.DataAnnotations;

namespace InsightLab.Web.Models
{
    /// <summary>
    /// Represents a single A/B testing experiment run by the business
    /// (e.g. "Checkout Redesign", "Email Campaign", "Pricing Page").
    /// An Experiment has many ExperimentParticipants (one-to-many relationship).
    /// </summary>
    public class Experiment
    {
        [Key]
        public int ExperimentId { get; set; }

        [Required]
        [StringLength(150)]
        public string ExperimentName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// Navigation property: every participant (session) that took part
        /// in this experiment, split across Variant A and Variant B.
        /// </summary>
        public List<ExperimentParticipant> Participants { get; set; } = new();
    }
}
