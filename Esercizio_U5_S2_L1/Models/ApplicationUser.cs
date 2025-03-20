using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Esercizio_U5_S2_L1.Models {
    public class ApplicationUser : IdentityUser {
        [Required]
        public required string FirstName {
            get; set;
        }

        [Required]
        public required string LastName {
            get; set;
        }

        [Required]
        public DateOnly BirthDate {
            get; set;
        }

        public ICollection<ApplicationUserRole> ApplicationUserRoles {
            get; set;
        }

        public ICollection<Studente> Studenti {
            get; set;
        }
    }
}
