using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Homework19_ASPNET.Auth;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Homework19_ASPNET.Controllers.Api.Models;

namespace Homework19_ASPNET.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger log;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly HttpClient _httpClient;

        public AccountController(UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                ILoggerFactory Log)
        {
            this.log = Log.CreateLogger(">>> Мой Logger ");
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            log.LogWarning($" ------- \n >> Login(string returnUrl) сработал, returnUrl = {returnUrl}\n ------- \n ");

            return View(new UserLogin()
            {
                ReturnUrl = returnUrl
            });
        }

        //[HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(UserLogin model)
        {
            LoginUserRequest p = new LoginUserRequest(model.LoginProp, model.Password);
            var response = _httpClient.PostAsJsonAsync<LoginUserRequest>($"https://localhost:44393/api/login/", p).Result;
            //response.EnsureSuccessStatusCode();
            if (ModelState.IsValid)
            {
                ////получаем из формы email и пароль
                //var form = context.Request.Form;
                ////если email и / или пароль не установлены, посылаем статусный код ошибки 400
                //if (!form.ContainsKey("email") || !form.ContainsKey("password"))
                //    return Results.BadRequest("Email и/или пароль не установлены");
                //string email = form["email"];
                //string password = form["password"];

                ////находим пользователя
                //Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
                //если пользователь не найден, отправляем статусный код 401
                //if (person is null) return Results.Unauthorized();
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                //    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.Name)
                //};
                //var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //await context.SignInAsync(claimsPrincipal);
                //return Results.Redirect(returnUrl ?? "/");
            }

            ModelState.AddModelError("", "Пользователь не найден");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserRegistration());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.LoginProp };
                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Project");
                }
                else//иначе
                {
                    foreach (var identityError in createResult.Errors)
                    {
                        ModelState.AddModelError("", identityError.Description);
                    }
                }
            }

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Project");
        }

    }
}