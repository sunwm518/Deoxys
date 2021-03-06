﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using AsmResolver;
using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class OpCodeDetection : IDevirtualizationStage
    {
        public string Name => nameof(OpCodeDetection);
        public bool Execute(DeoxysContext context)
        {
            var extractedOpCodes = ReadOpCodes(context,context.Nasha2.ToArray());
            if (extractedOpCodes.Count < 14)
            {
                return false;
            }
            foreach (var opCode in extractedOpCodes)
            {
                context.DeoxysOpCodes[opCode.RandomValue] = opCode;
            }
            return true;
        }
        
        private List<NashaOpCode> ReadOpCodes(DeoxysContext context,byte[] opcodeValues)
        {
            var opCodes = new List<NashaOpCode>();
            var reader = new BinaryReader(new MemoryStream(opcodeValues));
            reader.BaseStream.Position += 8;
            //OpCode scrambling seems to have it's flaws.
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                var opc = ReadOpCode(reader);
                if (opc.Code == (NashaCode) 777 || opc.Code == (NashaCode)1337) // seems to be the exit code
                    break;
                opCodes.Add(opc);
                context.Logger.Info($"Found OpCode {opc.Code} with Random Value {opc.RandomValue}");
            }
            return opCodes;
        }

        private NashaOpCode ReadOpCode(BinaryReader reader)
        {
            var opc = new NashaOpCode();
            reader.BaseStream.Position += 4;
            opc.Code = (NashaCode)reader.ReadInt32();
            reader.BaseStream.Position += 4;
            opc.RandomValue = reader.ReadInt32();
            reader.BaseStream.Position += 8;
            return opc;
        }
    }
}