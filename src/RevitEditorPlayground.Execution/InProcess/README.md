I think we should have different options for in process, cuz the compilation output is always exe,
https://www.meziantou.net/inline-snapshot-testing-in-dotnet.html
We need a way to control where the exe is put

In test we usually have some special characters.


In code we have a term called `escape characters` and I want to understand what they really mean, and why they were created.
So if we start with a simple question, what is text for programming?
It's probably just a sequence of characters, so what is each character ?
Basically, we probably know every possible character that may occur, right?
So we can map it to some representation that is gonna be understandable for the computer.
Just an example, character `a` we could represent it with 0, then we could be character `b` and we could represent with 1,
if we are limited just by 0 or 1, then we could increase the count of them to more combinations.
so I think in my understanding, we know possible characters that may occur and we have a table for them that allows us
to represent them in a format understandable for the computer.
Is this right?
But then we have a concept `escape character`,
so probably it's still a character, it's still has some representation for the computer, right?
Let me try to read about it more, here is what I found:
```
An escape character is a character that causes one or more characters that follow it to be interpreted differently.
This forms an escape sequence, which is often used to represent a character that has an alternative meaning when printed literally.
```

Let's say we have something like this:
```
var sequenceOfCharacters = "A\nB";
```
I think we still save those characters, e.g. \n, right?
So we store what we see here, we have it in the memory, so does this mean what matters is just the interpretation ?
Interpretation for who ? Text editors, console ?
because if I print it to terminal I see 
```
AB
```
The same happens if I save it as txt and look at it.
Is it just for the interpretation in general?
So if I read it, by knowing those special characters, I can do something with them?
But why we have even introduced them in the first place?

It does make sense.
Maybe, so you say it's for the compiler?
and it interprets it as a single thing?
you said
'\n' → 10
does this mean when running it also has some predefined values that say 10 means to go a new line?


Okay, so compiler has some special meaning for those escape characters,
so if we have `A\nB` and we use ASCII for representing them, we will end up with 3 numbers, not 4.
that means that if I save that text to a file, we save 3 numbers, and not just A and B, we still have that `10` for `\n`.
And then it's up to any viewer that shows the text, if editor supports it makes sure that in the editor it starts at the next line
For example If I have code like this:
```
var sampleCodebase = """
                     using System;
                     Console.WriteLine("Hello World");
                     """;

var bytes = Encoding.ASCII.GetBytes(sampleCodebase);
var rawTextFromBytes = Encoding.ASCII.GetString(bytes);

var textToSave = rawTextFromBytes.Replace(oldValue: "\r\n", newValue: string.Empty);
```
then textToSave appears as just one line,
if I don't replace it appears to be:
```
using System;
Console.WriteLine("Hello World");
```
So that means that escape characters is there, and txt viewer just renders it this way.
So we can probably say that all that formatted text, e.g. when we see json formatted,
behind the hood it just contains some special characters that viewers know how to render,
and that's the reason why formatted json takes more space, cuz there are actually bytes there,
there are more characters.


I am using inline snapshoting tool for dotnet, and here is what I have:
```
var sampleCodebase = """
                     using System;
                     Console.WriteLine("Hello World");
                     """;

var bytes = Encoding.ASCII.GetBytes(sampleCodebase);
var rawTextFromBytes = Encoding.ASCII.GetString(bytes);
        
InlineSnapshot.Validate(rawTextFromBytes, """
    "using System;\r\nConsole.WriteLine(\u0022Hello World\u0022);"
    """);
```

but I want to have it a better representation, something more similar to what we have in text editor,
so I don't want to see \r\n, but visually I want Console.WriteLine to appear on the next line, is this even possible?


I am using this tool:
<PackageReference Include="Meziantou.Framework.InlineSnapshotTesting" Version="3.3.36" />
Do you know if we have a way to use
```csharp
"""
someTest
"""
```
this syntax for snapshoting?
Here are some links
https://www.meziantou.net/inline-snapshot-testing-in-dotnet.htm









Now in memory execution.
it's very similar to in process, the only thing that changes is the way we provide code and execute it.
We need a way to find what is executable.

Let's say I am building a library in C# that can be used for different frameworks,
for net48, for net8 and so on.
And of course, within this library, I will have some checks like if it's for NET48 then do it this way,
if this is for NET8 do it that way.
I think that I need to think from perspective of a client,
for example, if I am building something that uses net48, then when I reference this library, I want to get net48 version of it
so it's compatible, so probably there is something in dotnet that figures it out, right?
Because we just include the package in our csproj.
In such cases it could be useful to check out how it's done by some popular libraries,
so if I go to Newtonsoft, I will see this line:
```
<TargetFrameworks Condition="'$(LibraryFrameworks)'==''">net8.0;net6.0;net45;net40;net35;net20;netstandard1.0;netstandard1.3;netstandard2.0</TargetFrameworks>
<TargetFrameworks Condition="'$(LibraryFrameworks)'!=''">$(LibraryFrameworks)</TargetFrameworks>
```

