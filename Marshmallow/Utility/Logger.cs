using System;

namespace Marshmallow.Utility
{
    public class Logger
    {
        public static void Log(string text, LogType type)
        {
            Main.helper.Console.WriteLine(Enum.GetName(typeof(LogType), type) + " : " + text);
        }
        public enum LogType
        {
            Log,
            Error,
            Warning,
            Todo
        }
    }
}
