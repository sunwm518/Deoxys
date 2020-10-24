using System.IO;

namespace Deoxys.Core
{
    public class DeoxysOptions
    {
        public DeoxysOptions(string path, ParseOptions parseOptions)
        {
            FilePath = path;
            OutPath = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty,
                $"{Path.GetFileNameWithoutExtension(path)}-Devirtualized{Path.GetExtension(path)}");
            ParseOptions = parseOptions;
        }

        public string FilePath { get; set; }
        public string OutPath { get; set; }
        public ParseOptions ParseOptions { get; set; }
    }
}