using Shouldly;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.OpenObserve.Tests.Builders;

namespace Serilog.Sinks.OpenObserve.Tests;

public class LogEntryFormatterTests
{
    private readonly LogEntryFormatter _logEntryFormatter = new();

    [Fact]
    public void DefaultLogEvent_ShouldBeFormatCorrectly()
    {
        var expected =
            "{\"@t\":\"2024-01-01T18:20:30.0000000Z\",\"@m\":\"\",\"@mt\":\"\",\"@i\":\"00000000\",\"@l\":\"Debug\"}" +
            Environment.NewLine;
        var defaultEvent = LogEventBuilder.Init().Build();
        var writer = new StringWriter();
        _logEntryFormatter.Format(defaultEvent, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(LogEventLevel.Debug, "Debug")]
    [InlineData(LogEventLevel.Error, "Error")]
    [InlineData(LogEventLevel.Fatal, "Fatal")]
    [InlineData(LogEventLevel.Warning, "Warning")]
    [InlineData(LogEventLevel.Information, "Information")]
    [InlineData(LogEventLevel.Verbose, "Verbose")]
    public void DefaultLogEvent_WithLogLevel_ShouldBeFormatCorrectly(LogEventLevel logEventLevel,
        string expectedLevelName)
    {
        var expected =
            $"{{\"@t\":\"2024-01-01T18:20:30.0000000Z\",\"@m\":\"\",\"@mt\":\"\",\"@i\":\"00000000\",\"@l\":\"{expectedLevelName}\"}}" +
            Environment.NewLine;
        var defaultEvent = LogEventBuilder.Init().WithDefinedLogLevel(logEventLevel).Build();
        var writer = new StringWriter();
        _logEntryFormatter.Format(defaultEvent, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public void DefaultLogEvent_WithDefinedException_ShouldBeFormattedCorrectly()
    {
        var expected =
            "{\"@t\":\"2024-01-01T18:20:30.0000000Z\",\"@m\":\"\",\"@mt\":\"\",\"@i\":\"00000000\",\"@l\":\"Error\",\"@x\":\"System.Exception: Exception message\"}" +
            Environment.NewLine;
        var defaultEvent = LogEventBuilder.Init().WithDefinedLogLevel(LogEventLevel.Error)
            .WithDefinedException(new Exception("Exception message")).Build();
        var writer = new StringWriter();
        _logEntryFormatter.Format(defaultEvent, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public void DefaultLogEvent_WithDefinedValues_ShouldBeFormattedCorrectly()
    {
        var expected =
            "{\"@t\":\"2024-01-01T18:20:30.0000000Z\",\"@m\":\"\",\"@mt\":\"\",\"@i\":\"00000000\",\"@l\":\"Debug\",\"name_1\":\"value_1\",\"name_2\":{\"1\":\"2\"}}" +
            Environment.NewLine;
        var defaultEvent = LogEventBuilder.Init()
            .WithDefinedEventProperty(new LogEventProperty("name_1", new ScalarValue("value_1")))
            .WithDefinedEventProperty(new LogEventProperty("name_2",
                new DictionaryValue([
                    new KeyValuePair<ScalarValue, LogEventPropertyValue>(new ScalarValue("1"), new ScalarValue("2"))
                ])))
            .Build();
        var writer = new StringWriter();
        _logEntryFormatter.Format(defaultEvent, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public void DefaultLogEvent_WithDefinedMessageTemplate_ShouldBeFormatedCorrectly()
    {
        var expected =
            "{\"@t\":\"2024-01-01T18:20:30.0000000Z\",\"@m\":\"message token: \\\"First token value\\\"\",\"@mt\":\"message token: {FirstToken}\",\"@i\":\"b0bbb0c0\",\"@l\":\"Debug\",\"FirstToken\":\"First token value\"}" +
            Environment.NewLine;
        var defaultEvent = LogEventBuilder.Init()
            .WithDefinedMessageTemplate(new MessageTemplate(new List<MessageTemplateToken>
            {
                new TextToken("message token: "),
                new PropertyToken("FirstToken", "{FirstToken}")
            }))
            .WithDefinedEventProperty(
                new LogEventProperty("FirstToken", new ScalarValue("First token value")))
            .Build();
        var writer = new StringWriter();
        _logEntryFormatter.Format(defaultEvent, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public void DefaultLogEvent_WithDefinedMessageTemplateAndCustomTokens_ShouldBeFormatedCorrectly()
    {
        var expected =
            "{\"@t\":\"2024-01-01T18:20:30.0000000Z\",\"@m\":\"Count: \\\"5\\\"with data \\\"{ Counter = 5, CounterMessage = Counter value is \\\\\\\"5\\\\\\\" }\\\"\",\"@mt\":\"Count: {Count}with data {Data}\",\"@i\":\"25613a2e\",\"@l\":\"Debug\",\"Count\":\"5\",\"Data\":\"{ Counter = 5, CounterMessage = Counter value is \\\"5\\\" }\"}" +
            Environment.NewLine;
        var defaultEvent = LogEventBuilder.Init()
            .WithDefinedMessageTemplate(new MessageTemplate(new List<MessageTemplateToken>
            {
                new TextToken("Count: "),
                new PropertyToken("Count", "{Count}"),
                new TextToken("with data "),
                new PropertyToken("Data", "{Data}")
            }))
            .WithDefinedEventProperty(
                new LogEventProperty("Count", new ScalarValue("5")))
            .WithDefinedEventProperty(
                new LogEventProperty("Data",
                    new ScalarValue("{ Counter = 5, CounterMessage = Counter value is \"5\" }")))
            .Build();
        var writer = new StringWriter();
        _logEntryFormatter.Format(defaultEvent, writer);
        writer.ToString().ShouldBe(expected);
    }
}