using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class GameInvitationController : Controller
    {
        private IStringLocalizer<GameInvitationController> m_stringLocalizer;
        private IUserService m_userService;

        public GameInvitationController(IUserService userService,
            IStringLocalizer<GameInvitationController> stringLocalizer)
        {
            m_userService = userService;
            m_stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public IActionResult Index(string email)
        {
            var gameInvitationModel = new GameInvitationModel
            {
                InvitedBy = email
            };
            HttpContext.Session.SetString("email", email);
            return View(gameInvitationModel);
        }

        [HttpPost]
        public IActionResult Index(GameInvitationModel gameInvitationModel)
        {
            return Content(m_stringLocalizer["GameInvitationConfirmationMessage", 
                gameInvitationModel.EmailTo]);
        }
    }
}