using System;
using System.Collections.Generic;
using System.Text;

namespace LiftSim
{
    public enum LogInfo
    {
        Info,
        Debug
    }
    public static class Logger
    {
        public static LogInfo MaxLogLevel = LogInfo.Debug;
        public static void Log(string message, LogInfo level)
        {
            if (level > MaxLogLevel) return;
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + message);
        }
    }
}
