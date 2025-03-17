using Esercizio_U5_S2_L1.ViewModels;
using Esercizio_U5_S2_L1.Data;
using Microsoft.EntityFrameworkCore;
using Esercizio_U5_S2_L1.Models;

namespace Esercizio_U5_S2_L1.Services {
    public class StudenteService {
        private readonly AppDbContext _context;

        public StudenteService(AppDbContext context) {
            this._context = context;
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


    }
}
