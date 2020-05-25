using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ClinicaVet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ClinicaVet.Areas.Identity.Pages.Account {
   [AllowAnonymous]
   public class RegisterModel : PageModel {
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly ILogger<RegisterModel> _logger;
      private readonly IEmailSender _emailSender;

      public RegisterModel(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender) {
         _userManager = userManager;
         _signInManager = signInManager;
         _logger = logger;
         _emailSender = emailSender;
      }

      [BindProperty]
      public InputModel Input { get; set; }

      public string ReturnUrl { get; set; }

      public IList<AuthenticationScheme> ExternalLogins { get; set; }

      public class InputModel {
         [Required]
         [EmailAddress]
         [Display(Name = "Email")]
         public string Email { get; set; }

         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "Password")]
         public string Password { get; set; }

         [DataType(DataType.Password)]
         [Display(Name = "Confirm password")]
         [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
         public string ConfirmPassword { get; set; }

         // **********************************************

         [Required(ErrorMessage = "O Nome é de preenchimento obrigatório")]
         [StringLength(40, ErrorMessage = "O {0} só pode ter, no máximo, {1} carateres.")]
         [RegularExpression("[A-ZÁÍÓÚÉÂ][a-zãõáéíóúàèìòùäëïöüçâêîôû]+" +
         "(( | e |-|'| d'| de | d[ao](s)? )[A-ZÁÍÓÚÉÂ][a-zãõáéíóúàèìòùäëïöüçâêîôû]+){1,3}",
            ErrorMessage = "Só são aceites letras. Cada palavra deve começar por uma Maiúscula, separadas por um espaço em branco.")]
         public string Nome { get; set; }

      }


      // o acesso a este método faz-se qd o FORM entrega os dados em HTTP GET
      public async Task OnGetAsync(string returnUrl = null) {
         ReturnUrl = returnUrl;
         ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
      }


      // o acesso a este método faz-se qd o FORM entrega os dados em HTTP POST
      public async Task<IActionResult> OnPostAsync(string returnUrl = null) {

         returnUrl = returnUrl ?? Url.Content("~/");
         //  ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

         // este ModelState está a validar os dados do InputModel
         if (ModelState.IsValid) {

            // adicionar o código para processar o ficheiro com a imagem do User

            var user = new ApplicationUser {
               UserName = Input.Email,
               Email = Input.Email,
               Nome = Input.Nome,
               Fotografia = "", // para a Fotografia será necessário executar uma ação semelhante ao que fizemos no Create do Veterinário
               Timestamp = DateTime.Now,
            };

            // estamos aqui, realmente, a tentar criar o utilizador
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded) {
               // houve sucesso na criação do utilizador
               _logger.LogInformation("User created a new account with password.");

               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
               var callbackUrl = Url.Page(
                   "/Account/ConfirmEmail",
                   pageHandler: null,
                   values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                   protocol: Request.Scheme);

               await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

               if (_userManager.Options.SignIn.RequireConfirmedAccount) {
                  return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
               }
               else {
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return LocalRedirect(returnUrl);
               }
            }
            foreach (var error in result.Errors) {
               ModelState.AddModelError(string.Empty, error.Description);
            }
         }

         // If we got this far, something failed, redisplay form
         return Page();
      }
   }
}
