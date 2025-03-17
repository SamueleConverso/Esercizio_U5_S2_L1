using System.Diagnostics;
using Esercizio_U5_S2_L1.Models;
using Esercizio_U5_S2_L1.Services;
using Esercizio_U5_S2_L1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_U5_S2_L1.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly StudenteService _studenteService;

        public HomeController(ILogger<HomeController> logger, StudenteService studenteService) {
            _logger = logger;
            _studenteService = studenteService;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet("/studenti/get-all")]
        public async Task<IActionResult> GetStudenti() {
            try {
                var studenti = await _studenteService.GetAllStudentiAsync();
                //var studentiViewModel = new StudentiViewModel {
                //    Studenti = studenti.Studenti
                //};
                return PartialView("_StudentiList", studenti);
            } catch (Exception ex) {
                _logger.LogError(ex, "Errore durante il caricamento degli studenti");
                return StatusCode(500);
            }

        }

        [Route("/studente/details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id) {
            var studente = await _studenteService.GetStudenteByIdAsync(id);

            if (studente == null) {
                TempData["Error"] = "Error while finding entity on database";
                return RedirectToAction("Index");
            }

            var studenteViewModel = new StudenteDetailsViewModel() {
                Id = studente.Id,
                Nome = studente.Nome,
                Cognome = studente.Cognome,
                DataDiNascita = studente.DataDiNascita,
                Email = studente.Email
            };

            return Json(studenteViewModel);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
