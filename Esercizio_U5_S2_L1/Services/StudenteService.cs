using Esercizio_U5_S2_L1.ViewModels;
using Esercizio_U5_S2_L1.Data;
using Microsoft.EntityFrameworkCore;
using Esercizio_U5_S2_L1.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Esercizio_U5_S2_L1.Services {
    public class StudenteService {
        private readonly AppDbContext _context;
        private readonly LoggerService _loggerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudenteService(AppDbContext context, LoggerService loggerService, UserManager<ApplicationUser> userManager) {
            this._context = context;
            _loggerService = loggerService;
            _userManager = userManager;
        }

        private async Task<bool> SaveAsync() {
            try {
                var rows = await _context.SaveChangesAsync();

                if (rows > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<StudentiViewModel> GetAllStudentiAsync() {
            var studenti = new StudentiViewModel();

            try {
                studenti.Studenti = await _context.Studenti.Include(s => s.ApplicationUser).ToListAsync();
            } catch (Exception ex) {
                studenti.Studenti = null;
                Console.WriteLine(ex.Message);
                throw;
            }

            return studenti;
        }

        public async Task<Studente> GetStudenteByIdAsync(Guid id) {
            var studente = await _context.Studenti.FindAsync(id);

            if (studente == null) {
                return null;
            }

            return studente;
        }

        public async Task<bool> UpdateStudenteAsync(UpdateStudenteViewModel updateStudenteViewModel) {
            try {
                var studente = await _context.Studenti.FindAsync(updateStudenteViewModel.Id);

                if (studente == null) {
                    return false;
                }

                studente.Nome = updateStudenteViewModel.Nome;
                studente.Cognome = updateStudenteViewModel.Cognome;
                studente.DataDiNascita = updateStudenteViewModel.DataDiNascita;
                studente.Email = updateStudenteViewModel.Email;

                return await SaveAsync();
            } catch (Exception ex) {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteStudenteByIdAsync(Guid id) {
            try {
                var studente = await _context.Studenti.FindAsync(id);

                if (studente == null) {
                    _loggerService.LogWarning($"Product not found");
                    return false;
                }

                _context.Studenti.Remove(studente);

                return await SaveAsync();
            } catch (Exception ex) {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddStudenteAsync(AddStudenteViewModel addStudenteViewModel, ClaimsPrincipal claimsPrincipal) {
            var applicationUser = await _userManager.FindByEmailAsync(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            try {
                var studente = new Studente() {
                    Id = Guid.NewGuid(),
                    Nome = addStudenteViewModel.Nome,
                    Cognome = addStudenteViewModel.Cognome,
                    DataDiNascita = addStudenteViewModel.DataDiNascita,
                    Email = addStudenteViewModel.Email,
                    ApplicationUserId = applicationUser.Id,
                };

                _context.Studenti.Add(studente);

                return await SaveAsync();
            } catch (Exception ex) {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

    }
}
