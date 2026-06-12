using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NHSDigitalExclusionWebsite.Models
{
    [Table("booking_attempts")]
    public class BookingAttempt
    {
        [Key]
        [Column("booking_id")]
        public int BookingId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("booking_channel")]
        public string BookingChannel { get; set; } = "";

        [Column("booking_outcome")]
        public string BookingOutcome { get; set; } = "";


        [Column("failure_reason")]
        public string? FailureReason { get; set; }

        [Column("attempt_datetime")]
        public DateTime AttemptDatetime { get; set; }

        [Column("notes")]
        public string? Notes { get; set; } = "";

        [Column("risk_score")]
        public int RiskScore { get; set; }

        [Column("risk_level")]
        public string RiskLevel { get; set; } = "";
    }
}