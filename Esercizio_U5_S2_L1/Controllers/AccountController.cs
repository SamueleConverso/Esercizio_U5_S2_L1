using System.Security.Claims;
using System.Web;
using Esercizio_U5_S2_L1.Models;
using Esercizio_U5_S2_L1.Services;
using Esercizio_U5_S2_L1.ViewModels;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Esercizio_U5_S2_L1.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly EmailService _emailService;
        private readonly IFluentEmail _fluentEmail;
        private readonly GmailService _gmailService;
        private readonly SendGridService _sendGridService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, EmailService emailService, IFluentEmail fluentEmail, GmailService gmailService, SendGridService sendGridService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _fluentEmail = fluentEmail;
            _gmailService = gmailService;
            _sendGridService = sendGridService;
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {

            if (!ModelState.IsValid) {
                ModelState.AddModelError(string.Empty, "Compila correttamente i campi.");
                return View(model);
            }

            var user = new ApplicationUser {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                BirthDate = model.BirthDate
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            //if (!result.Succeeded) {
            //    return View();
            //}

            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var userForRole = await _userManager.FindByEmailAsync(user.Email);
            await _userManager.AddToRoleAsync(userForRole, "Studente");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);


            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new {
                userEmail = user.Email,
                token = encodedToken
            }, Request.Scheme);

            //var emailSent = await _emailService.SendEmail(user.FirstName, user.LastName, user.Email, confirmationLink);
            //var emailSent = await _gmailService.SendEmailAsync(user.FirstName, user.LastName, user.Email, confirmationLink);
            var emailSent = await _sendGridService.SendEmailAsync(user.FirstName, user.LastName, user.Email, confirmationLink);

            if (!emailSent) {
                ModelState.AddModelError(string.Empty, "Errore nell'invio dell'email di conferma.");
                return View(model);
            }

            return RedirectToAction(nameof(RegistrationConfirmation));
        }

        public async Task<IActionResult> ConfirmEmail(string userEmail, string token) {
            if (userEmail == null || token == null) {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
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

        [AllowAnonymous]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null) {
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);



            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            //var roles = await _signInManager.UserManager.GetRolesAsync(user);

            //foreach (var role in roles) {
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (!signInResult.Succeeded) {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

