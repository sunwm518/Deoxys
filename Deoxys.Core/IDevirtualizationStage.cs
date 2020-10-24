namespace Deoxys.Core
{
    public interface IDevirtualizationStage
    { 
        string Name { get; }
        bool Execute(DeoxysContext context);
    }
}