using System.ComponentModel.DataAnnotations;

namespace Esercizio_U5_S2_L1.ViewModels {
    public class LoginViewModel {

        [Display(Name = "Email (username)")]
        [Required(ErrorMessage = "Inserisci l'email")]
        [EmailAddress(ErrorMessage = "Inserisci un'email valida")]
        public required string Email {
            get; set;
        }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Inserisci la password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La password deve contenere almeno 8 caratteri.")]
        public required string Password {
            get; set;
        }
        [Display(Name = "Ricordami")]
        public bool RememberMe {
            get; set;
        }
    }
}
