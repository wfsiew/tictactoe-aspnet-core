﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class GameInvitationController : Controller
    {
        private IUserService m_userService;

        public GameInvitationController(IUserService userService)
        {
            m_userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            var gameInvitationModel = new GameInvitationModel
            {
                InvitedBy = email
            };
            return View(gameInvitationModel);
        }
    }
}