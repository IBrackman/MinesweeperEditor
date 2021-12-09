using System.Linq;
using System.Xml.Linq;

namespace MinesweeperStatistics
{
    internal class XmlEditor : IXmlEditor
    {
        private readonly XElement statisticsXml;

        public XmlEditor(XElement statisticsXml)
        {
            this.statisticsXml = statisticsXml;
        }

        #region IXmlEditor

        public Statistics GetStatistics(Difficulty difficulty)
        {
            return new Statistics
            (
                GetStatisticsParameter(difficulty, "GamesPlayed"),
                GetStatisticsParameter(difficulty, "GamesWon"),
                GetStatisticsParameter(difficulty, "LongestWinStreak"),
                GetStatisticsParameter(difficulty, "LongestLoseStreak")
            );
        }


        public void SetStatistics(Difficulty difficulty, Statistics statistics)
        {
            SetStatisticsParameter(difficulty, "GamesPlayed", statistics.GamesPlayed);
            SetStatisticsParameter(difficulty, "GamesWon", statistics.GamesWon);
            SetStatisticsParameter(difficulty, "LongestWinStreak", statistics.LongestWinStreak);
            SetStatisticsParameter(difficulty, "LongestLoseStreak", statistics.LongestLoseStreak);
        }

        #endregion IXmlEditor

        /// <summary>
        /// Метод, позволяющий получать значение статистического параметра.
        /// </summary>
        /// <param name="difficulty">Уровень сложности.</param>
        /// <param name="parameterName">Название параметра.</param>
        /// <returns>Значение параметра.</returns>
        private int GetStatisticsParameter(Difficulty difficulty, string parameterName)
        {
            var element = GetParameterContainer(difficulty, parameterName);
            return int.Parse(element.Value);
        }

        /// <summary>
        /// Метод, позволяющий получать значение статистического параметра.
        /// </summary>
        /// <param name="difficulty">Уровень сложности.</param>
        /// <param name="parameterName">Название параметра.</param>
        /// <param name="value">Значение параметра.</param>
        private void SetStatisticsParameter(Difficulty difficulty, string parameterName, int value)
        {
            var element = GetParameterContainer(difficulty, parameterName);
            element.SetValue(value);
        }

        /// <summary>
        /// Метод, возвращающий контейнер статистического параметра - тег,
        /// отвечающий за хранение этого параметра.
        /// </summary>
        /// <param name="difficulty">Уровень сложности.</param>
        /// <param name="parameterName">Название параметра.</param>
        /// <returns>Объект XElement.</returns>
        private XElement GetParameterContainer(Difficulty difficulty, string parameterName)
        {
            var gameStat = statisticsXml.Descendants("GameStat").ToArray()[(int) difficulty];
            return gameStat.Element(parameterName);
        }
    }
}