using System.Reflection.Emit;

namespace Deoxys.Core
{
    public class NashaInstruction
    {
        public NashaOpCode OpCode { get; }
        public object Operand { get; set; }

        public NashaInstruction(NashaOpCode opCode) : this(opCode,null) {}

        public NashaInstruction() : this(new NashaOpCode(NashaCode.Nop,0),null) {}
        public NashaInstruction(NashaOpCode opCode, object operand)
        {
            OpCode = opCode;
            Operand = operand;
        }

        public override string ToString()
        {
            if (Operand == null)
                return $"[{OpCode.Code}]";
            return $"[{OpCode.Code}] - [{Operand}]";
        }
    }
}