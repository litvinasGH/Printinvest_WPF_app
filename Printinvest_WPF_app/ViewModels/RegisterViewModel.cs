﻿using Printinvest_WPF_app.Repositories;
using Printinvest_WPF_app.Utilities;
using Printinvest_WPF_app.Views.Pages;
using System;
using System.Windows;
using System.Windows.Input;

namespace Printinvest_WPF_app.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly UserRepository _userRepository;
        private string _login;
        private string _password;
        private string _confirmPassword;
        private string _name;
        private string _errorMessage;

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel()
        {
            _userRepository = RepositoryManager.Users;
            RegisterCommand = new RelayCommand(RegisterExecute, CanRegisterExecute);
            NavigateToLoginCommand = new RelayCommand(() => Navigate("Login"));
        }

        private bool CanRegisterExecute()
        {
            return !string.IsNullOrWhiteSpace(Login) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   !string.IsNullOrWhiteSpace(Name);
        }

        private void RegisterExecute()
        {
            try
            {
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = Application.Current.Resources["ErrorPasswordsDoNotMatch"]?.ToString() ?? "Пароли не совпадают.";
                    return;
                }
                if (Password.Length < 6)
                {
                    ErrorMessage = Application.Current.Resources["ErrorPasswordTooShort"]?.ToString() ?? "Пароль слишком короткий. Минимум 6 символов.";
                    return;
                }
                var existingUser = _userRepository.GetByLogin(Login);
                if (existingUser != null)
                {
                    ErrorMessage = Application.Current.Resources["ErrorUserExists"]?.ToString() ?? "Пользователь с таким логином уже существует.";
                    return;
                }

                var newUser = new Models.User
                {
                    Login = Login,
                    HashPassword = HashHelper.HashPassword(Password),
                    Name = Name,
                    Role = Models.UserRole.Client
                };

                _userRepository.Add(newUser);
                SessionManager.Login(newUser);
                Navigate("Profile");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        private void Navigate(string page)
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow?.DataContext is MainViewModel mainViewModel)
            {
                if (page == "Login")
                {
                    mainViewModel.CurrentPage = new LoginPage();
                }
                else
                {
                    mainViewModel.CurrentPage = new ProfilePage();
                }
            }
        }
    }
}