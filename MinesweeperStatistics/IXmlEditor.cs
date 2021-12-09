namespace MinesweeperStatistics
{
    /// <summary>
    /// Именнованные константы, соответствующие уровням сложности.
    /// </summary>
    public enum Difficulty
    {
        Beginner,
        Intermediate,
        Advanced
    };

    public interface IXmlEditor
    {
        /// <summary>
        /// Метод, позволяющий получать текущее значение статистики.
        /// </summary>
        /// <param name="difficulty">Уровень сложности.</param>
        /// <returns>Объект типа Statistics.</returns>
        Statistics GetStatistics(Difficulty difficulty);

        /// <summary>
        /// Метод, позволяющий устанавливать статистику.
        /// </summary>
        /// <param name="difficulty">Уровень сложности.</param>
        /// <param name="statistics">Значение статистики.</param>
        void SetStatistics(Difficulty difficulty, Statistics statistics);
    }
}
