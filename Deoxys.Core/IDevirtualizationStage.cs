namespace Deoxys.Core
{
    public interface IDevirtualizationStage
    { 
        string Name { get; }
        void Execute(DeoxysContext context);
    }
}