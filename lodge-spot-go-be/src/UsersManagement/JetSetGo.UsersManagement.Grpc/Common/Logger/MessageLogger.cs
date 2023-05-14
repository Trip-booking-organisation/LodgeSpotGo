namespace JetSetGo.UsersManagement.Grpc.Common.Logger;

public class MessageLogger
{
    private ILogger<MessageLogger> _logger;

    public MessageLogger(ILogger<MessageLogger> logger)
    {
        _logger = logger;
    }

    public void LogInfo(string header,string message)
    {
        _logger.LogInformation($"----- {header} ------ {message}");
    }
    public void LogError(string header,string message)
    {
        _logger.LogError($"----- {header} ------ {message}");
    }
}