using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace Deoxys.Core.Recompiling
{
    public class NashaRecompiler
    {
        public DeoxysContext Context { get; }

        public NashaRecompiler(DeoxysContext context)
        {
            Context = context;
        }

        public void RecompileMethod(NashaMethod nashaMethod)
        {
            nashaMethod.Parent.Method.CilMethodBody = RecompileCilMethodBody(nashaMethod.MethodBody);
        }

        public CilMethodBody RecompileCilMethodBody(NashaMethodBody nashaMethodBody)
        {
            var body = new CilMethodBody(nashaMethodBody.NashaMethod.Parent.Method);
            foreach (var instruction in nashaMethodBody.Instructions)
            {
                var cilInstruction = RecompileInstruction(instruction);
                body.Instructions.Add(cilInstruction);
            }
            //TODO: Fixups
            return body;
        }

        private CilInstruction RecompileInstruction(NashaInstruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case NashaCode.Nop:
                    return new CilInstruction(CilOpCodes.Nop);
                case NashaCode.Ret:
                    return new CilInstruction(CilOpCodes.Ret);
                case NashaCode.Ldstr:
                    return new CilInstruction(CilOpCodes.Ldstr,instruction.Operand);
                case NashaCode.Ldarg_0:
                    return new CilInstruction(CilOpCodes.Ldarg_0);
                case NashaCode.Call:
                    return new CilInstruction(CilOpCodes.Call,instruction.Operand);
            }
            return new CilInstruction(CilOpCodes.Nop);
        }
    }
}