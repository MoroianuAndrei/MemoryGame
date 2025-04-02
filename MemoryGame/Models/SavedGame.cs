using System;
using System.Collections.Generic;

namespace MemoryGame.Models
{
    [Serializable]
    public class SavedGame
    {
        public string Username { get; set; }
        public DateTime SaveDate { get; set; }
        public int BoardRows { get; set; }
        public int BoardColumns { get; set; }
        public int Category { get; set; }
        public int Background { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }

        // State of each card (IsFlipped, IsMatched, ImageId)
        public List<CardState> CardStates { get; set; } = new List<CardState>();
    }

    [Serializable]
    public class CardState
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }
    }
}