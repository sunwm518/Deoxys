using System;
using System.Linq;
using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class SectionDetection : IDevirtualizationStage
    {
        public string Name => nameof(SectionDetection);

        public bool Execute(DeoxysContext context)
        {
            //Locating Nasha0,Nasha1,Nasha2 Sections
            var peSections = context.PeFile.Sections.Where(q =>
                q.Name != ".text" && q.Name != ".rsrc" && q.Name != ".reloc" && q.Name.Contains("0") ||
                q.Name.Contains("1") || q.Name.Contains("2")).ToList();
            if (!peSections.Any())
            {
                return false;
            }

            if (peSections.Count > 3)
            {
                peSections = peSections.Where(q => q.Name.Contains("Nasha")).ToList();
            }

            if (peSections.Count == 3)
            {
                context.Nasha0 = peSections[0];
                context.Logger.Success(
                    $"Located Nasha Section {context.Nasha0.Name} With Offset {context.Nasha0.Offset}");
                context.Nasha1 = peSections[1];
                context.Logger.Success(
                    $"Located Nasha Section {context.Nasha1.Name} With Offset {context.Nasha1.Offset}");
                context.Nasha2 = peSections[2];
                context.Logger.Success(
                    $"Located Nasha Section {context.Nasha2.Name} With Offset {context.Nasha2.Offset}");
                return true;
            }
            return false;
        }
    }
}