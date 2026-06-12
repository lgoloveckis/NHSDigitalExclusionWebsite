using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Models;


namespace NHSDigitalExclusionWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<FailureReason> FailureReasons { get; set; }
        public DbSet<BookingAttempt> BookingAttempts { get; set; }
        public DbSet<AssistedBookingTicket> AssistedBookingTickets { get; set; }
        public DbSet<TicketAuditLog> TicketAuditLogs { get; set; }
    }
}