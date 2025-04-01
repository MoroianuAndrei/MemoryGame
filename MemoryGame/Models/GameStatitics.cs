using System;
using System.Collections.Generic;

namespace MemoryGame.Models
{
    public class GameStatistics
    {
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int BoardSize { get; set; } // Total cards
        public int Moves { get; set; }
        public bool IsCompleted { get; set; }
        public int Category { get; set; }
    }

    [Serializable]
    public class UserStatistics
    {
        public string Username { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public double WinRate { get; set; }
        public int TotalScore { get; set; }
        public TimeSpan TotalPlayTime { get; set; }
        public string FormattedPlayTime { get; set; } // For display purposes
        public List<GameStatistics> RecentGames { get; set; } = new List<GameStatistics>();
    }
}