using SPD = Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace TESTING_WEB_API_ASP.Helpers
{
    public static class HFile
    {
        /// <summary>
        /// Очистка папки временных файлов.
        /// </summary>
        /// <param name="oldDays">Насколько устаревшие по дням файлы стоит удалять? (По умолчанию 0)</param>
        public static void tempAllClear(int oldDays = 0)
        {
            if (Directory.Exists(LConfig.temp_dir))
            {
                oldDays = -Math.Abs(oldDays);
                foreach (FileInfo file in new DirectoryInfo(LConfig.temp_dir).GetFiles())
                    if (file.LastAccessTime < DateTime.Now.AddDays(oldDays))
                    {
                        file.Delete();
                        HLogger.log.Info("Удаление устаревших файлов кеша: " + file);
                    }
            }
        }

        /// <summary>
        /// Удаление конкретного временного файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="oldDays">Насколько устаревшие по дням файлы стоит удалять? (По умолчанию 0)</param>
        public static void tempFileRemove(string filePath, int oldDays = 0)
        {
            if (File.Exists(filePath))
            {
                oldDays = -Math.Abs(oldDays);
                FileInfo fi = new FileInfo(filePath);
                if (fi.LastAccessTime < DateTime.Now.AddDays(oldDays))
                {
                    fi.Delete();
                    HLogger.log.Info("Удаление устаревших дубликатов: " + filePath);
                }
            }
        }

        /// <summary>
        /// Получить временный поток сконвертированного документа.
        /// </summary>
        /// <param name="fileStream">Входящий поток текущего документа</param>
        /// <param name="tempFilePath">Путь сохранения файла</param>
        /// <returns>Временный поток нового файла (В случае ошибки вернёт null)</returns>
        public static MemoryStream getConvertFileDoc(MemoryStream fileStream, string tempFilePath)
        {
            HLogger.log.Debug("Чтение MemoryStream");

            SPD.Document document = new SPD.Document(fileStream);

            HLogger.log.Debug("Инициализация документа");

            if (!File.Exists(tempFilePath))
            {
                document.SaveToFile(tempFilePath, SPD.FileFormat.PDF);
                HLogger.log.Debug("Сохранение временного файла");
            }
            else
                HLogger.log.Debug("Подобный временный файл уже существует");

            try
            {
                MemoryStream result = new MemoryStream(File.ReadAllBytes(tempFilePath));
                return result;
            }
            catch
            {
                HLogger.log.Error("Ошибка при создании временного потока");
                return null;
            }
        }

        /// <summary>
        /// Получить временный поток сконвертированного документа.
        /// </summary>
        /// <param name="document_bytes">Входящие байты текущего документа</param>
        /// <param name="tempFilePath">Путь сохранения файла</param>
        /// <returns>Временный поток нового файла (В случае ошибки вернёт null)</returns>
        public static MemoryStream getConvertFileDoc(byte[] document_bytes, string tempFilePath)
        {
            using (MemoryStream fileStream = new MemoryStream(document_bytes))
            {
                HLogger.log.Debug("Чтение MemoryStream");

                SPD.Document document = new SPD.Document(fileStream);

                HLogger.log.Debug("Инициализация документа:");

                if (!File.Exists(tempFilePath))
                {
                    document.SaveToFile(tempFilePath, SPD.FileFormat.PDF);
                    HLogger.log.Debug("Сохранение временного файла");
                }
                else
                    HLogger.log.Debug("Подобный временный файл уже существует");

                try
                {
                    MemoryStream result = new MemoryStream(File.ReadAllBytes(tempFilePath));
                    return result;
                }
                catch
                {
                    HLogger.log.Error("Ошибка при создании временного потока");
                    return null;
                }
            }
        }

        /// <summary>
        /// Получить произвольный тип документа на основе расширения файла.
        /// </summary>
        /// <param name="fileExp">Расширение файла</param>
        /// <returns></returns>
        public static string getFileType(string fileExp)
        {
            // Проверка расширения файла, и его исправление в случае невалидности
            HLogger.log.Debug("Расширение полученного файла: " + fileExp);

            if (fileExp.Substring(0, 1) == ".")
            {
                HLogger.log.Debug("Невалидное расширение с начальной точкой");
                fileExp = fileExp.Substring(1, fileExp.Length - 1);
                HLogger.log.Debug("Исправленное расширение файла: " + fileExp);
            }

            /**
             * Проверка допустимых расширений файлов, и возврат их типового отношения
             */

            string[] docs = { "doc", "docx", "html" };

            if (Array.Exists(docs, x => x == fileExp))
                return "doc";

            return null;
        }
    }
}
