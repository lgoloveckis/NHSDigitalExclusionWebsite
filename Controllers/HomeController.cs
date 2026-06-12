using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NHSDigitalExclusionWebsite.Models;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Data;

namespace NHSDigitalExclusionWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // to make cards visible on Dashboard
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalPatients = await _context.Patients.CountAsync();
            ViewBag.TotalBookings = await _context.BookingAttempts.CountAsync();
            ViewBag.FailedBookings = await _context.BookingAttempts
                .CountAsync(b => b.BookingOutcome == "Failed");
            ViewBag.HighRiskBookings = await _context.BookingAttempts
                .CountAsync(b => b.RiskLevel == "High");
            ViewBag.OpenTickets = await _context.AssistedBookingTickets
                .CountAsync(t => t.SupportStatus == "Open");

            ViewBag.LowRisk = await _context.BookingAttempts.CountAsync(b => b.RiskLevel == "Low");
            ViewBag.MediumRisk = await _context.BookingAttempts.CountAsync(b => b.RiskLevel == "Medium");
            ViewBag.HighRisk = await _context.BookingAttempts.CountAsync(b => b.RiskLevel == "High");

            ViewBag.OpenTicketsChart = await _context.AssistedBookingTickets.CountAsync(t => t.SupportStatus == "Open");
            ViewBag.InProgressTicketsChart = await _context.AssistedBookingTickets.CountAsync(t => t.SupportStatus == "In Progress");
            ViewBag.ClosedTicketsChart = await _context.AssistedBookingTickets.CountAsync(t => t.SupportStatus == "Closed");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
