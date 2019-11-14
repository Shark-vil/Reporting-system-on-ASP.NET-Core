using NLog;

namespace TESTING_WEB_API_ASP.Helpers
{
    public static class HLogger
    {
        // Переменная для работы с файлом лога
        public static readonly Logger log = LogManager.GetCurrentClassLogger();
    }
}
