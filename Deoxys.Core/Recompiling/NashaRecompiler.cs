using System.Diagnostics.SymbolStore;
using System.Linq;
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
                Context.Logger.Info($"Recompiled Nasha Instruction {instruction} Into Cil Instruction {cilInstruction}");
            }
            //After converting Nasha instruction to cil some instructions will need to be edited after.
            //Ex: Br - 30 =>
            //    Br - (Cil Label of Instruction with index 30)
            ReplaceOperands(body);
            return body;
        }

        private void ReplaceOperands( CilMethodBody cilMethodBody)
        {
            foreach (var instruction in cilMethodBody.Instructions)
            {
                switch (instruction.OpCode.Code)
                {
                    case CilCode.Br:
                    case CilCode.Brtrue:
                    case CilCode.Brfalse:
                        instruction.Operand = new CilInstructionLabel(cilMethodBody.Instructions[(int) instruction.Operand + 1]);
                        break;
                    case CilCode.Ldloc:
                    case CilCode.Stloc:
                        //NashaVM handles all local variables as type of object, which means it doesn't give us info
                        //about them. I'm planning to make something to recover them soon.
                        instruction.Operand = cilMethodBody.LocalVariables.Count > (int) instruction.Operand
                            ? cilMethodBody.LocalVariables[(int) instruction.Operand]
                            : new CilLocalVariable(cilMethodBody.Owner.Module.CorLibTypeFactory.Object);
                        if (instruction.Operand is CilLocalVariable variable && variable.Index == -1)
                        {
                            cilMethodBody.LocalVariables.Add(variable);
                        }
                        break;
                    case CilCode.Ldarg:
                        instruction.Operand =
                            cilMethodBody.Owner.Parameters.GetBySignatureIndex((short) instruction.Operand);
                        break;
                }
            }
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
                case NashaCode.Ldarg:
                    return new CilInstruction(CilOpCodes.Ldarg,instruction.Operand);
                case NashaCode.Call:
                    return new CilInstruction(CilOpCodes.Call,instruction.Operand);
                case NashaCode.Br:
                    return new CilInstruction(CilOpCodes.Br,instruction.Operand);
                case NashaCode.LdcI4:
                    return new CilInstruction(CilOpCodes.Ldc_I4,instruction.Operand);
                case NashaCode.Ldloc:
                    return new CilInstruction(CilOpCodes.Ldloc,instruction.Operand);
                case NashaCode.Stloc:
                    return new CilInstruction(CilOpCodes.Stloc,instruction.Operand);
                case NashaCode.BrFalse:
                    return new CilInstruction(CilOpCodes.Brfalse,instruction.Operand);
                case NashaCode.BrTrue:
                    return new CilInstruction(CilOpCodes.Brtrue,instruction.Operand);
                case NashaCode.Pop:
                    return new CilInstruction(CilOpCodes.Pop);
                case NashaCode.Newobj:
                    return new CilInstruction(CilOpCodes.Newobj,instruction.Operand);
                case NashaCode.Castclass:
                    return new CilInstruction(CilOpCodes.Castclass,instruction.Operand);
                case NashaCode.Ldftn:
                    return new CilInstruction(CilOpCodes.Ldftn,instruction.Operand);
                case NashaCode.Dup:
                    return new CilInstruction(CilOpCodes.Dup);
                case NashaCode.Ldfld:
                    return new CilInstruction(CilOpCodes.Ldfld,instruction.Operand);
                case NashaCode.Stfld:
                    return new CilInstruction(CilOpCodes.Stfld,instruction.Operand);
                case NashaCode.Newarr:
                    return new CilInstruction(CilOpCodes.Newarr,instruction.Operand);
            }
            return new CilInstruction(CilOpCodes.Nop);
        }
    }
}