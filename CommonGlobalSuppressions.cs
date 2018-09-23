// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.

#pragma warning disable SA1117 // Parameters must be on same line or separate lines

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix",
    Justification = "Tries to force a Collection suffix on Dictionaries")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Do not directly await a Task",
    Justification = "The Microsoft.Extensions.Caching library doesn't using ConfigureAwait(false) so we won't either")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis must be spaced correctly",
    Justification = "StyleCop doesn't understand C# 7 tuple return types yet (https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2308)")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis must be spaced correctly",
    Justification = "StyleCop doesn't understand C# 7 tuple return types yet (https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2308)")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this",
    Justification = "Disagree")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis must be on line of last parameter",
    Justification = "Makes things less readable in some cases")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:Do not use regions",
    Justification = "Regions are great")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:Using directives must be placed correctly",
    Justification = "Microsoft source templates use the other style")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements must appear in the correct order",
    Justification = "Disagree")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements must be ordered by access",
    Justification = "Disagree")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1208:System using directives must be placed before other using directives",
    Justification = "Prefer pure alphabetical ordering")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces must not be omitted",
    Justification = "For inline checks that throw exceptions, like null checks")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace must be followed by blank line",
    Justification = "Disagree")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:File must have header",
    Justification = "Headers are stupid")]

#pragma warning restore SA1117 // Parameters must be on same line or separate lines