﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class UserService : IUserService
    {
        private static ConcurrentBag<UserModel> m_userStore;

        static UserService()
        {
            m_userStore = new ConcurrentBag<UserModel>();
        }

        public Task<bool> RegisterUser(UserModel userModel)
        {
            m_userStore.Add(userModel);
            return Task.FromResult(true);
        }

        public Task<bool> IsOnline(string name)
        {
            return Task.FromResult(true);
        }

        public Task<UserModel> GetUserByEmail(string email)
        {
            return Task.FromResult(m_userStore.FirstOrDefault(u => u.Email == email));
        }

        public Task UpdateUser(UserModel userModel)
        {
            m_userStore = new ConcurrentBag<UserModel>(m_userStore.Where(u => u.Email != userModel.Email))
            {
                userModel
            };
            return Task.CompletedTask;
        }
    }
}
