using Microsoft.EntityFrameworkCore;
using Esercizio_U5_S2_L1.Models;

namespace Esercizio_U5_S2_L1.Data {
    public class AppDbContext : DbContext {
        public AppDbContext() {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        public DbSet<Studente> Studenti {
            get; set;
        }
    }
}
