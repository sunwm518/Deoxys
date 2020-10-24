using System.Collections.Generic;

namespace Deoxys.Core.Disassembly
{
    public class NashaOperandResolver
    {
        public DeoxysContext Context { get; }

        public NashaOperandResolver(DeoxysContext context)
        {
            Context = context;
        }
    }
}