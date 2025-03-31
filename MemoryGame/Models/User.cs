using System;
using System.ComponentModel;

namespace MemoryGame.Models
{
    [Serializable]
    public class User : INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        // Statistici de joc
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int TotalScore { get; set; }
        public TimeSpan TotalPlayTime { get; set; }

        public User()
        {
            GamesPlayed = 0;
            GamesWon = 0;
            TotalScore = 0;
            TotalPlayTime = TimeSpan.Zero;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}