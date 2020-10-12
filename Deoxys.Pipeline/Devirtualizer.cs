using System.Collections.Generic;
using Deoxys.Core;
using Deoxys.Pipeline.DevirtualizationStages;

namespace Deoxys.Pipeline
{
    public class Devirtualizer
    {
        public DeoxysContext Ctx { get; set; }
        public IList<IDevirtualizationStage> DevirtualizationStages { get; }

        public Devirtualizer(DeoxysContext ctx)
        {
            Ctx = ctx;
            DevirtualizationStages = new List<IDevirtualizationStage>()
            {
                new MethodDetection()
            };
        }

        public void Devirtualize()
        {
            foreach (var stage in DevirtualizationStages)
            {
                Ctx.Logger.Info($"Executing Devirtualization Stage {stage.Name}...");
                stage.Execute(Ctx);
                Ctx.Logger.Success($"Executed Devirtualization Stage {stage.Name}!");
            }
        }

        public void Save()
        {
        }
    }
}