using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Deoxys.Core
{
    public class NashaMethod
    {
        public NashaMethodInfo Parent { get; }
        public NashaMethodBody MethodBody { get; set; }
        public NashaMethod(NashaMethodInfo parent)
        {
            Parent = parent;
            MethodBody = new NashaMethodBody(this);
        }
    }
}