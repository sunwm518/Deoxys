using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Deoxys.Core;

namespace Deoxys.Pipeline.DevirtualizationStages
{
    public class CfgDetection : IDevirtualizationStage
    {
        public string Name => nameof(CfgDetection);

        public bool Execute(DeoxysContext context)
        {
            var ctor = context.Module.GetOrCreateModuleConstructor();
            var module = ctor.DeclaringType;
            if (module.Fields.Count >= 1 && ctor.CilMethodBody.Instructions.Count == 7)
            {
                var field = ctor.CilMethodBody.Instructions[1];
                if (field.OpCode == CilOpCodes.Stsfld && field.Operand is FieldDefinition fieldDefinition)
                {
                    context.Cfg = fieldDefinition;
                    context.Logger.Success($"Detected CFG Field [{context.Cfg.Name}] At Constructor {module.Name}");
                    return true;
                }
            }
            return false;
        }
    }
}