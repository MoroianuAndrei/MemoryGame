using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Serialization;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Views;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private User _currentPlayer;
        public User CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        private int _currentScore;
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnPropertyChanged(nameof(CurrentScore));
            }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }

        private ObservableCollection<Card> _cards;
        public ObservableCollection<Card> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }

        private int _boardRows = 4;
        public int BoardRows
        {
            get => _boardRows;
            set
            {
                _boardRows = value;
                OnPropertyChanged(nameof(BoardRows));
            }
        }

        private int _boardColumns = 4;
        public int BoardColumns
        {
            get => _boardColumns;
            set
            {
                _boardColumns = value;
                OnPropertyChanged(nameof(BoardColumns));
            }
        }

        private int _selectedCategory = 1;
        private int _selectedBackground = 1;
        private DateTime _gameStartTime;
        private DispatcherTimer _gameTimer;
        private Card _firstFlippedCard;
        private Card _secondFlippedCard;
        private bool _isProcessingTurn;
        private int _secondsRemaining = 0; // Countdown timer starts at 30 seconds
        private int _moves;
        private int _pendingBoardRows = 4;
        private int _pendingBoardColumns = 4;

        public ICommand SelectCategoryCommand { get; }
        public ICommand SelectBackgroundCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand StatisticsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SetBoardSizeCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand FlipCardCommand { get; }

        public event EventHandler<EventArgs> ReturnToSignIn;

        public GameViewModel()
        {
            // Inițializăm comenzile
            SelectCategoryCommand = new RelayCommand(param => SelectCategory(int.Parse((string)param)));
            SelectBackgroundCommand = new RelayCommand(param => SelectBackground(int.Parse((string)param)));
            NewGameCommand = new RelayCommand(_ => StartNewGame());
            OpenGameCommand = new RelayCommand(_ => OpenSavedGame());
            SaveGameCommand = new RelayCommand(_ => SaveGame(), _ => Cards != null && Cards.Count > 0);
            StatisticsCommand = new RelayCommand(_ => ShowStatistics());
            ExitCommand = new RelayCommand(_ => Exit());
            SetBoardSizeCommand = new RelayCommand(param => SetBoardSize((string)param));
            AboutCommand = new RelayCommand(_ => ShowAboutDialog());
            FlipCardCommand = new RelayCommand(param => FlipCard((Card)param), param => CanFlipCard((Card)param));

            // Inițializăm timer-ul
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _gameTimer.Tick += GameTimer_Tick;

            // Valor inițiale
            ElapsedTime = "00:00";
            CurrentScore = 0;
        }

        // We also need to update the GameTimer_Tick method to handle variable start times
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _secondsRemaining--;

            // Format the time as mm:ss
            int minutes = _secondsRemaining / 60;
            int seconds = _secondsRemaining % 60;
            ElapsedTime = $"{minutes:D2}:{seconds:D2}";

            // Check if time's up
            if (_secondsRemaining <= 0)
            {
                _gameTimer.Stop();
                MessageBox.Show("Time's up! Game over.", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);

                // Display final score
                MessageBox.Show($"Your final score: {CurrentScore}\nMoves: {_moves}",
                    "Game Results", MessageBoxButton.OK, MessageBoxImage.Information);

                // Save game statistics - use the total time that was allocated
                int totalCards = BoardRows * BoardColumns;
                int totalTime = totalCards * 2;
                totalTime = (int)Math.Ceiling(totalTime / 10.0) * 10;
                SaveGameStatistics(TimeSpan.FromSeconds(totalTime));

                // Reset game state after time's up
                ResetGameState();
            }
        }

        private string CategoryOnCards(int id)
        {
            switch(id)
            {
                case 1:
                    return "Cartoons";
                    break;
                case 2:
                    return "Flowers";
                    break;
                case 3:
                    return "Animals";
                    break;
                default:
                    return "";
                    break;
            }
        }

        private void SelectCategory(int categoryId)
        {
            _selectedCategory = categoryId;
            MessageBox.Show($"Selected {CategoryOnCards(categoryId)} Category", "Category", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string BackgroundOnCards(int id)
        {
            switch(id)
            {
                case 1:
                    return "Blue";
                    break;
                case 2:
                    return "Pink";
                    break;
                case 3:
                    return "Purple";
                    break;
                case 4:
                    return "Yellow";
                    break;
                case 5:
                    return "Green";
                    break;
                case 6:
                    return "Orange";
                    break;
                case 7:
                    return "Red";
                    break;
                default:
                    return "";
                    break;
            }
        }

        private void SelectBackground(int backgroundId)
        {
            _selectedBackground = backgroundId;
            MessageBox.Show($"Selected {BackgroundOnCards(backgroundId)} Background", "Background", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StartNewGame()
        {
            // Oprești timer-ul jocului curent
            _gameTimer.Stop();

            // Actualizează dimensiunile tablei de joc cu setările din așteptare
            BoardRows = _pendingBoardRows;
            BoardColumns = _pendingBoardColumns;

            // Calculează timpul de start, resetează scorul și creează o nouă tablă
            int totalCards = BoardRows * BoardColumns;
            _secondsRemaining = totalCards * 2;
            _secondsRemaining = (int)Math.Ceiling(_secondsRemaining / 10.0) * 10;

            int minutes = _secondsRemaining / 60;
            int seconds = _secondsRemaining % 60;
            ElapsedTime = $"{minutes:D2}:{seconds:D2}";

            CurrentScore = 0;
            _moves = 0;
            _firstFlippedCard = null;
            _secondFlippedCard = null;
            _isProcessingTurn = false;

            CreateGameBoard();

            _gameStartTime = DateTime.Now;
            _gameTimer.Start();
        }

        private void CreateGameBoard()
        {
            Cards = new ObservableCollection<Card>();

            // Pentru simulare, vom crea cărți cu imagini de același tip
            int totalCards = BoardRows * BoardColumns;
            int pairsCount = totalCards / 2;

            // Asigură-te că numărul de cărți este par
            if (totalCards % 2 != 0)
            {
                MessageBox.Show("The number of cards must be even!", "Invalid Configuration",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lista de id-uri pentru perechi
            List<int> imageIds = new List<int>();
            for (int i = 1; i <= pairsCount; i++)
            {
                imageIds.Add(i);
                imageIds.Add(i);
            }

            // Amestecăm id-urile
            Random random = new Random();
            imageIds = imageIds.OrderBy(x => random.Next()).ToList();

            // Creăm cărțile cu imaginile corespunzătoare
            // Modificare: folosim fundalul selectat
            BitmapImage backImage = new BitmapImage(new Uri($"pack://application:,,,/Images/Game/Background{_selectedBackground}.png", UriKind.Absolute));

            for (int i = 0; i < totalCards; i++)
            {
                // Încarcă imaginea frontală bazată pe categoria selectată
                BitmapImage frontImage = new BitmapImage(
                    new Uri($"pack://application:,,,/Images/Game/Category{_selectedCategory}/Photo{imageIds[i]}.png", UriKind.Absolute));

                Cards.Add(new Card(i, frontImage, backImage));
            }
        }

        private void SaveGame()
        {
            try
            {
                // Create a SavedGame object to store the current game state
                var savedGame = new SavedGame
                {
                    Username = CurrentPlayer.Username,
                    SaveDate = DateTime.Now,
                    BoardRows = BoardRows,
                    BoardColumns = BoardColumns,
                    Category = _selectedCategory,
                    Background = _selectedBackground,
                    Score = CurrentScore,
                    Moves = _moves
                };

                // Calculate elapsed time from the remaining seconds
                int totalCards = BoardRows * BoardColumns;
                int totalTime = totalCards * 2;
                totalTime = (int)Math.Ceiling(totalTime / 10.0) * 10;
                savedGame.ElapsedTime = TimeSpan.FromSeconds(totalTime - _secondsRemaining);

                // Save the state of each card
                savedGame.CardStates = new List<CardState>();
                foreach (var card in Cards)
                {
                    // Extract the image ID from the front image URI
                    string frontImagePath = card.FrontImage.ToString();
                    int imageId = 0;

                    // Parse the image ID from the image path
                    string[] parts = frontImagePath.Split('/');
                    string fileName = parts[parts.Length - 1];
                    string fileNameWithoutExtension = fileName.Split('.')[0];
                    if (fileNameWithoutExtension.StartsWith("Photo"))
                    {
                        int.TryParse(fileNameWithoutExtension.Substring(5), out imageId);
                    }

                    savedGame.CardStates.Add(new CardState
                    {
                        Id = card.Id,
                        ImageId = imageId,
                        IsFlipped = card.IsFlipped,
                        IsMatched = card.IsMatched
                    });
                }

                // Path to the user's XML file
                string userDirectory = "Users";
                string userFilePath = Path.Combine(userDirectory, $"{CurrentPlayer.Username}.xml");

                if (File.Exists(userFilePath))
                {
                    try
                    {
                        // Load the existing user data
                        XmlSerializer serializer = new XmlSerializer(typeof(UserData));
                        UserData userData;

                        using (FileStream fs = new FileStream(userFilePath, FileMode.Open))
                        {
                            userData = (UserData)serializer.Deserialize(fs);
                        }

                        // Update the saved game
                        userData.SavedGame = savedGame;

                        // Save the updated user data
                        using (FileStream fs = new FileStream(userFilePath, FileMode.Create))
                        {
                            serializer.Serialize(fs, userData);
                        }

                        MessageBox.Show($"Game saved successfully for {CurrentPlayer.Username}!", "Game Saved",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving game: {ex.Message}", "Save Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"User file for {CurrentPlayer.Username} not found.", "Save Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing game save: {ex.Message}", "Save Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSavedGame()
        {
            try
            {
                // Path to the user's XML file
                string userDirectory = "Users";
                string userFilePath = Path.Combine(userDirectory, $"{CurrentPlayer.Username}.xml");

                if (!File.Exists(userFilePath))
                {
                    MessageBox.Show($"No user file found for {CurrentPlayer.Username}.", "No Saved Game",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Load the user data
                XmlSerializer serializer = new XmlSerializer(typeof(UserData));
                UserData userData;

                using (FileStream fs = new FileStream(userFilePath, FileMode.Open))
                {
                    userData = (UserData)serializer.Deserialize(fs);
                }

                // Check if there's a saved game
                if (userData.SavedGame == null)
                {
                    MessageBox.Show($"No saved game found for {CurrentPlayer.Username}.", "No Saved Game",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SavedGame savedGame = userData.SavedGame;

                // Stop the current game timer
                _gameTimer.Stop();

                // Restore game settings
                BoardRows = savedGame.BoardRows;
                BoardColumns = savedGame.BoardColumns;
                _selectedCategory = savedGame.Category;
                _selectedBackground = savedGame.Background;
                CurrentScore = savedGame.Score;
                _moves = savedGame.Moves;

                // Calculate remaining time
                int totalCards = BoardRows * BoardColumns;
                int totalTime = totalCards * 2;
                totalTime = (int)Math.Ceiling(totalTime / 10.0) * 10;
                _secondsRemaining = totalTime - (int)savedGame.ElapsedTime.TotalSeconds;

                // Update timer display
                int minutes = _secondsRemaining / 60;
                int seconds = _secondsRemaining % 60;
                ElapsedTime = $"{minutes:D2}:{seconds:D2}";

                // Reset card tracking
                _firstFlippedCard = null;
                _secondFlippedCard = null;
                _isProcessingTurn = false;

                // Restore the cards
                // Restore the cards
                Cards = new ObservableCollection<Card>();
                // Folosește fundalul selectat
                BitmapImage backImage = new BitmapImage(new Uri($"pack://application:,,,/Images/Game/Background{_selectedBackground}.png", UriKind.Absolute));

                foreach (var cardState in savedGame.CardStates)
                {
                    BitmapImage frontImage = new BitmapImage(
                        new Uri($"pack://application:,,,/Images/Game/Category{_selectedCategory}/Photo{cardState.ImageId}.png", UriKind.Absolute));

                    var card = new Card(cardState.Id, frontImage, backImage)
                    {
                        IsFlipped = cardState.IsFlipped,
                        IsMatched = cardState.IsMatched
                    };

                    Cards.Add(card);
                }

                // Clear the saved game from userData
                userData.SavedGame = null;

                // Save the updated user data without the saved game
                using (FileStream fs = new FileStream(userFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, userData);
                }

                // Start the timer for the loaded game
                _gameTimer.Start();

                MessageBox.Show($"Game loaded successfully for {CurrentPlayer.Username}!", "Game Loaded",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game: {ex.Message}", "Load Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowStatistics()
        {
            try
            {
                // Directory containing user files
                string userDirectory = "Users";

                // List to hold all users' statistics
                List<UserStatistics> allUsersStats = new List<UserStatistics>();

                // Get all user XML files
                if (Directory.Exists(userDirectory))
                {
                    string[] userFiles = Directory.GetFiles(userDirectory, "*.xml");

                    foreach (string userFile in userFiles)
                    {
                        try
                        {
                            // Load user data from XML file
                            XmlSerializer serializer = new XmlSerializer(typeof(UserData));

                            using (FileStream fs = new FileStream(userFile, FileMode.Open))
                            {
                                UserData userData = (UserData)serializer.Deserialize(fs);

                                // Create statistics object for each user
                                UserStatistics userStats = new UserStatistics
                                {
                                    Username = userData.User.Username,
                                    GamesPlayed = userData.User.GamesPlayed,
                                    GamesWon = userData.User.GamesWon,
                                    TotalScore = userData.User.TotalScore,
                                    TotalPlayTime = userData.User.TotalPlayTime
                                };

                                allUsersStats.Add(userStats);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading user from {userFile}: {ex.Message}", "Statistics Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Create and show statistics dialog
                    var statsDialog = new StatisticsDialog(allUsersStats);
                    statsDialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("User directory not found.", "Statistics Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Statistics Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit()
        {
            // Întreabă utilizatorul dacă dorește să salveze jocul înainte de ieșire
            if (Cards != null && Cards.Count > 0)
            {
                var result = MessageBox.Show("Do you want to save the current game before exiting?",
                    "Save Game", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                    return;

                if (result == MessageBoxResult.Yes)
                    SaveGame();
            }

            // Oprim timer-ul
            _gameTimer.Stop();

            // Declanșăm evenimentul pentru a reveni la ecranul de login
            ReturnToSignIn?.Invoke(this, EventArgs.Empty);
        }

        private void SetBoardSize(string sizeType)
        {
            // Setăm dimensiunile în așteptare, care vor fi aplicate doar când jocul începe
            if (sizeType == "Standard")
            {
                _pendingBoardRows = 4;
                _pendingBoardColumns = 4;
                MessageBox.Show("Board size set to Standard (4x4). Press New Game to start.",
                    "Board Size Changed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (sizeType == "Custom")
            {
                var dialog = new CustomBoardDialog();
                if (dialog.ShowDialog() == true)
                {
                    _pendingBoardRows = dialog.SelectedRows;
                    _pendingBoardColumns = dialog.SelectedColumns;
                    MessageBox.Show($"Board size set to Custom ({dialog.SelectedRows}x{dialog.SelectedColumns}). Press New Game to start.",
                        "Board Size Changed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        
        private void ShowAboutDialog()
        {
            var dialog = new AboutDialog();
            dialog.ShowDialog();
        }

        private bool CanFlipCard(Card card)
        {
            return card != null && !card.IsFlipped && !card.IsMatched && !_isProcessingTurn && _secondsRemaining > 0;
        }

        private void FlipCard(Card card)
        {
            if (!CanFlipCard(card))
                return;

            // Întoarcem cartea
            card.IsFlipped = true;

            // Verificăm dacă este prima sau a doua carte întoarsă
            if (_firstFlippedCard == null)
            {
                // Prima carte întoarsă
                _firstFlippedCard = card;
            }
            else
            {
                // A doua carte întoarsă - verificăm dacă e pereche
                _secondFlippedCard = card;
                _moves++;
                _isProcessingTurn = true;

                // Verificăm potrivirea (getId este folosit ca identificator de imagine)
                bool isMatch = _firstFlippedCard.FrontImage.ToString() == card.FrontImage.ToString();

                if (isMatch)
                {
                    // Cărțile formează o pereche
                    _firstFlippedCard.IsMatched = true;
                    _secondFlippedCard.IsMatched = true;
                    CurrentScore += 10; // Adăugăm puncte pentru pereche

                    _firstFlippedCard = null;
                    _secondFlippedCard = null;
                    _isProcessingTurn = false;

                    // Verificăm dacă jocul s-a terminat
                    CheckGameCompletion();
                }
                else
                {
                    // Cărțile nu formează o pereche - le întoarcem înapoi după un scurt delay
                    // Folosim Application.Current.Dispatcher pentru a asigura execuția pe thread-ul UI
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        // Creăm un nou timer pentru această operațiune
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                        timer.Tick += (s, e) =>
                        {
                            // Oprim timer-ul pentru a nu se executa de mai multe ori
                            timer.Stop();

                            // Întoarcem cărțile înapoi
                            if (_firstFlippedCard != null)
                                _firstFlippedCard.IsFlipped = false;

                            if (_secondFlippedCard != null)
                                _secondFlippedCard.IsFlipped = false;

                            // Resetăm starea
                            _firstFlippedCard = null;
                            _secondFlippedCard = null;
                            _isProcessingTurn = false;

                            // Forțăm o actualizare a UI
                            CommandManager.InvalidateRequerySuggested();
                        };

                        // Pornește timer-ul
                        timer.Start();
                    }));
                }
            }
        }

        private void ResetGameState()
        {
            // Oprește timer-ul
            _gameTimer.Stop();

            // Resetează scorul și timpul afișat
            CurrentScore = 0;
            ElapsedTime = "00:00";

            // Șterge toate cărțile din joc
            Cards = null; // Sau Cards.Clear(); dacă preferi să păstrezi lista goală

            // Resetează selecțiile și starea jocului
            _firstFlippedCard = null;
            _secondFlippedCard = null;
            _isProcessingTurn = false;

            // Notifică UI-ul despre schimbări
            OnPropertyChanged(nameof(CurrentScore));
            OnPropertyChanged(nameof(ElapsedTime));
            OnPropertyChanged(nameof(Cards));
        }

        private void CheckGameCompletion()
        {
            // Verificăm dacă toate cărțile au fost potrivite
            if (Cards != null && Cards.All(c => c.IsMatched))
            {
                _gameTimer.Stop();

                // Calculăm timpul rămas
                int timeRemaining = _secondsRemaining;

                // Adăugăm bonus pentru timpul rămas
                int timeBonus = timeRemaining * 2;
                CurrentScore += timeBonus;

                // Afișăm mesaj de felicitare
                MessageBox.Show($"Congratulations! You've completed the game!\n\nScore: {CurrentScore}\nTime Bonus: +{timeBonus}\nTime Remaining: {timeRemaining} seconds\nMoves: {_moves}",
                    "Game Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                // Calculate the total allocated time
                int totalCards = BoardRows * BoardColumns;
                int totalTime = totalCards * 2;
                totalTime = (int)Math.Ceiling(totalTime / 10.0) * 10;

                // Salvăm statisticile pentru jocul curent
                SaveGameStatistics(TimeSpan.FromSeconds(totalTime - _secondsRemaining));

                // Revine la starea inițială a jocului
                ResetGameState();
            }
        }

        private void SaveGameStatistics(TimeSpan duration)
        {
            try
            {
                // Path to the user's XML file
                string userDirectory = "Users";
                string userFilePath = Path.Combine(userDirectory, $"{CurrentPlayer.Username}.xml");

                if (!File.Exists(userFilePath))
                {
                    MessageBox.Show($"User file for {CurrentPlayer.Username} not found.", "Statistics Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Load the existing user data
                XmlSerializer serializer = new XmlSerializer(typeof(UserData));
                UserData userData;

                using (FileStream fs = new FileStream(userFilePath, FileMode.Open))
                {
                    userData = (UserData)serializer.Deserialize(fs);
                }

                // Update the user statistics
                userData.User.GamesPlayed++; // Increment games played

                // Add current game score to total score
                userData.User.TotalScore += CurrentScore;

                // Add the current game's duration to total play time
                userData.User.TotalPlayTime += duration;

                // Check if the game was won (all cards matched)
                bool gameWon = Cards != null && Cards.All(c => c.IsMatched);
                if (gameWon)
                {
                    userData.User.GamesWon++; // Increment games won
                }

                // Save the updated user data
                using (FileStream fs = new FileStream(userFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, userData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game statistics: {ex.Message}", "Statistics Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}