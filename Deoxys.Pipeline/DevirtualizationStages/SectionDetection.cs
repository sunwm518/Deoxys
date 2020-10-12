using System;
using System.Linq;
using AsmResolver;
using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class SectionDetection : IDevirtualizationStage
    {
        public string Name => nameof(SectionDetection);
        public void Execute(DeoxysContext context)
        {
            var peSections = context.PeFile.Sections.Where(q => q.Name != ".text" && q.Name != ".rsrc" && q.Name != ".reloc" && q.Name.Contains("0") || q.Name.Contains("1")).ToList();
            if (!peSections.Any())
            {
                context.Logger.Error("Could not locate any NashaVM sections!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            if(peSections.Count > 2)
            { 
                peSections = peSections.Where(q => q.Name.Contains("Nasha")).ToList();
            }
            if (peSections.Count == 2)
            {
                context.Nasha0 = peSections[0];
                context.Logger.Success($"Located NashaSection {context.Nasha0.Name} With Offset {context.Nasha0.Offset}");
                context.Nasha1 = peSections[1];
                context.Logger.Success($"Located NashaSection {context.Nasha1.Name} With Offset {context.Nasha1.Offset}");
                return;
            }
            context.Logger.Error("Could not locate any NashaVM sections!");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}