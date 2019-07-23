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
            await m_userService.RegisterUser(userModel);
            return Content($"User {userModel.FirstName}" +
                $" {userModel.LastName} has been registered successfully");
        }
    }
}