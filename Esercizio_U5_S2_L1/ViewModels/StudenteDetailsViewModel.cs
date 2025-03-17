namespace Esercizio_U5_S2_L1.ViewModels {
    public class StudenteDetailsViewModel {
        public Guid Id {
            get; set;
        }
        public string Nome {
            get; set;
        }

        public string Cognome {
            get; set;
        }

        public DateOnly DataDiNascita {
            get; set;
        }

        public string Email {
            get; set;
        }
    }
}
