using System.Web;
using Esercizio_U5_S2_L1.Models;
using Esercizio_U5_S2_L1.Services;
using Esercizio_U5_S2_L1.ViewModels;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_U5_S2_L1.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly EmailService _emailService;
        private readonly IFluentEmail _fluentEmail;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, EmailService emailService, IFluentEmail fluentEmail) {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _fluentEmail = fluentEmail;
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {

            var user = new ApplicationUser {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                BirthDate = model.BirthDate
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                return View();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);


            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new {
                userId = user.Id,
                token = encodedToken
            }, Request.Scheme);

            await _emailService.SendEmail(user.FirstName, user.LastName, user.Email, confirmationLink);

            return RedirectToAction(nameof(RegistrationConfirmation));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token) {
            if (userId == null || token == null) {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return RedirectToAction("Index", "Home");
            }

            var decodedToken = HttpUtility.UrlDecode(token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded) {
                return View("ConfirmEmail");
            } else {
                return View("Error");
            }
        }

        public IActionResult RegistrationConfirmation() {
            return View();
        }

    }
}

