namespace PlantUmlClassDiagramGenerator.SourceGenerator.Options;

[Flags]
internal enum AssociationTypes
{
    NotSet = 0x8000,
    None = 0,
    Inheritance = 0x01,
    Realization = 0x02,
    Property = 0x04,
    Field = 0x08,
    MethodParameter = 0x10,
    Nest = 0x20,
    All = Inheritance | Realization | Property | Field | MethodParameter | Nest
}
