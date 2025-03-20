using System.ComponentModel.DataAnnotations;

namespace Esercizio_U5_S2_L1.ViewModels {
    public class RegisterViewModel {
        [Required]
        public Guid Id {
            get; set;
        }

        [Display(Name = "Email (sarà il tuo username)")]
        [Required(ErrorMessage = "L'indirizzo email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
        public required string Email {
            get; set;
        }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        public required string FirstName {
            get; set;
        }

        [Display(Name = "Cognome")]
        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        public required string LastName {
            get; set;
        }

        [Display(Name = "Data di nascita")]
        [Required(ErrorMessage = "La data di nascita è obbligatoria.")]
        [DataType(DataType.Date)]
        public required DateOnly BirthDate {
            get; set;
        }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "La password è obbligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La password deve contenere almeno 8 caratteri.")]
        public required string Password {
            get; set;
        }

        [Display(Name = "Conferma password")]
        [Required(ErrorMessage = "Conferma la tua password.")]
        [Compare(nameof(Password), ErrorMessage = "Le password non corrispondono.")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword {
            get; set;
        }
    }
}
