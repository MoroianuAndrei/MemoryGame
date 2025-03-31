using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MemoryGame.Helpers;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                UpdateSelectedUserImage();
            }
        }

        private BitmapImage _selectedUserImage;
        public BitmapImage SelectedUserImage
        {
            get => _selectedUserImage;
            set
            {
                _selectedUserImage = value;
                OnPropertyChanged(nameof(SelectedUserImage));
            }
        }

        public ICommand NewUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand CancelCommand { get; }

        // Eveniment pentru a notifica necesitatea afișării CreateUserView
        public event EventHandler NewUserRequested;

        private readonly string _usersFilePath = "users.xml";

        public SignInViewModel()
        {
            // Inițializarea comenzilor
            NewUserCommand = new RelayCommand(_ => OnNewUserRequested());
            DeleteUserCommand = new RelayCommand(_ => DeleteSelectedUser(), _ => SelectedUser != null);
            PlayCommand = new RelayCommand(_ => StartGame(), _ => SelectedUser != null);
            CancelCommand = new RelayCommand(_ => Application.Current.Shutdown());

            // Încărcăm utilizatorii din fișier
            LoadUsers();
        }

        private void LoadUsers()
        {
            Users = new ObservableCollection<User>();

            if (File.Exists(_usersFilePath))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<User>));
                    using (FileStream fs = new FileStream(_usersFilePath, FileMode.Open))
                    {
                        Users = (ObservableCollection<User>)serializer.Deserialize(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la încărcarea utilizatorilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void SaveUsers()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<User>));
                using (FileStream fs = new FileStream(_usersFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, Users);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea utilizatorilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSelectedUserImage()
        {
            if (SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.ImagePath))
            {
                try
                {
                    SelectedUserImage = new BitmapImage(new Uri(SelectedUser.ImagePath, UriKind.Relative));
                }
                catch
                {
                    SelectedUserImage = null;
                }
            }
            else
            {
                SelectedUserImage = null;
            }
        }

        private void OnNewUserRequested()
        {
            // Declanșăm evenimentul pentru a notifica MainViewModel să afișeze CreateUserView
            NewUserRequested?.Invoke(this, EventArgs.Empty);
        }

        public void AddUser(User user)
        {
            if (user != null)
            {
                Users.Add(user);
                SelectedUser = user;
                SaveUsers();
            }
        }

        private void DeleteSelectedUser()
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show($"Ești sigur că vrei să ștergi utilizatorul '{SelectedUser.Username}'?",
                    "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Users.Remove(SelectedUser);
                    SaveUsers();
                    SelectedUser = Users.Count > 0 ? Users[0] : null;
                }
            }
        }

        private void StartGame()
        {
            if (SelectedUser != null)
            {
                // Aici se va implementa logica pentru a începe jocul
                MessageBox.Show($"Jocul începe pentru utilizatorul {SelectedUser.Username}!", "Start joc",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Aici se va adăuga codul pentru a deschide fereastra jocului
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}