using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class MethodDetection : IDevirtualizationStage
    {
        public string Name => nameof(MethodDetection);

        public bool Execute(DeoxysContext context)
        {
            MapMethods(context);
            return context.VirtualizedMethods.Count != 0;
        }

        private void MapMethods(DeoxysContext context)
        {
            foreach (var type in context.Module.GetAllTypes())
            {
                foreach (var method in type.Methods.Where(q => q.IsIL && q.CilMethodBody.Instructions.Count >= 7))
                {
                    var instructions = method.CilMethodBody.Instructions;
                    if (instructions.First().OpCode == CilOpCodes.Newobj &&
                        ((IMethodDescriptor) instructions.First().Operand).DeclaringType !=
                        context.Cfg.Signature.FieldType.GetUnderlyingTypeDefOrRef())
                    {
                        var instructionField = instructions.First(q => q.OpCode == CilOpCodes.Ldsfld);
                        if (instructionField != null)
                        {
                            var index = instructions.IndexOf(instructionField);
                            var methodKey = instructions[index - 1].GetLdcI4Constant();
                            context.VirtualizedMethods.Add(new NashaMethodInfo(method, methodKey));
                            context.Logger.Success($"Found Virtualized Method {method.Name} With Key {methodKey}");
                        }
                    }
                }
            }
        }
    }
}