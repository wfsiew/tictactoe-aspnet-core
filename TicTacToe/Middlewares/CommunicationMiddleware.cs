using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Services;

namespace TicTacToe.Middlewares
{
    public class CommunicationMiddleware
    {
        private readonly RequestDelegate m_next;
        private readonly IUserService m_userService;

        public CommunicationMiddleware(RequestDelegate next, IUserService userService)
        {
            m_next = next;
            m_userService = userService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await m_next.Invoke(httpContext);
            return;
        }
    }
}
