using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using AsmResolver;

namespace Deoxys.Core.Disassembly
{
    public class NashaInstructionDisassembler
    {
        public NashaMethodDisassembler Disassembler { get; }
        public BinaryReader InstructionReader { get; set; }

        public NashaInstructionDisassembler(NashaMethodDisassembler disassembler)
        {
            Disassembler = disassembler;
            InstructionReader = new BinaryReader(new MemoryStream(Decompress(disassembler.Context.Nasha0.ToArray())));
        }

        public List<NashaInstruction> DisassembleAllInstructions(NashaMethod nashaMethod)
        {
            var instructions = new List<NashaInstruction>();
            InstructionReader.BaseStream.Position = nashaMethod.Parent._methodKey;
            int count = InstructionReader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                instructions.Add(DisassembleInstruction());
            }
            return instructions;
        }

        public NashaInstruction DisassembleInstruction()
        {
            byte code = InstructionReader.ReadByte();
            if (!Disassembler.Context.DeoxysOpCodes.TryGetValue(code, out var opCode))
            {
                return new NashaInstruction(new NashaOpCode(NashaCode.Nop,0),null);
            }
            var instruction = new NashaInstruction(opCode);
            instruction.Operand = DisassembleOperand(opCode);
            return instruction;
        }

        public object DisassembleOperand(NashaOpCode opCode)
        {
            switch (opCode.Code)
            {
                case NashaCode.Ret:
                case NashaCode.Nop:
                    break;
                case NashaCode.Ldstr:
                    return Encoding.UTF8.GetString(InstructionReader.ReadBytes(InstructionReader.ReadInt32()));
                    break;
                case NashaCode.Call:
                    InstructionReader.ReadInt16();
                    return Disassembler.Context.Module.LookupMember(InstructionReader.ReadInt32());
                    break;
                default:
                    break;
            }
            return null;
        }
        private static byte[] Decompress(byte[] data)
        {
            byte[] decompressedArray = null;
            try
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (MemoryStream compressStream = new MemoryStream(data))
                    {
                        using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
                        {
                            deflateStream.CopyTo(decompressedStream);
                        }
                    }
                    decompressedArray = decompressedStream.ToArray();
                }
            }
            catch (Exception exception)
            {
            }

            return decompressedArray;
        }
    }
}