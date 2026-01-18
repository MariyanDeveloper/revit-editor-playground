using Functional;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Compilation.Utils;

public static class CSharpSyntaxTreeParsing
{
    extension(CSharpSyntaxTree)
    {
        public static Result<IReadOnlyList<SyntaxTree>> ParseText(IReadOnlyList<string> codebase, CSharpParseOptions parseOptions,
            IReadOnlyList<string> globalUsings)
        {
            try
            {
                var syntaxTrees = codebase
                    .Select(code => CSharpSyntaxTree.ParseText(code, parseOptions));

                if (!globalUsings.Any())
                {
                    return syntaxTrees.ToArray();
                }

                var globalSection = globalUsings
                    .Select(u => $"global using {u};")
                    .JoinBy(Environment.NewLine);

                var globalUsingsTree = CSharpSyntaxTree.ParseText(globalSection, parseOptions);

                return syntaxTrees.Append(globalUsingsTree).ToArray();
            }
            catch (Exception e)
            {
                return Error.Failure(description: $"Failed to parse syntax tree: {e.Message}. {e.StackTrace}");
            }
        }
    }
}