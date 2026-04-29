using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MappingGenerator;

internal static class CheckForGeneratorAttributeUsages
{
    internal static IncrementalValuesProvider<List<ClassModel>> Run(IncrementalGeneratorInitializationContext incrementalGeneratorInitializationContext)
    {
        IncrementalValuesProvider<List<ClassModel>> incrementalValuesProvider = incrementalGeneratorInitializationContext.SyntaxProvider.CreateSyntaxProvider((syntaxNode, _) =>
        {
            if (syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax)
            {
                return recordDeclarationSyntax.IsKind(SyntaxKind.RecordDeclaration);
            }

            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                return classDeclarationSyntax.IsKind(SyntaxKind.ClassDeclaration);
            }

            return false;
        }, (generatorSyntaxContext, _) =>
        {
            return GenerateClassModelFromSyntaxContext.Run(generatorSyntaxContext);
        }).Where(classModel => classModel is not null);

        return incrementalValuesProvider;
    }
}
