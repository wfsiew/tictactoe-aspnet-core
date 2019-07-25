using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IUserService m_userService;
        readonly IEmailService m_emailService;

        public UserRegistrationController(IUserService userService, IEmailService emailService)
        {
            m_userService = userService;
            m_emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await m_userService.RegisterUser(userModel);
                return RedirectToAction(nameof(EmailConfirmation), new { userModel.Email });
                //return Content($"User {userModel.FirstName}" +
                //    $" {userModel.LastName} has been registered successfully");
            }

            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string email)
        {
            var user = await m_userService.GetUserByEmail(email);
            var urlAction = new UrlActionContext
            {
                Action = "ConfirmEmail",
                Controller = "UserRegistration",
                Values = new { email },
                Protocol = Request.Scheme,
                Host = Request.Host.ToString()
            };

            var message = $"Thank you for your registration on our web site, please click here to confirm your email " + $"{Url.Action(urlAction)}";

            try
            {
                m_emailService.SendEmail(email,
                "Tic-Tac-Toe Email Confirmation", message).Wait();
            } 
            
            catch(Exception e)
            { }

            if (user?.IsEmailConfirmed == true)
                return RedirectToAction("Index", "GameInvitation", new { email = email });

            ViewBag.Email = email;
            return View();
        }
    }
}