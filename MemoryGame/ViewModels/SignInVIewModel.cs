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
using MemoryGame.Views;

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

        // Eveniment pentru a notifica necesitatea afișării GameView
        public event EventHandler<User> GameRequested;

        // Directory for user files
        private readonly string _usersDirectory = "Users";

        // File that stores the list of usernames
        private readonly string _userListFilePath = "userlist.xml";

        public SignInViewModel()
        {
            // Inițializarea comenzilor
            NewUserCommand = new RelayCommand(_ => OnNewUserRequested());
            DeleteUserCommand = new RelayCommand(_ => DeleteSelectedUser(), _ => SelectedUser != null);
            PlayCommand = new RelayCommand(_ => StartGame(), _ => SelectedUser != null);
            CancelCommand = new RelayCommand(_ => Application.Current.Shutdown());

            // Make sure the users directory exists
            if (!Directory.Exists(_usersDirectory))
            {
                Directory.CreateDirectory(_usersDirectory);
            }

            // Încărcăm utilizatorii
            LoadUsers();
        }

        private void LoadUsers()
        {
            Users = new ObservableCollection<User>();

            // First check if we have a list of usernames
            if (File.Exists(_userListFilePath))
            {
                try
                {
                    // Load the list of usernames
                    XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                    List<string> usernames;

                    using (FileStream fs = new FileStream(_userListFilePath, FileMode.Open))
                    {
                        usernames = (List<string>)serializer.Deserialize(fs);
                    }

                    // Load each user from their individual file
                    foreach (string username in usernames)
                    {
                        string userFilePath = Path.Combine(_usersDirectory, $"{username}.xml");
                        if (File.Exists(userFilePath))
                        {
                            try
                            {
                                XmlSerializer userSerializer = new XmlSerializer(typeof(UserData));
                                using (FileStream fs = new FileStream(userFilePath, FileMode.Open))
                                {
                                    UserData userData = (UserData)userSerializer.Deserialize(fs);
                                    Users.Add(userData.User);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error loading user {username}: {ex.Message}", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading user list: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveUserList()
        {
            try
            {
                // Extract all usernames
                List<string> usernames = Users.Select(u => u.Username).ToList();

                // Save the list of usernames
                XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                using (FileStream fs = new FileStream(_userListFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, usernames);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user list: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                // Check if the username already exists
                if (Users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"A user with the name '{user.Username}' already exists. Please choose a different name.",
                        "Duplicate Username", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Add user to collection
                Users.Add(user);
                SelectedUser = user;

                // Save user to individual file
                SaveUser(user);

                // Update the user list
                SaveUserList();
            }
        }

        private void SaveUser(User user)
        {
            try
            {
                // Create a user data object to store user and potentially saved game
                UserData userData = new UserData
                {
                    User = user,
                    SavedGame = null // No saved game yet
                };

                // Save to user's individual XML file
                string userFilePath = Path.Combine(_usersDirectory, $"{user.Username}.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(UserData));

                using (FileStream fs = new FileStream(userFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, userData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user {user.Username}: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSelectedUser()
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the user '{SelectedUser.Username}'?",
                    "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Delete the user's XML file
                    string userFilePath = Path.Combine(_usersDirectory, $"{SelectedUser.Username}.xml");
                    if (File.Exists(userFilePath))
                    {
                        try
                        {
                            File.Delete(userFilePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting user file: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Remove from collection
                    Users.Remove(SelectedUser);

                    // Update the user list
                    SaveUserList();

                    // Select another user if available
                    SelectedUser = Users.Count > 0 ? Users[0] : null;
                }
            }
        }

        private void StartGame()
        {
            if (SelectedUser != null)
            {
                // Declanșăm evenimentul pentru a notifica MainViewModel să afișeze GameView
                GameRequested?.Invoke(this, SelectedUser);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // New class to store both user data and saved game in the same file
    [Serializable]
    public class UserData
    {
        public User User { get; set; }
        public SavedGame SavedGame { get; set; }
    }
}