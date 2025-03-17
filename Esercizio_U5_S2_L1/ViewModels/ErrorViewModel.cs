namespace Esercizio_U5_S2_L1.ViewModels {
    public class ErrorViewModel {
        public string? RequestId {
            get; set;
        }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
