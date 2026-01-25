namespace RevitEditorPlayground.Execution.Contracts;

public class ScriptAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}