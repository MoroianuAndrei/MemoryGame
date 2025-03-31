using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace MemoryGame.Models
{
    public class Card : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private BitmapImage _frontImage;
        public BitmapImage FrontImage
        {
            get => _frontImage;
            set
            {
                _frontImage = value;
                OnPropertyChanged(nameof(FrontImage));
                UpdateDisplayImage();
            }
        }

        private BitmapImage _backImage;
        public BitmapImage BackImage
        {
            get => _backImage;
            set
            {
                _backImage = value;
                OnPropertyChanged(nameof(BackImage));
                UpdateDisplayImage();
            }
        }

        private bool _isFlipped;
        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                OnPropertyChanged(nameof(IsFlipped));
                UpdateDisplayImage();
            }
        }

        private bool _isMatched;
        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged(nameof(IsMatched));
                UpdateDisplayImage();
            }
        }

        private BitmapImage _displayImage;
        public BitmapImage DisplayImage
        {
            get => _displayImage;
            set
            {
                _displayImage = value;
                OnPropertyChanged(nameof(DisplayImage));
            }
        }

        public Card(int id, BitmapImage frontImage, BitmapImage backImage)
        {
            Id = id;
            FrontImage = frontImage;
            BackImage = backImage;
            IsFlipped = false;
            IsMatched = false;
            UpdateDisplayImage();
        }

        private void UpdateDisplayImage()
        {
            DisplayImage = IsFlipped || IsMatched ? FrontImage : BackImage;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}