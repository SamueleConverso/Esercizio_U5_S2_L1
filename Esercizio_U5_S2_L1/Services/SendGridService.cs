using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Esercizio_U5_S2_L1.Services {
    public class SendGridService {

        public async Task<bool> SendEmailAsync(string firstName, string lastName, string userEmail, string confirmationLink) {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("samu.converso@gmail.com", "Samuele Converso");
            var subject = "Conferma registrazione";
            var to = new EmailAddress(userEmail, "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = $"<p>Ciao {firstName} {lastName},</p>" +
                               $"<p>Per favore, conferma il tuo indirizzo email cliccando sul link: <a href='{confirmationLink}'>Conferma Email</a></p>" +
                               "<p>Grazie!</p>";
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

