using AsmResolver.DotNet;

namespace Deoxys.Core
{
    public class DeoxysMethodInfo
    {
        private readonly int _methodKey;

        public DeoxysMethodInfo(MethodDefinition method, int methodKey)
        {
            _methodKey = methodKey;
            Method = method;
        }

        public MethodDefinition Method { get; }
    }
}