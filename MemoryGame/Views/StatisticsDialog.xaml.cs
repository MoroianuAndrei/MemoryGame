using System;
using System.Collections.Generic;
using System.Windows;
using MemoryGame.Models;

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for StatisticsDialog.xaml
    /// </summary>
    public partial class StatisticsDialog : Window
    {
        public StatisticsDialog(List<UserStatistics> statistics)
        {
            InitializeComponent();

            // Calculate win rate for each user and format playtime
            foreach (var stat in statistics)
            {
                if (stat.GamesPlayed > 0)
                {
                    stat.WinRate = Math.Round((double)stat.GamesWon / stat.GamesPlayed * 100, 1);
                }
                else
                {
                    stat.WinRate = 0;
                }

                // Format the playtime as a string (hours:minutes:seconds)
                TimeSpan playTime = stat.TotalPlayTime;
                stat.FormattedPlayTime = $"{playTime.Hours:D2}:{playTime.Minutes:D2}:{playTime.Seconds:D2}";
            }

            // Sort by win rate descending
            statistics.Sort((a, b) => b.WinRate.CompareTo(a.WinRate));

            // Set the DataGrid's ItemsSource
            StatsDataGrid.ItemsSource = statistics;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}