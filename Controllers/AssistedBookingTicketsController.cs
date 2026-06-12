
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Models;
using NHSDigitalExclusionWebsite.Data;

public class AssistedBookingTicketsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssistedBookingTicketsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ASSISTEDBOOKINGTICKETS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.AssistedBookingTickets.ToListAsync());
    }

    // GET: ASSISTEDBOOKINGTICKETS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var assistedbookingticket = await _context.AssistedBookingTickets
            .FirstOrDefaultAsync(m => m.TicketId == id);
        if (assistedbookingticket == null)
        {
            return NotFound();
        }

        return View(assistedbookingticket);
    }

    // GET: ASSISTEDBOOKINGTICKETS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ASSISTEDBOOKINGTICKETS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TicketId,PatientId,BookingId,SupportStatus,AssignedStaff,SupportNotes,CreatedDate,ResolvedDate,Reason")] AssistedBookingTicket assistedbookingticket)
    {
        if (ModelState.IsValid)
        {
            _context.Add(assistedbookingticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(assistedbookingticket);
    }

    // GET: ASSISTEDBOOKINGTICKETS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var assistedbookingticket = await _context.AssistedBookingTickets.FindAsync(id);
        if (assistedbookingticket == null)
        {
            return NotFound();
        }
        return View(assistedbookingticket);
    }

    // POST: ASSISTEDBOOKINGTICKETS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("TicketId,PatientId,BookingId,SupportStatus,AssignedStaff,SupportNotes,CreatedDate,ResolvedDate,Reason")] AssistedBookingTicket assistedbookingticket)
    {
        if (id != assistedbookingticket.TicketId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (assistedbookingticket.SupportStatus == "Closed")
                {
                    assistedbookingticket.ResolvedDate = DateTime.Now;
                }
                else
                {
                    assistedbookingticket.ResolvedDate = null;
                }

                var oldTicket = await _context.AssistedBookingTickets
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TicketId == assistedbookingticket.TicketId);

                if (oldTicket != null &&
                    oldTicket.SupportStatus != assistedbookingticket.SupportStatus)
                {
                    var auditLog = new TicketAuditLog
                    {
                        TicketId = assistedbookingticket.TicketId,
                        OldStatus = oldTicket.SupportStatus,
                        NewStatus = assistedbookingticket.SupportStatus,
                        ChangedBy = User.Identity?.Name ?? "Unknown",
                        ChangedDate = DateTime.Now,
                        Notes = "Ticket status updated"
                    };

                    _context.TicketAuditLogs.Add(auditLog);
                }
                _context.Update(assistedbookingticket);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Assisted booking ticket has been saved successfully."; // to show this message
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssistedBookingTicketExists(assistedbookingticket.TicketId))
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
        return View(assistedbookingticket);
    }

    // GET: ASSISTEDBOOKINGTICKETS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var assistedbookingticket = await _context.AssistedBookingTickets
            .FirstOrDefaultAsync(m => m.TicketId == id);
        if (assistedbookingticket == null)
        {
            return NotFound();
        }

        return View(assistedbookingticket);
    }

    // POST: ASSISTEDBOOKINGTICKETS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var assistedbookingticket = await _context.AssistedBookingTickets.FindAsync(id);
        if (assistedbookingticket != null)
        {
            _context.AssistedBookingTickets.Remove(assistedbookingticket);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AssistedBookingTicketExists(int? ticketid)
    {
        return _context.AssistedBookingTickets.Any(e => e.TicketId == ticketid);
    }
}
