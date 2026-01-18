namespace RevitEditorPlayground.Shared.Events;

public static class DomainEvents
{
    extension(DomainEvent)
    {
        public static DomainEvent TimelineDebug(
            string code,
            string message,
            Dictionary<string, object>? metadata = null
        )
        {
            return DomainEvent.Timeline(
                code: code,
                message: message,
                level: EventLevel.Debug,
                metadata: metadata
            );
        }

        public static DomainEvent Timeline(
            string code,
            string message,
            EventLevel level = EventLevel.Info,
            Dictionary<string, object>? metadata = null
        )
        {
            return new TimelineEvent(
                Code: code,
                Message: message,
                Level: level,
                Timestamp: DateTimeOffset.UtcNow,
                Metadata: metadata
            );
        }

        public static DomainEvent Span(
            string code,
            string message,
            DateTimeOffset startTime,
            EventLevel level = EventLevel.Info,
            TimeSpan duration = default,
            Dictionary<string, object>? metadata = null
        )
        {
            return new SpanEvent(
                Code: code,
                Message: message,
                Level: level,
                StartTime: startTime,
                Duration: duration,
                Metadata: metadata
            );
        }
    }
}
