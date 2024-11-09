using Serilog.Events;

namespace Serilog.Sink.OpenObserve.Tests.Fixtures.Builders;

public class LogEventBuilder
{
    private DateTimeOffset _dateTimeOffset = new DateTimeOffset(2024, 1, 1, 18, 20, 30, TimeSpan.Zero);
    private LogEventLevel _logEventLevel = LogEventLevel.Debug;
    private Exception _exception = null;
    private MessageTemplate _messageTemplate = MessageTemplate.Empty;
    private List<LogEventProperty> _logEventProperties = new List<LogEventProperty>();

    private LogEventBuilder()
    {
    }

    public static LogEventBuilder Init()
    {
        return new LogEventBuilder();
    }

    public LogEventBuilder WithDefinedDateTime(DateTimeOffset dateTimeOffset)
    {
        _dateTimeOffset = dateTimeOffset;
        return this;
    }

    public LogEventBuilder WithDefinedLogLevel(LogEventLevel logEventLevel)
    {
        _logEventLevel = logEventLevel;
        return this;
    }

    public LogEventBuilder WithDefinedException(Exception exception)
    {
        _exception = exception;
        return this;
    }

    public LogEventBuilder WithDefinedEventProperty(LogEventProperty logEventProperty)
    {
        _logEventProperties.Add(logEventProperty);
        return this;
    }

    public LogEventBuilder WithDefinedMessageTemplate(MessageTemplate messageTemplate)
    {
        _messageTemplate = messageTemplate;
        return this;
    }

    public LogEvent Build()
    {
        return new LogEvent(
            _dateTimeOffset, _logEventLevel, _exception, _messageTemplate, _logEventProperties);
    }
}