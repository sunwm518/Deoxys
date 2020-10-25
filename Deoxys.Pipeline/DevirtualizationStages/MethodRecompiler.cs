using Deoxys.Core;
using Deoxys.Core.Recompiling;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class MethodRecompiler : IDevirtualizationStage
    {
        public string Name { get; }
        public bool Execute(DeoxysContext context)
        {
            RecompileMethods(context);
            return true;
        }

        public void RecompileMethods(DeoxysContext context)
        {
            var recompiler = new NashaRecompiler(context);
            foreach (var method in context.DisassembledVirtualizedMethods)
            {
                //Recompiling each method
                recompiler.RecompileMethod(method);
            }
        }
    }
}