
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Models;
using NHSDigitalExclusionWebsite.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

public class BookingAttemptsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingAttemptsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: BOOKINGATTEMPTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.BookingAttempts.ToListAsync());
    }

    // GET: BOOKINGATTEMPTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookingattempt = await _context.BookingAttempts
            .FirstOrDefaultAsync(m => m.BookingId == id);
        if (bookingattempt == null)
        {
            return NotFound();
        }

        return View(bookingattempt);
    }

    // GET: BOOKINGATTEMPTS/Create
    public IActionResult Create()
    {
        ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "NhsNumber");
        ViewData["FailureReason"] = new SelectList(_context.FailureReasons, "ReasonName", "ReasonName");
        return View();
    }

    // POST: BOOKINGATTEMPTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BookingId,PatientId,BookingChannel,BookingOutcome,FailureReason,AttemptDatetime,Notes,RiskScore,RiskLevel")] BookingAttempt bookingattempt)
    {
        bookingattempt.RiskScore = CalculateRiskScore(
        bookingattempt.BookingOutcome,
        bookingattempt.BookingChannel,
        bookingattempt.FailureReason
);

        bookingattempt.RiskLevel = GetRiskLevel(bookingattempt.RiskScore);
        if (ModelState.IsValid)
        {
            _context.Add(bookingattempt);
            await _context.SaveChangesAsync();

            if (bookingattempt.RiskLevel == "High")
            {
                var ticket = new AssistedBookingTicket
                {
                    PatientId = bookingattempt.PatientId,
                    BookingId = bookingattempt.BookingId,
                    SupportStatus = "Open",
                    AssignedStaff = null,
                    SupportNotes = "High risk booking detected. Patient may need assisted support.",
                    CreatedDate = DateTime.Now,
                    ResolvedDate = null,
                    Reason = bookingattempt.FailureReason
                };

                _context.AssistedBookingTickets.Add(ticket);
                await _context.SaveChangesAsync();

                TempData["Message"] = "High risk booking detected. Assisted booking ticket created automatically.";
            }
            else
            {
                TempData["Message"] = "Booking attempt saved successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
        return View(bookingattempt);
    }

    // GET: BOOKINGATTEMPTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookingattempt = await _context.BookingAttempts.FindAsync(id);
        if (bookingattempt == null)
        {
            return NotFound();
        }
        return View(bookingattempt);
    }

    // POST: BOOKINGATTEMPTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("BookingId,PatientId,BookingChannel,BookingOutcome,FailureReason,AttemptDatetime,Notes,RiskScore,RiskLevel")] BookingAttempt bookingattempt)
    {
        if (id != bookingattempt.BookingId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(bookingattempt);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingAttemptExists(bookingattempt.BookingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(bookingattempt);
    }

    // GET: BOOKINGATTEMPTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookingattempt = await _context.BookingAttempts
            .FirstOrDefaultAsync(m => m.BookingId == id);
        if (bookingattempt == null)
        {
            return NotFound();
        }

        return View(bookingattempt);
    }

    // POST: BOOKINGATTEMPTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var bookingattempt = await _context.BookingAttempts.FindAsync(id);
        if (bookingattempt != null)
        {
            _context.BookingAttempts.Remove(bookingattempt);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookingAttemptExists(int? id)
    {
        return _context.BookingAttempts.Any(e => e.BookingId == id);
    }
    private int CalculateRiskScore(string outcome, string channel, string? failureReason)
    {
        int score = 0;

        if (outcome == "Failed")
        {
            score += 50;
        }

        if (channel == "Online")
        {
            score += 20;
        }
        else if (channel == "Phone")
        {
            score += 10;
        }
        else if (channel == "Walk-in")
        {
            score += 5;
        }

        if (!string.IsNullOrEmpty(failureReason))
        {
            var reason = _context.FailureReasons
                .FirstOrDefault(r => r.ReasonName == failureReason);

            if (reason != null)
            {
                score += reason.RiskPoints;
            }
        }

        return score;
    }

    private string GetRiskLevel(int score)
    {
        if (score >= 80)
        {
            return "High";
        }
        else if (score >= 50)
        {
            return "Medium";
        }
        else
        {
            return "Low";
        }
    }
    // method to export csv file
    public async Task<IActionResult> ExportCsv()
    {
        var bookings = await _context.BookingAttempts.ToListAsync();

        var csv = new System.Text.StringBuilder();

        csv.AppendLine("BookingId,PatientId,BookingChannel,BookingOutcome,FailureReason,AttemptDatetime,Notes,RiskScore,RiskLevel");

        foreach (var b in bookings)
        {
            csv.AppendLine($"{b.BookingId},{b.PatientId},{b.BookingChannel},{b.BookingOutcome},{b.FailureReason},{b.AttemptDatetime},{b.Notes},{b.RiskScore},{b.RiskLevel}");
        }

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());

        return File(bytes, "text/csv", "booking_attempts.csv");
    }
}
