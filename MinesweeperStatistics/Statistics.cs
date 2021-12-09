using System;

namespace MinesweeperStatistics
{
    public class Statistics
    {
        private int gamesPlayed;
        private int gamesWon;
        private int longestWinStreak;
        private int longestLoseStreak;

        public int GamesPlayed
        {
            get { return gamesPlayed; }
            set
            {
                var validateResult = Validate(value, gamesWon, longestWinStreak, longestLoseStreak);
                if (!validateResult.IsValid)
                    throw new Exception(validateResult.Message);

                gamesPlayed = value;
            }
        }

        public int GamesWon
        {
            get { return gamesWon; }
            set
            {
                var validateResult = Validate(gamesPlayed, value, longestWinStreak, longestLoseStreak);
                if (!validateResult.IsValid)
                    throw new Exception(validateResult.Message);

                gamesWon = value;
            }
        }

        public int LongestWinStreak
        {
            get { return longestWinStreak; }
            set
            {
                var validateResult = Validate(gamesPlayed, gamesWon, value, longestLoseStreak);
                if (!validateResult.IsValid)
                    throw new Exception(validateResult.Message);

                longestWinStreak = value;
            }
        }

        public int LongestLoseStreak
        {
            get { return longestLoseStreak; }
            set
            {
                var validateResult = Validate(gamesPlayed, gamesWon, longestWinStreak, value);
                if (!validateResult.IsValid)
                    throw new Exception(validateResult.Message);

                longestLoseStreak = value;
            }
        }

        public Statistics(int gamesPlayed, int gamesWon, int longestWinStreak, int longestLoseStreak)
        {
            var validateResult = Validate(gamesPlayed, gamesWon, longestWinStreak, longestLoseStreak);
            if (!validateResult.IsValid)
                throw new Exception(validateResult.Message);

            this.gamesPlayed = gamesPlayed;
            this.gamesWon = gamesWon;
            this.longestWinStreak = longestWinStreak;
            this.longestLoseStreak = longestLoseStreak;
        }

        /// <summary>
        /// Метод, отвечающий за проверку бизнес-правил класса.
        /// </summary>
        /// <param name="vGamesPlayed">Количество проведенных игр.</param>
        /// <param name="vGamesWon">Количество выигранных игр.</param>
        /// <param name="vLongestWinStreak">Максимальное количество выигрышей подряд.</param>
        /// <param name="vLongestLoseStreak">Максимальное количество проигрышей подряд.</param>
        /// <returns>Объект типа StatisticsValidateResult.</returns>
        private static StatisticsValidateResult Validate(int vGamesPlayed,
            int vGamesWon, int vLongestWinStreak, int vLongestLoseStreak)
        {
            if (vGamesPlayed < 0 || vGamesWon < 0 || vLongestWinStreak < 0 || vLongestLoseStreak < 0)
                return new StatisticsValidateResult("Parameter can not be " + "negative.");

            if (vGamesWon > vGamesPlayed)
                return new StatisticsValidateResult("Games Won can not " + "be greater than Games Played.");

            if (vGamesPlayed == vGamesWon && (vLongestWinStreak != vGamesWon || vLongestLoseStreak != 0))
                return new StatisticsValidateResult("If Games Played " +
                                                    "equals Games Won, then Longest Win Streak must " +
                                                    "equal Games Won, and Longest Lose Streak must be " +
                                                    "equal to zero.");

            if ((vLongestWinStreak + vLongestLoseStreak) > vGamesPlayed)
                return new StatisticsValidateResult("Sum LongestWinStreak " +
                                                    "and LongestLoseStreak can not be greater than " +
                                                    "Games Played.");

            if (vLongestWinStreak > vGamesWon)
                return new StatisticsValidateResult("Longest Win Streak " +
                                                    "can not be greater than Games Won.");

            if (vGamesWon > 0 && vLongestWinStreak <= 0)
                return new StatisticsValidateResult("If Games Won greater" +
                                                    "zero, then Longest Win Streak must be greater " + "zero");

            if (vGamesPlayed > vGamesWon && vLongestLoseStreak <= 0)
                return new StatisticsValidateResult("If Games Played " +
                                                    "greater Games Won, then Longest Lose Streak " +
                                                    "must be greater zero");

            return new StatisticsValidateResult("");
        }
    }
}