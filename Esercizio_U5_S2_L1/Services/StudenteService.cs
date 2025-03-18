﻿using Esercizio_U5_S2_L1.ViewModels;
using Esercizio_U5_S2_L1.Data;
using Microsoft.EntityFrameworkCore;
using Esercizio_U5_S2_L1.Models;

namespace Esercizio_U5_S2_L1.Services {
    public class StudenteService {
        private readonly AppDbContext _context;

        public StudenteService(AppDbContext context) {
            this._context = context;
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
                //_loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<StudentiViewModel> GetAllStudentiAsync() {
            var studenti = new StudentiViewModel();

            try {
                studenti.Studenti = await _context.Studenti.ToListAsync();
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
                //_loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteStudenteByIdAsync(Guid id) {
            try {
                var studente = await _context.Studenti.FindAsync(id);

                if (studente == null) {
                    //_loggerService.LogWarning($"Product not found");
                    return false;
                }

                _context.Studenti.Remove(studente);

                return await SaveAsync();
            } catch (Exception ex) {
                //_loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddStudenteAsync(AddStudenteViewModel addStudenteViewModel) {
            try {
                var studente = new Studente() {
                    Id = Guid.NewGuid(),
                    Nome = addStudenteViewModel.Nome,
                    Cognome = addStudenteViewModel.Cognome,
                    DataDiNascita = addStudenteViewModel.DataDiNascita,
                    Email = addStudenteViewModel.Email
                };

                _context.Studenti.Add(studente);

                return await SaveAsync();
            } catch (Exception ex) {
                //_loggerService.LogError(ex.Message);
                return false;
            }
        }

    }
}
