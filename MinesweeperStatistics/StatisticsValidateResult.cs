namespace MinesweeperStatistics
{
    public class StatisticsValidateResult
    {
        public string Message { get; set; }

        public bool IsValid
        {
            get { return string.IsNullOrEmpty(Message); }
        }

        public StatisticsValidateResult(string message)
        {
            Message = message;
        }
    }
}