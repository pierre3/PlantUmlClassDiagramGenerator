namespace PlantUmlClassDiagramGenerator.SourceGenerator.Options;

[Flags]
internal enum Accessibilities
{
    NotSet = 0x8000,
    None = 0,
    Public = 0x01,
    Protected = 0x02,
    Internal = 0x04,
    ProtectedInternal = 0x08,
    PrivateProtected = 0x10,
    Private = 0x20,
    All = Public | Protected | Internal | ProtectedInternal | PrivateProtected | Private
}
