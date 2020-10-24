namespace Deoxys.Core.Disassembly
{
    public class NashaMethodDisassembler
    {
        public DeoxysContext Context { get; }
        public NashaInstructionDisassembler InstructionDisassembler { get; set; }

        public NashaMethodDisassembler(DeoxysContext context)
        {
            Context = context;
            InstructionDisassembler = new NashaInstructionDisassembler(this);
        }

        public NashaMethod DisassembleMethod(NashaMethodInfo methodInfo)
        {
            var method = new NashaMethod(methodInfo);
            method.MethodBody = DisassembleMethodBody(method);
            return method;
        }

        private NashaMethodBody DisassembleMethodBody(NashaMethod method)
        {
            var body = new NashaMethodBody(method);
            body.Instructions = InstructionDisassembler.DisassembleAllInstructions(method);
            return body;
        }
    }
}