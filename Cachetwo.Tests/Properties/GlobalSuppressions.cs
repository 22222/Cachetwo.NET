// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.

#pragma warning disable SA1117 // Parameters must be on same line or separate lines

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1028:Enum Storage should be Int32",
    Justification = "Some test cases use these")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1044:Properties should not be write only",
    Justification = "Some test cases use these")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class",
    Justification = "Doesn't matter in unit tests")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1618:Generic type parameters must be documented",
    Justification = "Doesn't matter in unit tests")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output",
    Justification = "Not necessary for unit tests")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores",
    Justification = "Not necessary for unit tests")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods",
    Justification = "Some test cases use these")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations.",
    Justification = "Doesn't matter in unit tests")]

#pragma warning restore SA1117 // Parameters must be on same line or separate lines