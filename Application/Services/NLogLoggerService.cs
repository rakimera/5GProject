using Application.Interfaces;
using NLog;

namespace Application.Services;

public class NLogLoggerService : ILoggerService
{
    private readonly Logger _logger;

    public NLogLoggerService()
    {
        _logger = LogManager.GetCurrentClassLogger();
        ;
    }

    public void LogInformation(string message)
    {
        _logger.Info(message);
    }

    public void LogError(string message)
    {
        _logger.Error(message);
    }
}