
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Models;
using NHSDigitalExclusionWebsite.Data;

public class TicketAuditLogsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TicketAuditLogsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TICKETAUDITLOGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TicketAuditLogs.ToListAsync());
    }

    // GET: TICKETAUDITLOGS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticketauditlog = await _context.TicketAuditLogs
            .FirstOrDefaultAsync(m => m.AuditId == id);
        if (ticketauditlog == null)
        {
            return NotFound();
        }

        return View(ticketauditlog);
    }

    // GET: TICKETAUDITLOGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TICKETAUDITLOGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AuditId,TicketId,OldStatus,NewStatus,ChangedBy,ChangedDate,Notes")] TicketAuditLog ticketauditlog)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ticketauditlog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(ticketauditlog);
    }

    // GET: TICKETAUDITLOGS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticketauditlog = await _context.TicketAuditLogs.FindAsync(id);
        if (ticketauditlog == null)
        {
            return NotFound();
        }
        return View(ticketauditlog);
    }

    // POST: TICKETAUDITLOGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("AuditId,TicketId,OldStatus,NewStatus,ChangedBy,ChangedDate,Notes")] TicketAuditLog ticketauditlog)
    {
        if (id != ticketauditlog.AuditId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ticketauditlog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketAuditLogExists(ticketauditlog.AuditId))
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
        return View(ticketauditlog);
    }

    // GET: TICKETAUDITLOGS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticketauditlog = await _context.TicketAuditLogs
            .FirstOrDefaultAsync(m => m.AuditId == id);
        if (ticketauditlog == null)
        {
            return NotFound();
        }

        return View(ticketauditlog);
    }

    // POST: TICKETAUDITLOGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var ticketauditlog = await _context.TicketAuditLogs.FindAsync(id);
        if (ticketauditlog != null)
        {
            _context.TicketAuditLogs.Remove(ticketauditlog);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TicketAuditLogExists(int? id)
    {
        return _context.TicketAuditLogs.Any(e => e.AuditId == id);
    }
}
