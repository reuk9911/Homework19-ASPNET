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
using System.Net.Http.Headers;

namespace Homework19_ASPNET.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger log;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private HttpClient _httpClient;

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
            if (returnUrl==null)
                returnUrl= $"/"; //https://localhost:44393
            log.LogWarning($" ------- \n >> Login(string returnUrl) сработал, returnUrl = {returnUrl}\n ------- \n ");

            return View(new UserLogin()
            {
                ReturnUrl = returnUrl
            });
        }

        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLogin model)
        {
            //response.EnsureSuccessStatusCode();
            if (ModelState.IsValid)
            {
                LoginUserRequest p = new LoginUserRequest(model.LoginProp, model.Password);
                var response = _httpClient.PostAsJsonAsync<LoginUserRequest>($"https://localhost:44393/api/login/", p).Result;
                
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", );

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                return Redirect(model.ReturnUrl ?? "/");
                
            }
            else
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