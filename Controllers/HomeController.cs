using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PatientJournalMVC.Models;

namespace PatientJournalMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedInPersonnummer"));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}