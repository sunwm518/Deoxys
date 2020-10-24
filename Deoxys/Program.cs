using System;
using System.Drawing;
using CommandLine;
using Deoxys.Core;
using Deoxys.Pipeline;
using Console = Colorful.Console;

namespace Deoxys
{
    internal class Program
    {
        private static readonly Version CurrentVersion = new Version("1.0.0");

        public static void Main(string[] args)
        {
            Console.Title = $"Deoxys - Version {CurrentVersion}";
            var options = Parser.Default.ParseArguments<ParseOptions>(args).Value;
            var deoxysOptions = new DeoxysOptions(args[0], options);
            var ctx = new DeoxysContext(deoxysOptions, new ConsoleLogger());
            PrintInfo(ctx);
            var devirtualizer = new Devirtualizer(ctx);
            devirtualizer.Devirtualize();
            devirtualizer.Save();
            Console.ReadLine();
        }

        public static void PrintInfo(DeoxysContext Context)
        {
            //Console.WriteAscii("Deoxys", Color.Red);
            foreach (var line in @"
______                         
|  _  \                        
| | | |___  _____  ___   _ ___ 
| | | / _ \/ _ \ \/ / | | / __|
| |/ /  __/ (_) >  <| |_| \__ \
|___/ \___|\___/_/\_\\__, |___/
                      __/ |    
                     |___/     ".Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                WriteLineMiddle(line,Color.Red);
            }
            Context.Logger.Success($"Loaded file {Context.Module.Name}");
        }

        private static void WriteLineMiddle(string message, Color color)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message),color);
        }
    }
}