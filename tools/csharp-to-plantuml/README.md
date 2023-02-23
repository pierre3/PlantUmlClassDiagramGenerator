# csharp-to-plantuml README

Create class diagrams of PlantUML from C# source code.

## Requirements
- [.NET Core 6.0 Runtime](https://dotnet.microsoft.com/download/dotnet-core/6.0/runtime) 

## Extension Settings

- __csharp2plantuml.inputPath__
  Specify a input folder (relative to workspace folder)
- __csharp2plantuml.outputPath__
  Specify a output folder (relative to workspace folder)
- __csharp2plantuml.public__
  Only public accessibility members are output.
- __csharp2plantuml.ignoreAccessibility__
  Specify accessibiliies of members to ignore, with a comma separated list. (ex. 'private,protected,protected internal')
- __csharp2plantuml.excludePath__
  Specify exclude file or directory paths (relative to the \"InputPath\"), with a comma separated list. (ex. 'obj,Properties\\AssemblyInfo.cs')
- __csharp2plantuml.createAssociation__
  Create object associations from references of fields and properites.
- __csharp2plantuml.allInOne__
  Copy the output of all diagrams to file include.puml (this allows a PlanUMLServer to render it).
- __csharp2plantuml.attributeRequired__
  When this switch is enabled, only types with "PlantUmlDiagramAttribute" in the type declaration will be output.
- __csharp2plantuml.excludeUmlBeginEndTags__
  When this switch is enabled, it will exclude the \"@startuml\" and \"@enduml\" tags from the puml file.

## Known Issues


## Release Notes
## 1.3.2
- Add a switch to exclude "@startuml" and "@enduml" tags from the puml file. 
- Fixed to add "- "(private) as default modifier when access modifier is omitted.

## 1.3.1
- Fix an issue in which escape characters set in attribute parameters were not handled correctly  
  https://github.com/pierre3/PlantUmlClassDiagramGenerator/issues/62
  
## 1.3.0
- Add attribute-based configuration
  
## 1.2.5
- Update to .NET6.0
- Supports a record declaration
- Fix excludePaths from skipping incorrectly  
  https://github.com/pierre3/PlantUmlClassDiagramGenerator/pull/54

### 1.2.4
- Updated to .NET5.0

### 1.2.3
- Fixed Issue: 
    - https://github.com/pierre3/PlantUmlClassDiagramGenerator/issues/34

### 1.2.2
- Updated to .Net Core 3.1.
- Fixed Issue: 
    - https://github.com/pierre3/PlantUmlClassDiagramGenerator/issues/32
    - https://github.com/pierre3/PlantUmlClassDiagramGenerator/issues/26

### 1.2.1  
- Updated to .Net Core 3.0.
- Add Feature that allows for Nullable (?) type syntax.
- Add all-in-one option.

### 1.0.0
Add Feature that create object associations from references of fields and properties. 

### 0.0.1 preview
Beta release.
