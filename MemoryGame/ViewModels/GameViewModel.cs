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
        private DateTime _gameStartTime;
        private DispatcherTimer _gameTimer;
        private Card _firstFlippedCard;
        private Card _secondFlippedCard;
        private bool _isProcessingTurn;
        private TimeSpan _currentElapsedTime;
        private int _moves;

        public ICommand SelectCategoryCommand { get; }
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
            ElapsedTime = "00:00:00";
            CurrentScore = 0;
            _currentElapsedTime = TimeSpan.Zero;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _currentElapsedTime = _currentElapsedTime.Add(TimeSpan.FromSeconds(1));
            ElapsedTime = _currentElapsedTime.ToString("hh\\:mm\\:ss");
        }

        private void SelectCategory(int categoryId)
        {
            _selectedCategory = categoryId;
            MessageBox.Show($"Selected Category: {categoryId}", "Category", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StartNewGame()
        {
            // Resetăm timer-ul și scorul
            _gameTimer.Stop();
            _currentElapsedTime = TimeSpan.Zero;
            ElapsedTime = "00:00:00";
            CurrentScore = 0;
            _moves = 0;
            _firstFlippedCard = null;
            _secondFlippedCard = null;
            _isProcessingTurn = false;

            // Creăm cărțile pentru joc
            CreateGameBoard();

            // Începem cronometrarea
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
            BitmapImage backImage = new BitmapImage(new Uri($"pack://application:,,,/Images/Game/Background.png", UriKind.Absolute));

            for (int i = 0; i < totalCards; i++)
            {
                // Încarcă imaginea frontală bazată pe categoria selectată
                BitmapImage frontImage = new BitmapImage(
                    new Uri($"pack://application:,,,/Images/Game/Category{_selectedCategory}/Photo{imageIds[i]}.png", UriKind.Absolute));

                Cards.Add(new Card(i, frontImage, backImage));
            }
        }

        private void OpenSavedGame()
        {
            // Implementare pentru deschiderea unui joc salvat
            // ...
        }

        private void SaveGame()
        {
            // Implementare pentru salvarea jocului curent
            // ...
        }

        private void ShowStatistics()
        {
            // Implementare pentru afișarea statisticilor
            // ...
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
            if (sizeType == "Standard")
            {
                BoardRows = 4;
                BoardColumns = 4;
            }
            else if (sizeType == "Custom")
            {
                // Afișăm dialogul pentru setarea dimensiunii personalizate
                var dialog = new CustomBoardDialog();
                if (dialog.ShowDialog() == true)
                {
                    BoardRows = dialog.SelectedRows;
                    BoardColumns = dialog.SelectedColumns;
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
            return card != null && !card.IsFlipped && !card.IsMatched && !_isProcessingTurn;
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

        private void CheckGameCompletion()
        {
            // Verificăm dacă toate cărțile au fost potrivite
            if (Cards != null && Cards.All(c => c.IsMatched))
            {
                _gameTimer.Stop();

                // Calculăm timpul total de joc
                TimeSpan gameDuration = DateTime.Now - _gameStartTime;

                // Afișăm mesaj de felicitare
                MessageBox.Show($"Congratulations! You've completed the game!\n\nScore: {CurrentScore}\nTime: {ElapsedTime}\nMoves: {_moves}",
                    "Game Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                // Salvăm statisticile pentru jocul curent
                SaveGameStatistics(gameDuration);
            }
        }

        private void SaveGameStatistics(TimeSpan duration)
        {
            // Implementare pentru salvarea statisticilor după finalizarea jocului
            // ...
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}