# PlantUmlClassDiagramGenerator.SourceGenerator

## Overview
This tool is designed to generate PlantUML class diagrams from C# source code. Leveraging SourceGenerator functionality, it analyzes the source code and produces PlantUML class diagrams.

### Beta Release
Please note that this is a beta test version, and we appreciate your feedback to help improve and refine the tool.

## Features
- SourceGenerator Integration: Utilizing SourceGenerator, this tool seamlessly integrates with the C# compilation process to automatically generate PlantUML class diagrams.
- Improved Analysis with Symbols: In contrast to the previous version, PlantUmlClassDiagramGenerator, which relied on SyntaxTree for class analysis, the SourceGenerator version utilizes Symbols for a more efficient and accurate parsing of the source code.

## Usage

### 1. Installing the NuGet Package

Retrieve the [PlantUmlClassDiagramGenerator.SourceGenerator](https://www.nuget.org/packages/PlantUmlClassDiagramGenerator.SourceGenerator) package from NuGet Gallery and install it into your .NET project.


### 2. Editing the Project File

#### 2.1 Including "GENERATE_PLANTUML" in Conditional Compilation Symbols
  
This tool operates only when the preprocessor symbol "GENERATE_PLANTUML" is defined. The tool does not need to run constantly during coding; running it once when necessary is sufficient. Therefore, it is set up to operate only during specific build configurations.
Add "GENERATE_PLANTUML" to the conditional compilation symbols of your project build configurations.

To configure the tool to run during release builds, add the following section to your .csproj file:
```xml
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <DefineConstants>$(DefineConstants);GENERATE_PLANTUML</DefineConstants>
</PropertyGroup>
```

#### 2.2 Adding Project Settings

Add the following section to your project file (.csproj):
```xml
<ItemGroup>
	<CompilerVisibleProperty Include="PlantUmlGenerator_OutputDir" />
	<CompilerVisibleProperty Include="PlantUmlGenerator_AttributeRequierd" />
	<CompilerVisibleProperty Include="PlantUmlGenerator_IncludeMemberAccessibilities" />
	<CompilerVisibleProperty Include="PlantUmlGenerator_ExcludeMemberAccessibilities" />
</ItemGroup>
<PropertyGroup>
	<PlantUmlGenerator_OutputDir>$(ProjectDir)generated-uml</PlantUmlGenerator_OutputDir>
	<PlantUmlGenerator_AttributeRequierd>false</PlantUmlGenerator_AttributeRequierd>
	<PlantUmlGenerator_IncludeMemberAccessibilities>All</PlantUmlGenerator_IncludeMemberAccessibilities>
	<PlantUmlGenerator_ExcludeMemberAccessibilities>None</PlantUmlGenerator_ExcludeMemberAccessibilities>
</PropertyGroup>
```

|Property|Description|Default|
|--|--|--|
|PlantUmlGenerator_OutputDir|Specifies the directory where the generated UML files will be placed. If you want to establish associations between different projects, specify the same directory for all related projects.|$(ProjectDir)generated_uml|
|PlantUmlGenerator_AttributeRequierd|Set to true if you want to output only types with the PlantUmlDiagramAttribute, or false if you want to output all types defined within the project.|true|
|PlantUmlGenerator_IncludeMemberAccessibilities|Specifies the accessibilities of members to include in the output. Possible values are as follows: <br>・None<br>・Public<br>・Protected<br>・Internal<br>・ProtectedInternal<br>・PrivateProtected<br>・Private<br>・All<br>You can specify multiple values separated by commas. <br>Example: Public,Protected|All|
|PlantUmlGenerator_ExcludeMemberAccessibilities|Specifies the accessibilities of members to exclude from the output. The format of specification is the same as PlantUmlGenerator_IncludeMemberAccessibilities.|None|


### 3. Setting Attribute Values
Set attribute values for types and their members as needed.

#### 3.1 PlantUmlDiagramAttribute
If the project setting `PlantUmlGenerator_AttributeRequierd` is set to true, you need to add this attribute to the types you want to include in the output.

```cs
[PlantUmlDiagram]
class ClassA
{
    public int X { get;set;}
    //....
}
```

You can also specify the accessibility of members to be displayed by setting the `IncludeMemberAccessibilities` property. This property takes precedence over the project setting (`PlantUmlGenerator_IncludeMemberAccessibilities`).

```cs
[PlantUmlDiagram(IncludeMemberAccessibilities = Accessibilities.Public| Accessibilities.Protected)]
class ClassA
{
    private int n = 0;
    public int X { get;set;}
    protected string A { get;set; }
}
```

![classA](/uml/source-generator/0302-001.svg)

Conversely, you can also specify members to be excluded from the display using the `ExcludeMemberAccessibilities` property.

```cs
[PlantUmlDiagram(ExcludeMemberAccessibilities = Accessibilities.Private)]
class ClassA
{
    private int n = 0;
    public int X { get;set;}
    protected string A { get;set; }
}
```

#### 3.2 PlantUmlIgnoreAttribute

If the project setting `PlantUmlGenerator_AttributeRequierd` is set to false, all types defined within the project will be included in the output. If you want to exclude certain types from the output, you can apply the `PlantUmlIgnoreAttribute` to those types.

```csharp
[PlantUmlIgnore]
class ClassA
{
}

class ClassB
{

}
```


![classB](/uml/source-generator/0302-002.svg)

The `PlantUmlIgnoreAttribute` can also be used to hide specific members of a type.

```cs
class ClassA
{
    public int X {get;set;}
    [PlantUmlIgnore]
    public int Y {get;set;}

    public void MethodA(){}
    [PlantUmlIgnore]
    public void MethodB(){}
}
```


![classA](/uml/source-generator/0302-003.svg)

## Specification

### Output of UML Files
Under the directory specified by `PlantUmlGenerator_OutputDir`, folders named after the "Assembly Name" will be created, and within each of these folders, folders named after the "Namespace" will be created.

Example:
Consider a solution with the following project structure:

|Project|Assembly Name|Namespace|
|--|--|--|
|Consoto.App1.csproj|Consoto.App1|Consoto.App1<br>Consoto.App1.ViewModel<br>Consoto.App1.Model|
|Consoto.App1.Core.csproj|Consoto.App1.Core|Consoto.App1.Core<br>Consoto.App1.Core.Extensions|

For each project, if the `PlantUmlClassDiagramGenerator.SourceGenerator` package is installed, and `PlantUmlGenerator_OutputDir` is set to `$(SolutionDir)generated_uml`, UML files will be outputted following the folder structure illustrated below:

![dir](/uml/source-generator/0302-004.svg)

### Representation of Types
The keywords for types available in PlantUML are as follows:
- class
- struct
- interface
- abstract class
- enum
  
Modifiers such as `record`, `static`, and `sealed` are represented using stereotypes (`<<keyword>>`).


<details><summary>C#</summary>

```cs
class ClassA
{
    public string Name { get; }
    public int Value { get; }
    public ClassA(string name, int value) => (Name, Value) = (name, value);
}

static class StaticClass
{
    public static string SpecificName = "Hoge";
    public static string Piyo(int count) => string.Join(" ", Enumerable.Repeat("Pyyo", count));
}

abstract class AbstractClass
{
    public abstract void MethodA();
    public abstract void MethodB();
}

record RecordA(string Name,int Value);

public struct StructA()
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public record struct RecordStruct(float X, float Y, float Z);

public enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}

[Flags]
enum Accessibilities
{
    None = 0,
    Public = 0x01,
    Protected = 0x02,
    Internal = 0x04,
    ProtectedInternal = 0x08,
    PrivateProtected = 0x10,
    Private = 0x20,
    All = Public | Protected | Internal | ProtectedInternal | PrivateProtected | Private
}
```
</details>

<details><summary>PlantUml</summary>

```
class ClassA  {
    + <<readonly>> Name : string <<get>>
    + <<readonly>> Value : int <<get>>
    + ClassA(name : string, value : int)
}
class StaticClass <<static>>  {
    + {static} SpecificName : string
    + {static} Piyo(count : int) : string
    - {static} StaticClass()
}
abstract class AbstractClass  {
    + {abstract} MethodA() : void
    + {abstract} MethodB() : void
    # AbstractClass()
}
class RecordA <<record>>  {
    + RecordA(Name : string, Value : int)
    # <<readonly>> <<virtual>> EqualityContract : Type <<get>>
    + Name : string <<get>> <<set>>
    + Value : int <<get>> <<set>>
    + <<override>> ToString() : string
    # <<virtual>> PrintMembers(builder : StringBuilder) : bool
    + {static} operator !=(left : RecordA?, right : RecordA?) : bool
    + {static} operator ==(left : RecordA?, right : RecordA?) : bool
    + <<override>> GetHashCode() : int
    + <<override>> Equals(obj : object?) : bool
    + <<virtual>> Equals(other : RecordA?) : bool
    # RecordA(original : RecordA)
    + Deconstruct(Name : string, Value : int) : void
}
struct StructA <<sealed>>  {
    + StructA()
    + X : float <<get>> <<set>>
    + Y : float <<get>> <<set>>
    + Z : float <<get>> <<set>>
}
struct RecordStruct <<sealed>> <<record>>  {
    + RecordStruct(X : float, Y : float, Z : float)
    + X : float <<get>> <<set>>
    + Y : float <<get>> <<set>>
    + Z : float <<get>> <<set>>
    + <<readonly>> <<override>> ToString() : string
    - <<readonly>> PrintMembers(builder : StringBuilder) : bool
    + {static} operator !=(left : RecordStruct, right : RecordStruct) : bool
    + {static} operator ==(left : RecordStruct, right : RecordStruct) : bool
    + <<readonly>> <<override>> GetHashCode() : int
    + <<readonly>> <<override>> Equals(obj : object) : bool
    + <<readonly>> Equals(other : RecordStruct) : bool
    + <<readonly>> Deconstruct(X : float, Y : float, Z : float) : void
    + RecordStruct()
}
enum LogLevel <<sealed>>  {
    Trace = 0
    Debug = 1
    Info = 2
    Warn = 3
    Error = 4
    Fatal = 5
    + LogLevel()
}
enum Accessibilities <<Flags>> <<sealed>>  {
    None = 0
    Public = 1
    Protected = 2
    Internal = 4
    ProtectedInternal = 8
    PrivateProtected = 16
    Private = 32
    All = Public | Protected | Internal | ProtectedInternal | PrivateProtected | Private
    + Accessibilities()
}
```

</details>

![types](/uml/source-generator/0302-005.svg)


### Associations
Associations between one type and another are added beneath each individual type definition. The conditions for creating associations and the types of associations to apply are as follows.

#### Inheritance
If a type inherits from another type other than Object, ValueType, or Struct, an `Inheritance (<|--)` association is added.
```cs
public abstract class PlanetBase
{
}

public class Earth : PlanetBase
{
}
```

```
abstract class PlanetBase {
}

class Earth {
}

PlanetBase <|-- Earth
```

![PlanetBase](/uml/source-generator/0302-006.svg)

#### Interface Implementation
If a type implements an interface, a `Realization (<|..)` association is added.


```cs
interface ILogger 
{
}
class Logger : ILogger
{
}
```

```
interface ILogger {
}

class Logger {
}

ILogger <|.. Logger
```

![Logger](/uml/source-generator/0302-007.svg)

#### Property or Field Association
If the type of a property or field is the type being output, an `Aggregation (o--)` or `Composition (*--)` association is added.

The Composition association is applied if:
- The property or field is initialized in the initializer.
- It is initialized within the constructor.
  
```cs
class Class1
{
    public Item ItemA {get;} = new Item();  //Initialized in the initializer
    public Item ItemB {get;}
    public Item ItemC {get;}
    public Class1(Item item) 
    {
        ItemB = new Item();  //Initialized in the constructor
        ItemC = item;        //Injected from outside
    }
}
```

```
class Class1 {
    + <<readonly>> ItemA : Item <<get>>
    + <<readonly>> ItemB : Item <<get>>
    + <<readonly>> ItemC : Item <<get>>
    + Class1(item : Item)
}
Class1 *-- Item : ItemA
Class1 *-- Item : ItemB
Class1 o-- Item : ItemC
Class1 ..> Item
```

![Composition](/uml/source-generator/0302-008.svg)


##### For Types Implementing Array Types or `IEnumerable<T>`
For types implementing array types or `IEnumerable<T>`, the association is made with the element type instead of the property or field type. In this case, a multiplicity of `"*"` is added to the element type.

```cs
public abstract class PlanetBase(string name)
{
    public string Name { get; set; } = name;
    public IList<Moon> Moons { get; } = new List<Moon>();
    protected void AddMoon(Moon moon)
    {
        Moons.Add(moon);
    }
}
```
```
abstract class PlanetBase  {
    # PlanetBase(name : string)
    + Name : string <<get>> <<set>>
    + <<readonly>> Moons : IList<Moon> <<get>>
    # AddMoon(moon : Moon) : void
}
PlanetBase *-- "*" Moon : Moons
PlanetBase ..> Moon
```

![PlanetBase_Moon](/uml/source-generator/0302-009.svg)


#### Method Parameters
If the type of a method parameter is the type being output, a `Dependency (..>)` association is added.

```cs
class Parameters
{
   public int A {get;set;}
   public int B {get;set;}
}
class ClassA
{
    public void Execute(Parameters parameters)
    {
       Console.WriteLine($"({parameters.A},{parameters.B})");
    }
}
```

![Parameters](/uml/source-generator/0302-010.svg)


#### Nested Types
If a member contains a type definition, a `Nested (+--)` association is added. The name of the nested type follows the format `{ParentTypeName}::{TypeName}`.

<details><summary>C#</summary>

```cs
class Parent
{
    class ChiledA
    {
        class GrandchildA
        {
            class GreatGrandchild
            {

            }
        }
        class GrandchildB
        {
            class GreatGrandchild
            {

            }
        }
    }
    class ChildeB
    {
        class GrandchildA
        {
            class GreatGrandchild
            {
            }
        }
        class GrandchildB
        {

        }
    }
}
```

</details>

<details><summary>PlantUML</summary>

```
class Parent::ChiledA::GrandchildA::GreatGrandchild  {
    + GreatGrandchild()
}

class Parent::ChiledA::GrandchildB::GreatGrandchild  {
    + GreatGrandchild()
}

class Parent::ChiledA::GrandchildB  {
    + GrandchildB()
}
Parent::ChiledA::GrandchildB +.. Parent::ChiledA::GrandchildB::GreatGrandchild

class Parent::ChiledA::GrandchildA  {
    + GrandchildA()
}
Parent::ChiledA::GrandchildA +.. Parent::ChiledA::GrandchildA::GreatGrandchild

class Parent::ChiledA  {
    + ChiledA()
}
Parent::ChiledA +.. Parent::ChiledA::GrandchildA
Parent::ChiledA +.. Parent::ChiledA::GrandchildB

class Parent::ChildeB::GrandchildA::GreatGrandchild  {
    + GreatGrandchild()
}

class Parent::ChildeB::GrandchildA  {
    + GrandchildA()
}
Parent::ChildeB::GrandchildA +.. Parent::ChildeB::GrandchildA::GreatGrandchild

class Parent::ChildeB::GrandchildB  {
    + GrandchildB()
}

class Parent::ChildeB  {
    + ChildeB()
}
Parent::ChildeB +.. Parent::ChildeB::GrandchildA
Parent::ChildeB +.. Parent::ChildeB::GrandchildB

class Parent  {
    + Parent()
}
Parent +.. Parent::ChiledA
Parent +.. Parent::ChildeB

```

</details>

![Nested_Type](/uml/source-generator/0302-011.svg)

### File Reference
As each association is added, the `!include` directive is also added to reference the definitions of associated types.

If the same directory is specified in the project setting `PlantUmlGenerator_OutputDir`, files from other projects will be included following the folder structure rules, even if they are from different projects.

Example:

```cs
//Assembly: Consoto.App1.Core
namespace Consoto.App1.Core;
class Parameters
{
}
```

```cs
//Assembly: Consoto.App1
namespace Consoto.App1;
class ClassA
{
    public void Run(Parameter parameters)
    {
        //...
    }
}
```

If class definitions exist in the assembly and namespaces as shown above, the UML files will be output with the following folder structure:

![Directory](/uml/source-generator/0302-012.svg)

When creating the UML file for `ClassA`, if there is a relationship with `Parameters`, the `Parameters.puml` file in the output directory is searched.
If the file exists, the relationship and `!include` are added. The file path is relative to `ClassA.puml`.

```
@startuml ClassA
!include ../../../../Consoto.App1.Core/Consoto/App1/Core/Parameters.puml
class ClassA {
    + Run(parameters : Parameters) : void 
}
ClassA ..> Parameters
@enduml
```

## Output Example
The example below shows the output structure of a single file:
- `!include` for associated classes
- Class definitions
- Relationship definitions

<details><summary>C#</summary>

```cs
namespace SourceGeneratorTest.Planets.BaseTypes;
public abstract class PlanetBase(string name)
{
    public string Name { get; set; } = name;
    public double Diameter { get; protected set; }
    public double Mass { get; protected set; }
    public double DistanceFromSun { get; protected set; }
    public double OrbitalPeriod { get; protected set; }
    public double SurfaceTemperature { get; protected set; }
    public IList<Moon> Moons { get; } = new List<Moon>();

    protected void AddMoon(Moon moon)
    {
        Moons.Add(moon);
    }
    public abstract Task Orbit();
    public abstract Task Rotate();
}
```

```cs
namespace SourceGeneratorTest.Planets;
public class Earth : PlanetBase
{
    public Earth() : base("Earth")
    {
        Diameter = 12742;
        Mass = 5.972e24;
        DistanceFromSun = 149.6e6;
        OrbitalPeriod = 365.26;
        SurfaceTemperature = 288;
        AddMoon(new Moon("Moon", 3474, 7.35e22, 384400));
    }

    public override async Task Orbit()
    {
        await Task.Delay(5000);
    }

    public override async Task Rotate()
    {
        await Task.Delay(5000);
    }
}
```

```cs
namespace SourceGeneratorTest.Planets;
public sealed class Moon(string name, double diameter, double mass, double distanceFromPlanet)
{
    public string Name { get; } = name;
    public double Diameter { get; } = diameter;
    public double Mass { get; } = mass;
    public double DistanceFromPlanet { get; } = distanceFromPlanet;
}
```

</details>

<details><summary>PlantUML</summary>

```
@startuml PlanetBase
!include ../Moon.puml
abstract class PlanetBase  {
    # PlanetBase(name : string)
    + Name : string <<get>> <<set>>
    + Diameter : double <<get>> <<protected set>>
    + Mass : double <<get>> <<protected set>>
    + DistanceFromSun : double <<get>> <<protected set>>
    + OrbitalPeriod : double <<get>> <<protected set>>
    + SurfaceTemperature : double <<get>> <<protected set>>
    + <<readonly>> Moons : IList<Moon> <<get>>
    # AddMoon(moon : Moon) : void
    + {abstract} Orbit() : Task
    + {abstract} Rotate() : Task
}
PlanetBase *-- "*" Moon : Moons
PlanetBase ..> Moon
@enduml
```
```
@startuml Earth
!include BaseTypes/PlanetBase.puml
class Earth  {
    + Earth()
    + <<override>> <<async>> Orbit() : Task
    + <<override>> <<async>> Rotate() : Task
}
PlanetBase <|-- Earth
@enduml
```

```
@startuml Moon
class Moon <<sealed>>  {
    + Moon(name : string, diameter : double, mass : double, distanceFromPlanet : double)
    + <<readonly>> Name : string <<get>>
    + <<readonly>> Diameter : double <<get>>
    + <<readonly>> Mass : double <<get>>
    + <<readonly>> DistanceFromPlanet : double <<get>>
}
@enduml
```

</details>

![Example1](/uml/source-generator/0302-013.svg)

<details><summary>C#</summary>

```cs
namespace SourceGeneratorTest.Classes;
internal class SampleModel
{
    private readonly ILogger logger;
    private readonly IList<StructA> structures;

    public IReadOnlyList<Item> Items { get; } = new List<Item>();

    public SampleModel(ILogger logger, IList<StructA> structures)
    {
        this.logger = logger;
        this.structures = structures;
    }

    public async ValueTask Execute(Parameters parameters)
    {
        await Task.Delay(1000);
    }
}
```

```cs
namespace SourceGeneratorTest.Classes;
internal class Logger : ILogger
{
    public void Write(string message, LogLevel logLevel, Exception? exception = null)
    {
        Console.WriteLine($"[{logLevel}] {message}");
        if (exception != null)
        {
            Console.WriteLine(exception.Message);
        }
    }
    public void WriteDebug(string message) => Write(message, LogLevel.Debug);
    public void WriteInfo(string message) => Write(message, LogLevel.Info);
    public void WriteTrace(string message) => Write(message, LogLevel.Trace);
    public void WriteWarn(string message) => Write(message, LogLevel.Warn);
    public void WriteError(string message, Exception? exception = null) => Write(message, LogLevel.Error, exception);
    public void WriteFatal(string message, Exception? exception = null) => Write(message, LogLevel.Fatal, exception);
}
```

```cs
namespace SourceGeneratorTest.Library.Logs;
public interface ILogger
{
    void Write(string message, LogLevel logLevel, Exception exception);
    void WriteTrace(string message);
    void WriteDebug(string message);
    void WriteInfo(string message);
    void WriteWarn(string message);
    void WriteError(string message, Exception exception);
    void WriteFatal(string message, Exception exception);
}

public enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}
```
```cs
namespace SourceGeneratorTest.Library.Types;
public struct StructA()
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public record Item(string Name, double Value);

public record Parameters
{
    public int X { get; }
    public int Y { get; }
    public Parameters(int x, int y) => (X, Y) = (x, y);

    public int Area() => X * Y;
}
```

</details>

<details><summary>PlantUML</summary>

```
@startuml SampleModel
!include ../../../SourceGeneratorTest.Library/SourceGeneratorTest/Library/Logs/ILogger.puml
!include ../../../SourceGeneratorTest.Library/SourceGeneratorTest/Library/Types/StructA.puml
!include ../../../SourceGeneratorTest.Library/SourceGeneratorTest/Library/Types/Item.puml
!include ../../../SourceGeneratorTest.Library/SourceGeneratorTest/Library/Types/Parameters.puml
class SampleModel  {
    - <<readonly>> logger : ILogger
    - <<readonly>> structures : IList<StructA>
    + <<readonly>> Items : IReadOnlyList<Item> <<get>>
    + SampleModel(logger : ILogger, structures : IList<StructA>)
    + <<async>> Execute(parameters : Parameters) : ValueTask
}
SampleModel o-- ILogger : logger
SampleModel o-- "*" StructA : structures
SampleModel *-- "*" Item : Items
SampleModel ..> ILogger
SampleModel ..> Parameters
@enduml
```

```
@startuml Logger
!include ../../../SourceGeneratorTest.Library/SourceGeneratorTest/Library/Logs/ILogger.puml
!include ../../../SourceGeneratorTest.Library/SourceGeneratorTest/Library/Logs/LogLevel.puml
class Logger  {
    + Write(message : string, logLevel : LogLevel, exception : Exception?) : void
    + WriteDebug(message : string) : void
    + WriteInfo(message : string) : void
    + WriteTrace(message : string) : void
    + WriteWarn(message : string) : void
    + WriteError(message : string, exception : Exception?) : void
    + WriteFatal(message : string, exception : Exception?) : void
    + Logger()
}
ILogger <|.. Logger
Logger ..> LogLevel
@enduml
```

```
@startuml ILogger
!include ./LogLevel.puml
interface ILogger  {
    + Write(message : string, logLevel : LogLevel, exception : Exception) : void
    + WriteTrace(message : string) : void
    + WriteDebug(message : string) : void
    + WriteInfo(message : string) : void
    + WriteWarn(message : string) : void
    + WriteError(message : string, exception : Exception) : void
    + WriteFatal(message : string, exception : Exception) : void
}
ILogger ..> LogLevel
@enduml
```

```
@startuml LogLevel
enum LogLevel <<sealed>>  {
    Trace = 0
    Debug = 1
    Info = 2
    Warn = 3
    Error = 4
    Fatal = 5
    + LogLevel()
}
@enduml
```

```
@startuml Item
class Item <<record>>  {
    + Item(Name : string, Value : double)
    # <<readonly>> <<virtual>> EqualityContract : Type <<get>>
    + Name : string <<get>> <<set>>
    + Value : double <<get>> <<set>>
    + <<override>> ToString() : string
    # <<virtual>> PrintMembers(builder : StringBuilder) : bool
    + {static} operator !=(left : Item?, right : Item?) : bool
    + {static} operator ==(left : Item?, right : Item?) : bool
    + <<override>> GetHashCode() : int
    + <<override>> Equals(obj : object?) : bool
    + <<virtual>> Equals(other : Item?) : bool
    # Item(original : Item)
    + Deconstruct(Name : string, Value : double) : void
}
"IEquatable`1" "<Item>" <|.. Item
@enduml
```

```
@startuml StructA
struct StructA <<sealed>>  {
    + StructA()
    + X : float <<get>> <<set>>
    + Y : float <<get>> <<set>>
    + Z : float <<get>> <<set>>
}
@enduml
```

```
@startuml Parameters
class Parameters <<record>>  {
    # <<readonly>> <<virtual>> EqualityContract : Type <<get>>
    + <<readonly>> X : int <<get>>
    + <<readonly>> Y : int <<get>>
    + Parameters(x : int, y : int)
    + Area() : int
    + <<override>> ToString() : string
    # <<virtual>> PrintMembers(builder : StringBuilder) : bool
    + {static} operator !=(left : Parameters?, right : Parameters?) : bool
    + {static} operator ==(left : Parameters?, right : Parameters?) : bool
    + <<override>> GetHashCode() : int
    + <<override>> Equals(obj : object?) : bool
    + <<virtual>> Equals(other : Parameters?) : bool
    # Parameters(original : Parameters)
}
"IEquatable`1" "<Parameters>" <|.. Parameters
@enduml
```
</details>

![Example2](/uml/source-generator/0302-014.svg)

## Release Note
### [0.5.1-beta]

1. Added the following csproj properties:
  - OutputDir: Specifies the output directory.
  - AttributeRequired: A flag to indicate whether attributes are required. When set to false, classes without the PlantUmlDiagram attribute will be included in the output.
  - IncludeMemberAccessibilities: Specifies the accessibility of members to include in the output.
  - ExcludeMemberAccessibilities: Specifies the accessibility of members to exclude from the output.
2. Added PlantUmlIgnore attribute:  
  This attribute can be applied to types, properties, methods, etc., to exclude them from the output.
3. Changed the output folder structure:  
  Created folders based on namespaces and placed PlantUML files within them.

### [0.1.9-alpha]

- Nested Class Association:  
  It is possible to associate nested classes. 
- Event Member Output:  
  The capability to output event members has been added. 
- Composition Representation for Initialized Properties and Fields:  
  When properties or fields are initialized within an initializer or constructor, their associated types are now expressed using composition. 

### [0.1.8-alpha]
- Alpha Test Release

## ToDo
- [x] Differentiation between Aggregation and Composition. If initialized with new () in constructors or property initializers, is it Composition?
- [x] Representation of nested classes.
- [x] Control visibility of members based on access modifiers.
- [ ] Treatment of type access modifiers. Currently not supported.
- [ ] Hide automatically implemented methods for record types.


## Feedback
As an beta tester, your input is invaluable in shaping the future of this tool. Please share your thoughts, report bugs, and suggest enhancements on our [discussions page](https://github.com/pierre3/PlantUmlClassDiagramGenerator/discussions).