So If I create a classlib with net48, and reference Newtonsoft:
```
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
  </ItemGroup>
```

In the output I will see that it uses Net.Framework v4.5
If I change the framework to net10.0, I will see that now it uses it from NetCoreApp v6.0
It's still the same version, but different assembly,
then if I use netstandard 2.0, I will see that Newtonsoft comes from .NETStandard v2.0
So the framework is taken into consideration.
And in my library I need to make sure that I am using the same approach.
So people can target different frameworks, right?


Here is another issue I have,
So I have these types:
```csharp
public class SomeAttribute(string name) : Attribute
{
    public string Name { get; } = name;
};

public delegate object? SomeExecutable(object?[] args);
```

So my goal is to allow people to create various scripts and internally I will convert it to `SomeExecutable` delegate.
So here are some sample scripts:
```csharp
public static class SomeScripts
{
    [Some("Some good script")]
    public static object Do(object?[] args)
    {
      Console.WriteLine("Are you high ?");

      return null;
    }

    [Some("Some other script")]
    public static void Do()
    {
      Console.WriteLine("Are you high ?");
    }
  
}
```

And here is what I do:
```csharp
        var executingAssembly = Assembly.GetExecutingAssembly();


        var scriptsByAttributes = executingAssembly.GetTypes()
          .SelectMany(type =>
          {
            return type.GetMethods()
              .SelectMany(method =>
              {
                if (!method.IsStatic)
                {
                  return Enumerable.Empty<ScriptByAttribute>();
                }

                var customAttribute = method.GetCustomAttributes()
                  .OfType<SomeAttribute>()
                  .SingleOrDefault();

                if (customAttribute is null)
                {
                  return Enumerable.Empty<ScriptByAttribute>();
                }

                var output = new ScriptByAttribute(
                  Type: type,
                  Method: method,
                  Attribute: customAttribute);

                return [output];
              });
          })
          .ToList();
        
        
        foreach (var script in scriptsByAttributes)
        {
          var @delegate = Delegate.CreateDelegate(
            type: typeof(SomeExecutable),
            method: script.Method
          );

          var a = 5;
        }
```

It works for the first method,but not for the second one.
Here is the error I get:
```text
System.ArgumentException: Cannot bind to the target method because its signature is not compatible with that of the delegate type.
   at System.Delegate.CreateDelegate(Type type, MethodInfo method, Boolean throwOnBindFailure)
   at RevitEditorPlayground.Execution.Tests.Playground.Ho() in C:\personal\projects\revit-editor-playground\tests\RevitEditorPlayground.Execution.Tests\Playground.cs:line 73
   at InvokeStub_Playground.Ho(Object, Object, IntPtr*)
   at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)
```
And it sort of makes sense,
but it's interesting if there is a way to solve it,
so for example, if we don't pass any arguments, then the input is just null,
the same for return. Or it's not possible?
Maybe we need to use some expression trees or what?


I have an app that does compilation of C# code, here are the options I have:
```csharp
public record CompileOptions(
    Framework Framework,
    IReadOnlyList<MetadataReference> Packages,
    CSharpCompilationOptions CompilationOptions,
    CSharpParseOptions ParseOptions,
    Option<EmitOptions> EmitOptions,
    IReadOnlyList<string> GlobalUsings
);
public record Framework(FrameworkVersion Version, IReadOnlyList<MetadataReference> References);
```

So I haev a framework and packages, so I depend on MetadataReference that comes from Microsoft library.
So to execute code in isolation, I copy the those references, except the framework ones, into a separate directory
to achieve isolation, but the thing is that I don't always want to copy all references,
I need a library for compilation, but when it runs, I don't need to have it, cuz it may be provided by the host application,
and now I am thinking that maybe I should have my own type of the Reference, and then internally I will create it `MetadataReference`,
but also I maybe be able I can have different types of references, the ones that should be copied, and the ones that should not,
I don't know how to name them, do you have any suggestions?

Wait a second, so what you are saying is that so we basically have 2 types of references,
runtime reference, and compilation reference?
So the compilation is needed for the compilation time, it's just so compiler knows that these things exist,
right?
But when it actually runs it can be provided be the host application, right?
so in our case we want to be able to control more of a runtime behavior, right?



```
System.ArgumentException: Object of type 'System.Object[]' cannot be converted to type 'System.String'.
```

```csharp
public static int DoThing(string input)
{
    Console.WriteLine($"Input: {input}");
                        
    return 1;
}
```