using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MappingGenerator;

internal static class GenerateClassModelFromSyntaxContext
{
    internal static List<ClassModel> Run(GeneratorSyntaxContext generatorSyntaxContext)
    {
        List<ClassModel> classes = [];
        ClassDeclarationTypes classDeclarationType = ClassDeclarationTypes.Undefined;
        ClassDeclarationSyntax? classDeclarationSyntax = generatorSyntaxContext.Node as ClassDeclarationSyntax;
        RecordDeclarationSyntax? recordDeclarationSyntax = generatorSyntaxContext.Node as RecordDeclarationSyntax;
        INamedTypeSymbol? targetTypeSymbol = null;

        if (classDeclarationSyntax is not null)
        {
            targetTypeSymbol = generatorSyntaxContext.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
            classDeclarationType = ClassDeclarationTypes.Class;
        }
        else if (recordDeclarationSyntax is not null)
        {
            targetTypeSymbol = generatorSyntaxContext.SemanticModel.GetDeclaredSymbol(recordDeclarationSyntax) as INamedTypeSymbol;
            classDeclarationType = ClassDeclarationTypes.Record;
        }
        
        if (classDeclarationType is ClassDeclarationTypes.Undefined || targetTypeSymbol is null)
        {
            return [];
        }

        foreach (AttributeData attributeData in targetTypeSymbol.GetAttributes().Where(attributeData => attributeData.AttributeClass?.Name == CodeGenerator.MapperAttributeName || attributeData.AttributeClass?.Name == $"{CodeGenerator.MapperAttributeName}Attribute"))
        {
            if (attributeData.ConstructorArguments.Length != 1 || attributeData.ConstructorArguments[0].Value is not INamedTypeSymbol sourceTypeSymbol)
            {
                continue;
            }

            ClassModel classModel = new(classDeclarationType, sourceTypeSymbol.ContainingNamespace.ToDisplayString(), sourceTypeSymbol.Name, targetTypeSymbol.ContainingNamespace.ToDisplayString(), targetTypeSymbol.Name);
            GetSerializableProperties.Run(sourceTypeSymbol, targetTypeSymbol, ref classModel);
            classes.Add(classModel);
        }
        
        return classes;
    }
}
