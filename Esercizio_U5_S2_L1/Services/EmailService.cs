﻿using Esercizio_U5_S2_L1.ViewModels;
using FluentEmail.Core;

namespace Esercizio_U5_S2_L1.Services {
    public class EmailService {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail) {
            _fluentEmail = fluentEmail;
        }

        public async Task<bool> SendEmail(string username, string lastName, string email, string confirmationLink) {
            var emailViewModel = new ConfirmationEmailViewModel() {
                FirstName = username,
                LastName = lastName,
                ConfirmationLink = confirmationLink
            };

            var res = await _fluentEmail.To(email).Subject("Conferma registrazione")
                .UsingTemplateFromFile("Views/Templates/ConfirmationEmail.cshtml", emailViewModel)
                .SendAsync();

            return res.Successful;
        }
    }
}