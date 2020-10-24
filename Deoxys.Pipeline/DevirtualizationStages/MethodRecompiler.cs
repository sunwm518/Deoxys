using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class MethodRecompiler : IDevirtualizationStage
    {
        public string Name { get; }
        public bool Execute(DeoxysContext context)
        {
            return true;
        }
    }
}