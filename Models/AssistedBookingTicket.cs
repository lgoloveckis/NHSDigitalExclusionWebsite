using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NHSDigitalExclusionWebsite.Models
{
    [Table("assisted_booking_tickets")]
    public class AssistedBookingTicket
    {
        [Key]
        [Column("ticket_id")]
        public int TicketId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("booking_id")]
        public int BookingId { get; set; }

        [Column("support_status")]
        public string SupportStatus { get; set; } = "";

        [Column("assigned_staff")]
        public string? AssignedStaff { get; set; }

        [Column("support_notes")]
        public string? SupportNotes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("resolved_date")]
        public DateTime? ResolvedDate { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }
    }
}