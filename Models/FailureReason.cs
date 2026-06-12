using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NHSDigitalExclusionWebsite.Models
{
    [Table("failure_reasons")]
    public class FailureReason
    {
        [Key]
        [Column("reason_id")]
        public int ReasonId { get; set; }

        [Column("reason_name")]
        public string ReasonName { get; set; } = "";

        [Column("reason_description")]
        public string ReasonDescription { get; set; } = "";

        [Column("risk_points")]
        public int RiskPoints { get; set; }
    }
}