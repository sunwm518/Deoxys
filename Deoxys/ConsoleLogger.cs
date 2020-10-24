using System;
using System.Drawing;
using Deoxys.Core;
using Console = Colorful.Console;

namespace Deoxys
{
    public class ConsoleLogger : ILogger
    {
        public void Success(string message)
        {
            WriteCool(message,Color.CornflowerBlue);
        }

        public void Error(string message)
        {
            WriteCool(message, Color.Red);
        }

        public void Info(string message)
        {
            WriteCool(message, Color.Gray);
        }

        public void Warning(string message)
        {
            WriteCool(message, Color.Orange);
        }

        private void WriteCool(string message,Color color)
        {
            Console.WriteFormatted("[{0}] ",color,Color.White,DateTime.Now.ToString("HH:mm:ss"));
            Console.WriteLine(message + " ",color);
        }
    }
}