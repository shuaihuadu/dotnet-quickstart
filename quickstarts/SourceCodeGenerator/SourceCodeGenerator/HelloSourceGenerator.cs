﻿using Microsoft.CodeAnalysis;

namespace SourceCodeGenerator
{
    [Generator]
    public class HelloSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            IMethodSymbol mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);

            string source = $@"// <auto-generated />
            using System;

            namespace {mainMethod.ContainingNamespace.ToDisplayString()}
            {{
                public static partial class {mainMethod.ContainingType.Name}
                {{
                    static partial void HelloFrom(string name) => Console.WriteLine($""Generator says: Hi from '{{name}}'"");
                }}
            }}";

            string typeName = mainMethod.ContainingType.Name;

            context.AddSource($"{typeName}.g.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}