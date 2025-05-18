﻿using Printinvest_WPF_app.Models;

namespace Printinvest_WPF_app
{
    public static class SessionManager
    {
        public static User CurrentUser { get; private set; }

        public static void Login(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsAuthenticated => CurrentUser != null;

        public static bool IsAdmin => CurrentUser?.Role == UserRole.Admin;
        public static bool IsManager => CurrentUser?.Role == UserRole.Manager;
    }
}