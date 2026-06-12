
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Models;
using NHSDigitalExclusionWebsite.Data;

public class FailureReasonsController : Controller
{
    private readonly ApplicationDbContext _context;

    public FailureReasonsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: FAILUREREASONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.FailureReasons.ToListAsync());
    }

    // GET: FAILUREREASONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var failurereason = await _context.FailureReasons
            .FirstOrDefaultAsync(m => m.ReasonId == id);
        if (failurereason == null)
        {
            return NotFound();
        }

        return View(failurereason);
    }

    // GET: FAILUREREASONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FAILUREREASONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ReasonId,ReasonName,ReasonDescription,RiskPoints")] FailureReason failurereason)
    {
        if (ModelState.IsValid)
        {
            _context.Add(failurereason);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(failurereason);
    }

    // GET: FAILUREREASONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var failurereason = await _context.FailureReasons.FindAsync(id);
        if (failurereason == null)
        {
            return NotFound();
        }
        return View(failurereason);
    }

    // POST: FAILUREREASONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ReasonId,ReasonName,ReasonDescription,RiskPoints")] FailureReason failurereason)
    {
        if (id != failurereason.ReasonId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(failurereason);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FailureReasonExists(failurereason.ReasonId))
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
        return View(failurereason);
    }

    // GET: FAILUREREASONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var failurereason = await _context.FailureReasons
            .FirstOrDefaultAsync(m => m.ReasonId == id);
        if (failurereason == null)
        {
            return NotFound();
        }

        return View(failurereason);
    }

    // POST: FAILUREREASONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var failurereason = await _context.FailureReasons.FindAsync(id);
        if (failurereason != null)
        {
            _context.FailureReasons.Remove(failurereason);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FailureReasonExists(int? id)
    {
        return _context.FailureReasons.Any(e => e.ReasonId == id);
    }
}
