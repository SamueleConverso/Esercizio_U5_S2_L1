using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esercizio_U5_S2_L1.Models {
    [Table("Studenti")]
    public class Studente {
        [Key]
        public Guid Id {
            get; set;
        }

        [Required]
        public string Nome {
            get; set;
        }

        [Required]
        public string Cognome {
            get; set;
        }

        [Required]
        public DateOnly DataDiNascita {
            get; set;
        }

        [Required]
        public string Email {
            get; set;
        }

        public DateTime CreatedAt {
            get; set;
        }

        public string ApplicationUserId {
            get; set;
        }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser {
            get; set;
        }
    }
}
