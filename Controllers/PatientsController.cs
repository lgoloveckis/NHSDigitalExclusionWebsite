
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHSDigitalExclusionWebsite.Data;
using NHSDigitalExclusionWebsite.Models;

[Authorize(Roles = "Admin,SupportStaff")]
public class PatientsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PATIENTS. To be able to use search box on Patients Index.cshtml for autocomplete suggestions
     public async Task<IActionResult> Index(string searchString)
    {
        var patients = from p in _context.Patients
                       select p;

        if (!string.IsNullOrEmpty(searchString))
        {
            patients = patients.Where(p =>
                p.NhsNumber.Contains(searchString) ||
                p.FirstName.Contains(searchString) ||
                p.LastName.Contains(searchString) ||
                p.City.Contains(searchString));
        }

        ViewData["CurrentFilter"] = searchString;

        return View(await patients.ToListAsync());
    }

    // GET: PATIENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient = await _context.Patients
            .FirstOrDefaultAsync(m => m.PatientId == id);
        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    // GET: PATIENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PATIENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PatientId,NhsNumber,FirstName,LastName,DateOfBirth,Gender,Phone,Email,Postcode,City")] Patient patient)
    {
        if (ModelState.IsValid)
        {
            _context.Add(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

    // GET: PATIENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    // POST: PATIENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("PatientId,NhsNumber,FirstName,LastName,DateOfBirth,Gender,Phone,Email,Postcode,City")] Patient patient)
    {
        if (id != patient.PatientId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(patient);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.PatientId))
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
        return View(patient);
    }

    // GET: PATIENTS/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient = await _context.Patients
            .FirstOrDefaultAsync(m => m.PatientId == id);
        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    // POST: PATIENTS/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient != null)
        {
            _context.Patients.Remove(patient);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PatientExists(int? id)
    {
        return _context.Patients.Any(e => e.PatientId == id);
    }
      // this method needed for executing autocomplete search box
    public async Task<IActionResult> SearchSuggestions(string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return Json(new List<string>());
        }

        var suggestions = await _context.Patients
            .Where(p =>
                p.NhsNumber.Contains(term) ||
                p.FirstName.Contains(term) ||
                p.LastName.Contains(term) ||
                p.City.Contains(term))
            .Select(p => p.NhsNumber + " - " + p.FirstName + " " + p.LastName)
            .Take(5)
            .ToListAsync();

        return Json(suggestions);
    }
}
