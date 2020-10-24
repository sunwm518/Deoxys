using AsmResolver.DotNet;

namespace Deoxys.Core
{
    public class NashaMethodInfo
    {
        public readonly int _methodKey;

        public NashaMethodInfo(MethodDefinition method, int methodKey)
        {
            _methodKey = methodKey;
            Method = method;
        }

        public MethodDefinition Method { get; }
    }
}