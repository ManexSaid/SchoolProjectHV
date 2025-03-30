using Microsoft.AspNetCore.Mvc;
using PatientJournalMVC.Models;

namespace PatientJournalMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApiService _apiService;
        private const string SessionKeyPersonnummer = "LoggedInPersonnummer";
        private const string SessionKeyFullName = "LoggedInFullName";


        public PatientController(ApiService apiService)
        {
            _apiService = apiService;
        }


        public async Task<IActionResult> Journal()
        {
            var personnummer = HttpContext.Session.GetString(SessionKeyPersonnummer);
            if (string.IsNullOrEmpty(personnummer))
            {
                return RedirectToAction("Index", "Home");
            }

            var patient = await _apiService.GetPatientByPersonnummerAsync(personnummer);
            var journals = await _apiService.GetJournalsByPersonnummerAsync(personnummer);

            var viewModel = new PatientJournalViewModel
            {
                Patient = patient,
                Journals = journals ?? new List<Journal>()
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string personnummer)
        {
            if (string.IsNullOrWhiteSpace(personnummer))
            {
                TempData["LoginError"] = "Personnummer måste anges.";
                return RedirectToAction("Index", "Home");
            }

            var patient = await _apiService.GetPatientByPersonnummerAsync(personnummer);

            if (patient != null)
            {

                HttpContext.Session.SetString(SessionKeyPersonnummer, patient.Personnummer);
                HttpContext.Session.SetString(SessionKeyFullName, $"{patient.Fornamn} {patient.Efternamn}");
                return RedirectToAction("Journal");
            }
            else
            {
                TempData["LoginError"] = "Inget konto hittades för detta personnummer.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /Patient/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJournal(PatientJournalViewModel model)
        {
            var personnummer = HttpContext.Session.GetString(SessionKeyPersonnummer);
            if (string.IsNullOrEmpty(personnummer))
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(model.NewAnteckning))
            {
                TempData["JournalError"] = "Anteckningen får inte vara tom.";
                return RedirectToAction("Journal");
            }

            var newJournal = new Journal
            {
                Personnummer = personnummer,
                Anteckning = model.NewAnteckning
            };

            var createdJournal = await _apiService.CreateJournalAsync(newJournal);

            if (createdJournal == null)
            {
                TempData["JournalError"] = "Kunde inte spara anteckningen. Försök igen.";
            }

            return RedirectToAction("Journal");
        }
    }
}