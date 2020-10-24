using System.Collections.Generic;
using Deoxys.Core;
using Deoxys.Pipeline.DevirtualizationStages;

namespace Deoxys.Pipeline
{
    public class Devirtualizer
    {
        public Devirtualizer(DeoxysContext ctx)
        {
            Ctx = ctx;
            DevirtualizationStages = new List<IDevirtualizationStage>
            {
                new SectionDetection(),
                new CfgDetection(),
                new MethodDetection()
            };
        }

        public DeoxysContext Ctx { get; set; }
        public IList<IDevirtualizationStage> DevirtualizationStages { get; }

        public void Devirtualize()
        {
            foreach (var stage in DevirtualizationStages)
            {
                Ctx.Logger.Info($"Executing Devirtualization Stage {stage.Name}...");
                bool result = stage.Execute(Ctx);
                if (result)
                {
                    Ctx.Logger.Success($"Executed Devirtualization Stage {stage.Name}!");
                }
                else
                {
                    Ctx.Logger.Error($"Devirtualization Failed At Stage {stage.Name}! Aborting...");
                    break;
                }
            }
        }

        public void Save()
        {
        }
    }
}