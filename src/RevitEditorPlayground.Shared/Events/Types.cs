namespace RevitEditorPlayground.Shared.Events;

public abstract record DomainEvent(string Code, string Message, EventLevel Level);

public enum EventLevel
{
    Debug,
    Info,
    Warning,
    Error,
}

public sealed record TimelineEvent(
    string Code,
    string Message,
    EventLevel Level,
    DateTimeOffset Timestamp,
    Dictionary<string, object>? Metadata
) : DomainEvent(Code, Message, Level);

public sealed record SpanEvent(
    string Code,
    string Message,
    EventLevel Level,
    DateTimeOffset StartTime,
    TimeSpan Duration,
    Dictionary<string, object>? Metadata
) : DomainEvent(Code, Message, Level);
