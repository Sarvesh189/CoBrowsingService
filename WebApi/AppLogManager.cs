using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApi
{
    public class AppLogManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }

        public static void LogError(Exception ex)
        {
            logger.Error(ex);
        }

        public static string LogFolder()
        {
            var target = LogManager.Configuration.FindTargetByName("default") as NLog.Targets.FileTarget;
            var logfileName = target.FileName.ToString().Trim('\'');
            return logfileName.Substring(0, logfileName.LastIndexOf('\\'));
        }

        public static async Task<string> ReadLog(string file)
        {
            string readTextTask = null;

            if (File.Exists(file))
            {
                using (var reader = new StreamReader(file))
                {
                    readTextTask = await reader.ReadToEndAsync();

                }
            }

            return readTextTask;
        }
    }
    public enum AppLogLevel
    {
        Info = 0,
        Error = 1
    }
}