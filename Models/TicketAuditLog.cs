using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NHSDigitalExclusionWebsite.Models
{
    [Table("ticket_audit_logs")]
    public class TicketAuditLog
    {
        [Key]
        [Column("audit_id")]
        public int AuditId { get; set; }

        [Column("ticket_id")]
        public int TicketId { get; set; }

        [Column("old_status")]
        public string OldStatus { get; set; } = "";

        [Column("new_status")]
        public string NewStatus { get; set; } = "";

        [Column("changed_by")]
        public string ChangedBy { get; set; } = "";

        [Column("changed_date")]
        public DateTime ChangedDate { get; set; }

        [Column("notes")]
        public string Notes { get; set; } = "";
    }
}