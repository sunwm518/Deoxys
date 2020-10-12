namespace Deoxys.Core
{
    public interface ILogger
    {
        void Success(string message);
        void Error(string message);
        void Info(string message);
        void Warning(string message);
    }
}