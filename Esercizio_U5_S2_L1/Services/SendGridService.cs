using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Esercizio_U5_S2_L1.Services {
    public class SendGridService {
    }

    public async Task<bool> SendEmailAsync(string userEmail) {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("samu.converso@gmail.com", "Samuele Converso");
            var subject = "Conferma registrazione";
            var to = new EmailAddress(userEmail, "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode) {
                return true;
            } else {
                return false;
            }
        }
    }
}
