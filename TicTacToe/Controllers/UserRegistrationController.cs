using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IUserService m_userService;

        public UserRegistrationController(IUserService userService)
        {
            m_userService = userService;
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
            if (user?.IsEmailConfirmed == true)
                return RedirectToAction("Index", "GameInvitation",
                new { email = email });
            ViewBag.Email = email;
            //user.IsEmailConfirmed = true;
            //user.EmailConfirmationDate = DateTime.Now;
            //await m_userService.UpdateUser(user);
            return View();
        }
    }
}