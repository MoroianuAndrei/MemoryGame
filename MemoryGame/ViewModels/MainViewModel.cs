﻿using System.ComponentModel;
using System.Windows.Controls;
using MemoryGame.Models;
using MemoryGame.Views;
using MemoryGame.ViewModels;

namespace MemoryGame.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private SignInViewModel _signInViewModel;
        private CreateUserViewModel _createUserViewModel;
        private GameViewModel _gameViewModel;

        public MainViewModel()
        {
            // Inițializăm SignInViewModel și setăm CurrentView la SignInView
            _signInViewModel = new SignInViewModel();
            _signInViewModel.NewUserRequested += OnNewUserRequested;
            _signInViewModel.GameRequested += OnGameRequested;

            // Afișăm SignInView la început
            CurrentView = new SignInView() { DataContext = _signInViewModel };
        }

        private void OnNewUserRequested(object sender, System.EventArgs e)
        {
            // Creăm și afișăm CreateUserView
            _createUserViewModel = new CreateUserViewModel();
            _createUserViewModel.RequestClose += OnCreateUserClosed;

            CurrentView = new CreateUserView() { DataContext = _createUserViewModel };
        }

        private void OnGameRequested(object sender, User user)
        {
            // Creăm și afișăm GameView
            _gameViewModel = new GameViewModel { CurrentPlayer = user };
            _gameViewModel.ReturnToSignIn += OnReturnToSignIn;

            CurrentView = new GameView() { DataContext = _gameViewModel };
        }

        private void OnCreateUserClosed(object sender, User user)
        {
            // Revenim la SignInView și adăugăm noul utilizator dacă există
            if (user != null)
            {
                _signInViewModel.AddUser(user);
            }

            CurrentView = new SignInView() { DataContext = _signInViewModel };
        }

        private void OnReturnToSignIn(object sender, System.EventArgs e)
        {
            // Revenim la SignInView
            CurrentView = new SignInView() { DataContext = _signInViewModel };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}