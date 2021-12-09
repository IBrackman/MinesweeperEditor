using System;
using System.Windows;
using System.Windows.Controls;
using MinesweeperStatistics;

namespace MinesweeperEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IXmlEditor editor;

        private Statistics currentStatistics;

        public MainWindow()
        {
            InitializeComponent();

            editor = StatisticsEditor.Load();

            SetButton.Visibility = Visibility.Hidden;
        }

        private void ShowStatistics()
        {
            GamesPlayed.Text = currentStatistics.GamesPlayed.ToString();
            GamesWon.Text = currentStatistics.GamesWon.ToString();
            LongestWinStreak.Text = currentStatistics.LongestWinStreak.ToString();
            LongestLoseStreak.Text = currentStatistics.LongestLoseStreak.ToString();
        }

        private Statistics ReadStatistics()
        {
            int gamesPlayed;
            int gamesWon;
            int longestWinStreak;
            int longestLoseStreak;

            if (!int.TryParse(GamesPlayed.Text, out gamesPlayed))
                throw new ArgumentOutOfRangeException("GamesPlayed");

            if (!int.TryParse(GamesWon.Text, out gamesWon))
                throw new ArgumentOutOfRangeException("GamesWon");

            if (!int.TryParse(LongestWinStreak.Text, out longestWinStreak))
                throw new ArgumentOutOfRangeException("LongestWinStreak");

            if (!int.TryParse(LongestLoseStreak.Text, out longestLoseStreak))
                throw new ArgumentOutOfRangeException("LongestLoseStreak");

            return new Statistics(gamesPlayed, gamesWon, longestWinStreak, longestLoseStreak);
        }

        private Difficulty ReadDifficulty()
        {
            var str = DifficultyBox.SelectedValue.ToString();

            str = str.Substring(str.LastIndexOf(':') + 2);

            switch (str)
            {
                case "Beginner":
                    return Difficulty.Beginner;

                case "Intermediate":
                    return Difficulty.Intermediate;

                case "Advanced":
                    return Difficulty.Advanced;

                default:
                    return Difficulty.Beginner;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void DifficultyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentStatistics = editor.GetStatistics(ReadDifficulty());

            ShowStatistics();

            SetButton.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            editor.SetStatistics(ReadDifficulty(), ReadStatistics());
        }
    }
}