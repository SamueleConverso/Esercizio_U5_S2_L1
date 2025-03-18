using System.Diagnostics;
using Esercizio_U5_S2_L1.Models;
using Esercizio_U5_S2_L1.Services;
using Esercizio_U5_S2_L1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_U5_S2_L1.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly StudenteService _studenteService;
        private readonly LoggerService _loggerService;

        public HomeController(ILogger<HomeController> logger, StudenteService studenteService, LoggerService loggerService) {
            _logger = logger;
            _studenteService = studenteService;
            _loggerService = loggerService;
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
                //_logger.LogError(ex, "Errore durante il caricamento degli studenti");
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

        [Route("/studente/update/{id:guid}")]
        public async Task<IActionResult> UpdateStudente(Guid id) {
            var studente = await _studenteService.GetStudenteByIdAsync(id);

            if (studente == null) {
                TempData["Error"] = "Error while finding entity on database";
                return RedirectToAction("Index");
            }

            var updateStudenteViewModel = new UpdateStudenteViewModel() {
                Id = studente.Id,
                Nome = studente.Nome,
                Cognome = studente.Cognome,
                DataDiNascita = studente.DataDiNascita,
                Email = studente.Email
            };

            return PartialView("_UpdateForm", updateStudenteViewModel);
        }

        [Route("/studente/update/save")]
        public async Task<IActionResult> SaveUpdateStudente(UpdateStudenteViewModel updateStudenteViewModel) {
            var result = await _studenteService.UpdateStudenteAsync(updateStudenteViewModel);

            if (!result) {
                return Json(new {
                    success = false,
                    message = "Error while updating entity on database"
                });
            }

            return Json(new {
                success = true,
                message = "Update done successfully"
            });
            ;
        }

        [HttpPost]
        [Route("/studente/delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) {
            var result = await _studenteService.DeleteStudenteByIdAsync(id);

            if (!result) {
                return Json(new {
                    success = false,
                    message = "Error while deleting entity"
                });
            }

            string logmessage = "Entity deleted successfully";
            _loggerService.LogInformation(logmessage);

            return Json(new {
                success = true,
                message = logmessage
            });
        }

        [Route("/studente/add")]
        public IActionResult Add() {
            return PartialView("_AddForm");
        }

        [HttpPost]
        [Route("/studente/add")]
        public async Task<IActionResult> Add(AddStudenteViewModel addStudenteViewModel) {
            if (!ModelState.IsValid) {
                return Json(new {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }

            var result = await _studenteService.AddStudenteAsync(addStudenteViewModel);

            if (!result) {
                return Json(new {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }

            string logmessage = "Entity saved successfully to database";

            _loggerService.LogInformation(logmessage);
            return Json(new {
                success = true,
                message = logmessage
            });
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
