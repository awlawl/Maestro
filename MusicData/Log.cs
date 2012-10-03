using log4net.Config;

namespace MusicData
{
    public class Log
    {
        public static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Log));

        public static void Debug(string what)
        {
            _log.Debug(what);
        }
    }
}
