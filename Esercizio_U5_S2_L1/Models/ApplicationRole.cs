using Microsoft.AspNetCore.Identity;

namespace Esercizio_U5_S2_L1.Models {
    public class ApplicationRole : IdentityRole {
        public ICollection<ApplicationUserRole> ApplicationUserRole {
            get; set;
        }
    }
}
