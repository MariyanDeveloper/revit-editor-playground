using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Shared.Eventful;

public record Eventful<T>(T Value, IReadOnlyList<DomainEvent> Events);