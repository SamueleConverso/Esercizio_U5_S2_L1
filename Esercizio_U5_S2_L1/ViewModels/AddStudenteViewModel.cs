using System.ComponentModel.DataAnnotations;

namespace Esercizio_U5_S2_L1.ViewModels {
    public class AddStudenteViewModel {
        [Required]
        [StringLength(20)]
        public required string Nome {
            get; set;
        }

        [Required]
        [StringLength(20)]
        public required string Cognome {
            get; set;
        }

        [Required]
        public DateOnly DataDiNascita {
            get; set;
        }

        [Required]
        public required string Email {
            get; set;
        }
    }
}
