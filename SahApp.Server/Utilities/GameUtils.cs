using System.IO;

namespace SahApp.Server.Utilities
{
    public interface IGameLogger
    {
        void LogMove(string message);
    }

    public abstract class BaseLogger
    {
        protected string _filePath;

        public BaseLogger(string filePath)
        {
            _filePath = filePath;
        }

        public abstract void LogMove(string message);
    }

    public class FileLogger : BaseLogger, IGameLogger
    {
        public FileLogger() : base("game_history.txt") { }

        public override void LogMove(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now}] {message}\n";
                File.AppendAllText(_filePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File logging failed: {ex.Message}");
            }
        }
    }

    public class InvalidChessDataException : Exception
    {
        public InvalidChessDataException(string message) : base(message) { }
        public InvalidChessDataException(string message, Exception inner) : base(message, inner) { }
    }
}