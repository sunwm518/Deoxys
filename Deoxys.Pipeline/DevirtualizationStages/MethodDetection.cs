using System.IO;
using System.IO.Compression;
using System.Linq;
using AsmResolver;
using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class MethodDetection : IDevirtualizationStage
    {
        public string Name => nameof(MethodDetection);
        public bool Execute(DeoxysContext context)
        {
            return true;
        }
    }
}