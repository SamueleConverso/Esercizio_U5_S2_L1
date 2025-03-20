using Esercizio_U5_S2_L1.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Esercizio_U5_S2_L1.Services {
    public class GmailService {
        private readonly EmailSettings _emailSettings;

        public GmailService(IOptions<EmailSettings> emailSettings) {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string firstName, string lastName, string emailAddress, string confirmationLink) {
            try {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Il Team", _emailSettings.From));
                email.To.Add(new MailboxAddress($"{firstName} {lastName}", emailAddress));
                email.Subject = "Conferma la tua registrazione";

                var bodyBuilder = new BodyBuilder {
                    HtmlBody = $"<p>Ciao {firstName} {lastName},</p>" +
                               $"<p>Per favore, conferma il tuo indirizzo email cliccando sul link: <a href='{confirmationLink}'>Conferma Email</a></p>" +
                               "<p>Grazie!</p>"
                };

                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();

                // Disabilita la verifica del certificato SSL (da rimuovere in produzione)
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; //Da togliere in produzione
                //Da togliere in produzione

                //smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => {
                //    if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                //        return true;

                //    Console.WriteLine($"Errore di validazione SSL: {sslPolicyErrors}");

                //    if (chain != null) {
                //        foreach (var status in chain.ChainStatus) {
                //            Console.WriteLine($"Status: {status.StatusInformation}, Error: {status.Status}");
                //        }
                //    }

                //    return false;
                //};

                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            } catch (Exception ex) {
                // Log dell'errore (implementa un logger appropriato)
                Console.WriteLine($"Errore nell'invio dell'email: {ex.Message}");
                return false;
            }
        }
    }
}