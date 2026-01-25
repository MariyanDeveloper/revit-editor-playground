using System.Reflection;
using NetArchTest.Rules;
using RevitEditorPlayground.Execution.InMemory;
using RevitEditorPlayground.Execution.InProcess;
using RevitEditorPlayground.Shared;
using Shouldly;

namespace RevitEditorPlayground.ArchitectureTests.Execution;

public class ExecutionModulesTests
{
    private static Assembly ExecutionAssembly => typeof(InMemoryExecution).Assembly; 
    [Fact]
    public void InMemory_ShouldNotDependOnInProcess()
    {
        var inMemoryNamespace = typeof(InMemoryExecution).Namespace.NotNull();
        var inProcessNamespace =  typeof(InProcessExecution).Namespace.NotNull();

        Types.InAssembly(
                assembly: ExecutionAssembly
            )
            .That()
            .ResideInNamespace(inMemoryNamespace)
            .ShouldNot()
            .HaveDependencyOn(
                dependency: inProcessNamespace
            )
            .GetResult()
            .IsSuccessful
            .ShouldBeTrue();

    }
    
    [Fact]
    public void InProcess_ShouldNotDependOnInMemory()
    {
        var inMemoryNamespace = typeof(InMemoryExecution).Namespace.NotNull();
        var inProcessNamespace =  typeof(InProcessExecution).Namespace.NotNull();

        Types.InAssembly(
                assembly: ExecutionAssembly
            )
            .That()
            .ResideInNamespace(inProcessNamespace)
            .ShouldNot()
            .HaveDependencyOn(
                dependency: inMemoryNamespace
            )
            .GetResult()
            .IsSuccessful
            .ShouldBeTrue();

    }
}
