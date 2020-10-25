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
            Disassembler.Context.Logger.Success($"Disassembling Method {nashaMethod.Parent.Method.Name} With Key {nashaMethod.Parent._methodKey} - Instructions ({count})");
            for (int i = 0; i < count; i++)
            {
                instructions.Add(DisassembleInstruction());
            }
            return instructions;
        }

        private NashaInstruction DisassembleInstruction()
        {
            byte code = InstructionReader.ReadByte();
            if (!Disassembler.Context.DeoxysOpCodes.TryGetValue(code, out var opCode))
            {
                return new NashaInstruction(new NashaOpCode(NashaCode.Nop,0),null);
            }
            var instruction = new NashaInstruction(opCode);
            instruction.Operand = DisassembleOperand(opCode);
            Disassembler.Context.Logger.Info($"Disassembled Instruction {instruction}");
            return instruction;
        }

        private object DisassembleOperand(NashaOpCode opCode)
        {
            switch (opCode.Code)
            {
                case NashaCode.Ret:
                case NashaCode.Nop:
                case NashaCode.Dup:
                    break;
                case NashaCode.Ldstr:
                    return Encoding.UTF8.GetString(InstructionReader.ReadBytes(InstructionReader.ReadInt32()));
                case NashaCode.Call:
                case NashaCode.Newobj:
                case NashaCode.Castclass:
                case NashaCode.Ldftn:
                case NashaCode.Newarr:
                    InstructionReader.ReadInt16();
                    return Disassembler.Context.Module.LookupMember(InstructionReader.ReadInt32());
                case NashaCode.BrTrue:
                case NashaCode.BrFalse:
                case NashaCode.Stloc:
                case NashaCode.Ldloc:
                case NashaCode.LdcI4:
                case NashaCode.Br:
                    return InstructionReader.ReadInt32();
                case NashaCode.Ldarg:
                    return InstructionReader.ReadInt16();
                case NashaCode.Ldfld:
                case NashaCode.Stfld:
                    InstructionReader.ReadBoolean();
                    InstructionReader.ReadInt16();
                    return Disassembler.Context.Module.LookupMember(InstructionReader.ReadInt32());
            }
            return null;
        }
        private byte[] Decompress(byte[] data)
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
            catch(Exception ex)
            {
                Disassembler.Context.Logger.Error($"Could not decompress Nasha Section - ({ex})");
                Console.ReadLine();
                Environment.Exit(0);
            }
            return decompressedArray;
        }
    }
}