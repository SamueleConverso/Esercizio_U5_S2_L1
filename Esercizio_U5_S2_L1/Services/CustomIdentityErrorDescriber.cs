using Microsoft.AspNetCore.Identity;

namespace Esercizio_U5_S2_L1.Services {
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber {
        public override IdentityError DuplicateUserName(string userName) {
            return new IdentityError {
                Code = nameof(DuplicateUserName),
                Description = $"L'indirizzo email '{userName}' esiste già."
            };
        }
    }
}