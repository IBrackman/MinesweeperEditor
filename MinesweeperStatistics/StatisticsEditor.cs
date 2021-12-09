using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MinesweeperStatistics
{
    public class StatisticsEditor : IXmlEditor
    {
        /// <summary>
        /// Содержимое файла MinesweeperSettings.xml в виде
        /// последовательности байтов
        /// </summary>
        private readonly byte[] file;

        /// <summary>
        /// Блок бинарных данных файла MinesweeperSettings.xml
        /// </summary>
        private byte[] body;

        /// <summary>
        /// Блок файла, содержащий XML-документ
        /// </summary>
        private XElement statisticsXml;

        /// <summary>
        /// Путь к файлу MinesweeperSettings.xml
        /// </summary>
        private static readonly string PathToStatisticsFile =
            Environment.GetEnvironmentVariable("LocalAppData") +
            @"\Microsoft Games\Minesweeper\MinesweeperSettings.xml";

        private readonly XmlEditor xmlEditor;

        private StatisticsEditor()
        {
            // Проверяем существование файла
            if (!File.Exists(PathToStatisticsFile))
                throw new Exception("Minesweeper Statistics does not " +
                                    "exist on Your computer.");

            // Читаем файл
            file = File.ReadAllBytes(PathToStatisticsFile);

            // Разбиваем на блоки
            SplitXml();

            xmlEditor = new XmlEditor(statisticsXml);
        }


        /// <summary>
        /// Метод, разделяющий бинарные данные файла MinesweeperSettings.xml
        /// от XML-документа. 
        /// </summary>
        private void SplitXml()
        {
            var xmlOffset = GetXmlOffset();
            if (xmlOffset == -1)
                throw new Exception("Sorry, could not edit the " +
                                    "Minesweeper Statistics on Your computer.");

            body = file.Take(xmlOffset).ToArray();

            using (var xml = new MemoryStream(file.Skip(xmlOffset).ToArray()))
            {
                statisticsXml = XElement.Load(xml);
            }
        }

        /// <summary>
        /// Метод, вычисляющий смещение XML-документа в файле
        /// MinesweeperSettings.xml.
        /// </summary>
        /// <returns>Смещение XML-документа или -1, если не удалось вычислить
        /// соответствующее смещение.</returns>
        private int GetXmlOffset()
        {
            for (var i = 0; i < file.Length - 2; ++i)
            {
                // 0x3C,0x47,0x61 соответствует сигнатуре <Ga - началу
                // первого тега <Game> XML-документа. 
                if (file[i] == 0x3C && file[i + 1] == 0x47 && file[i + 2] == 0x61)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Метод, инициализирующий объект StatisticsEditor,
        /// на основе текущего содержимого файла MinesweeperSettings.xml.
        /// </summary>
        /// <returns>Объект типа StatisticsEditor.</returns>
        public static StatisticsEditor Load()
        {
            return new StatisticsEditor();
        }

        /// <summary>
        /// Метод, сохраняющий значение объекта StatisticsEditor
        /// в файл MinesweeperSettings.xml в корректном формате.
        /// </summary>
        public void Save()
        {
            using (var stream = File.OpenWrite(PathToStatisticsFile))
            {
                var xmlString = statisticsXml.ToString(SaveOptions.None);
                xmlString += "\n";

                var xmlBytes = Encoding.UTF8.GetBytes(xmlString);

                ChangeLength(xmlBytes.Length);

                stream.Write(body, 0, body.Length);
                stream.Write(xmlBytes.ToArray(), 0, xmlBytes.Length);
            }
        }

        /// <summary>
        /// Метод, изменяющий длину XML-документа в бинарных данных файла
        /// MinesweeperSettings.xml.
        /// </summary>
        /// <param name="xmlLength">Длина XML-документа.</param>
        private void ChangeLength(int xmlLength)
        {
            var xmlLengthBytes = BitConverter.GetBytes(xmlLength);
            Buffer.BlockCopy(xmlLengthBytes, 0, body, body.Length - 4, 4);
        }

        #region IXmlEditor

        public Statistics GetStatistics(Difficulty difficulty)
        {
            return xmlEditor.GetStatistics(difficulty);
        }

        public void SetStatistics(Difficulty difficulty, Statistics statistics)
        {
            xmlEditor.SetStatistics(difficulty, statistics);

            Save();
        }

        #endregion IXmlEditor
    }
}