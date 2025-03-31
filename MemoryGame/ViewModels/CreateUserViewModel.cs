using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MemoryGame.Helpers;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public class CreateUserViewModel : INotifyPropertyChanged
    {
        // Colecția de căi ale imaginilor
        public ObservableCollection<string> ImagePaths { get; } = new ObservableCollection<string>
        {
            "/images/user_icon/jerry.png",
            "/images/user_icon/tom.png",
            "/images/user_icon/johnny.png",
            "/images/user_icon/bugs_bunny.png",
            "/images/user_icon/mickey_mouse.png",
            "/images/user_icon/minnie_mouse.png",
            "/images/user_icon/goofy.png",
            "/images/user_icon/scooby_doo.png",
            "/images/user_icon/stitch.png",
            "/images/user_icon/garfield.png",
            "/images/user_icon/gumball.png",
            "/images/user_icon/darwin.png",
            "/images/user_icon/spongebob.png",
            "/images/user_icon/patrick.png",
            "/images/user_icon/finn.png",
            "/images/user_icon/jake.png"
        };

        private int _currentImageIndex;
        public int CurrentImageIndex
        {
            get => _currentImageIndex;
            set
            {
                if (_currentImageIndex != value)
                {
                    _currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                    UpdateImage();
                }
            }
        }

        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                ValidateInput();
            }
        }

        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            set
            {
                _canSave = value;
                OnPropertyChanged(nameof(CanSave));
            }
        }

        // Comenzile
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Eveniment pentru a notifica închiderea view-ului
        public event EventHandler<User> RequestClose;

        public CreateUserViewModel()
        {
            // Inițializare stare
            _currentImageIndex = 0;
            _username = string.Empty;
            _canSave = false;
            UpdateImage();

            // Inițializare comenzi
            NextCommand = new RelayCommand(_ => NextImage());
            PreviousCommand = new RelayCommand(_ => PreviousImage());
            SaveCommand = new RelayCommand(_ => SaveUser(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void UpdateImage()
        {
            try
            {
                CurrentImage = new BitmapImage(new Uri(ImagePaths[CurrentImageIndex], UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea imaginii: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NextImage()
        {
            if (CurrentImageIndex < ImagePaths.Count - 1)
                CurrentImageIndex++;
            else
                CurrentImageIndex = 0;
        }

        private void PreviousImage()
        {
            if (CurrentImageIndex > 0)
                CurrentImageIndex--;
            else
                CurrentImageIndex = ImagePaths.Count - 1;
        }

        private void ValidateInput()
        {
            CanSave = !string.IsNullOrWhiteSpace(Username);
        }

        private void SaveUser()
        {
            if (CanSave)
            {
                var user = new User
                {
                    Username = Username.Trim(),
                    ImagePath = ImagePaths[CurrentImageIndex]
                };

                RequestClose?.Invoke(this, user);
            }
        }

        private void Cancel()
        {
            // Închidem fără a salva niciun utilizator
            RequestClose?.Invoke(this, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}