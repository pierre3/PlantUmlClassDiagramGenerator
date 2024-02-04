# PlantUmlClassDiagramGenerator.SourceGenerator

## Overview
This tool is designed to generate PlantUML class diagrams from C# source code. Leveraging SourceGenerator functionality, it analyzes the source code and produces PlantUML class diagrams.

### Alpha Release
Please note that this is an alpha test version, and we appreciate your feedback to help improve and refine the tool.

## Features
- SourceGenerator Integration: Utilizing SourceGenerator, this tool seamlessly integrates with the C# compilation process to automatically generate PlantUML class diagrams.
- Improved Analysis with Symbols: In contrast to the previous version, PlantUmlClassDiagramGenerator, which relied on SyntaxTree for class analysis, the SourceGenerator version utilizes Symbols for a more efficient and accurate parsing of the source code.

## How to Use

### 1. Installing the NuGet Package
Obtain the [PlantUmlClassDiagramGenerator.SourceGenerator](https://www.nuget.org/packages/PlantUmlClassDiagramGenerator.SourceGenerator) package from NuGet Gallery and install it in your .NET project.

### 2. Include the "GENERATE_PLANTUML" Conditional Compilation Symbol
  
This tool operates only when the preprocessor symbol "GENERATE_PLANTUML" is defined. The tool does not need to run continuously during coding; it is sufficient to execute it once when necessary. Therefore, it is structured to work only during specific build configurations.

Add "GENERATE_PLANTUML" to the conditional compilation symbols in the project's build configuration.

To configure the tool to run during release builds, add the following section to the .csproj file:

```xml
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <DefineConstants>$(DefineConstants);GENERATE_PLANTUML</DefineConstants>
</PropertyGroup>
```

### 3. Apply the "PlantUmlDiagram" Attribute to Classes

Apply the "PlantUmlDiagram" attribute to classes, structures, etc., for which you want to output class diagrams.

```cs
using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

[PlantUmlDiagram]
class ClassA 
{
  //...
}
```

#### 4. Generating Class Diagrams
The class diagrams will be generated in the `generated-uml` folder under the project directory.

- One file is generated for each class.
- The filename is `{ClassName}.puml`.

## Specifications (Temporary)

### C# Sample Code
Examine how the source code described in [InputClasses.cs](https://github.com/pierre3/PlantUmlClassDiagramGenerator/tree/feature/source-generator/test/SourceGeneratorTest/Classes/InputClasses.cs) is converted into class diagrams.

### Type Representation
Keywords for types available in PlantUML are as follows:
- class
- struct
- interface
- abstract class
- enum

Modifiers like `record`, `static`, `sealed` are represented using stereotypes (`<<keyword>>`).

- class

```
class Item  {
    + Name : string <<get>> <<set>>
    + Value : Vector <<get>> <<set>>
    + Item() : void
}
```

- struct

```
struct Point <<sealed>>  {
    + Point(x : int, y : int) : void
    + X : int <<get>> <<set>>
    + Y : int <<get>> <<set>>
    + Point() : void
}
```

- interface
```
interface IInterface  {
    + MethodA(parameters : Paramters) : void
    + MethodB(value : int) : int
}
```

- abstract class

```
abstract class BaseClass`2<T1, T2>  {
    + {abstract} <<readonly>> Name : T1 <<get>>
    + {abstract} <<readonly>> Value : T2 <<get>>
    + {abstract} GetNameValue() : string
    # BaseClass() : void
}
```

- enum
```
enum LogLevel <<sealed>>  {
    Trace = 0
    Debug = 1
    Info = 2
    Warning = 3
    Error = 4
    + LogLevel() : void
}
enum ItemFlags <<Flags>> <<sealed>>  {
    None = 0
    Alpha = 1
    Beta = 2
    Gamma = 4
    Delta = 8
    + ItemFlags() : void
}
```

- record


```
class Paramters <<record>>  {
    + Paramters(ParamA : string, ParamB : string, ParamC : int, ParamD : int) : void
    # <<readonly>> <<virtual>> EqualityContract : Type <<get>>
    + ParamA : string <<get>> <<set>>
    + ParamB : string <<get>> <<set>>
    + ParamC : int <<get>> <<set>>
    + ParamD : int <<get>> <<set>>
    + <<override>> ToString() : string
    # <<virtual>> PrintMembers(builder : StringBuilder) : bool
    + {static} operator !=(left : Paramters?, right : Paramters?) : bool
    + {static} operator ==(left : Paramters?, right : Paramters?) : bool
    + <<override>> GetHashCode() : int
    + <<override>> Equals(obj : object?) : bool
    + <<virtual>> Equals(other : Paramters?) : bool
    # Paramters(original : Paramters) : void
    + Deconstruct(ParamA : string, ParamB : string, ParamC : int, ParamD : int) : void
}
struct Vector <<sealed>> <<readonly>> <<record>>  {
    + Vector(X : double, Y : double, Z : double) : void
    + <<readonly>> X : double <<get>>
    + <<readonly>> Y : double <<get>>
    + <<readonly>> Z : double <<get>>
    + <<readonly>> Norm() : double
    + <<readonly>> <<override>> ToString() : string
    - <<readonly>> PrintMembers(builder : StringBuilder) : bool
    + {static} operator !=(left : Vector, right : Vector) : bool
    + {static} operator ==(left : Vector, right : Vector) : bool
    + <<readonly>> <<override>> GetHashCode() : int
    + <<readonly>> <<override>> Equals(obj : object) : bool
    + <<readonly>> Equals(other : Vector) : bool
    + <<readonly>> Deconstruct(X : double, Y : double, Z : double) : void
    + Vector() : void
}
```

### Associations
Associations between types are added under each individual type definition. The conditions for association and the types of associations to be added are as follows.

#### Inheritance
If the target type inherits from a type other than Object, ValueType, or Struct, add the `Inheritance (<|--)` association.
```
"BaseClass`2" "<String, Int32>" <|-- DerivedClass
```

#### Implementation of Interfaces
If the target type implements an interface, add the `Realization (<|..)` association.
```
IInterface <|.. DerivedClass
```

#### Members of Properties or Fields
If the type defines properties or fields whose types are marked with the `PlantUmlDiagram` attribute, add the `Aggregation (o--)` association.


```
DerivedClass o-- "ILogger`1" : Logger
```

For types implementing array types or `IEnumerable<T>`, add a relationship with the element type instead of the property or field type. In this case, assign multiplicity `"*"` to the element type.

```
DerivedClass o-- "*" Item : Item1  //Item1: IList<Item>
DerivedClass o-- "*" Item : Item2  //Item2: Item[]
```

#### Method Parameters
If the type defines methods with parameters whose types are marked with the `PlantUmlDiagram` attribute, add the `Dependency (..>) ` association.

```
DerivedClass ..> Paramters
```

#### File Reference
Simultaneously with adding each association, add the `!include` directive to reference the type definition of the associated counterpart.

```
!include Parameters.puml
...
DerivedClass ..> Paramters
```

#### Example Output

```
@startuml DerivedClass
!include BaseClass`2.puml
!include IInterface.puml
!include ILogger`1.puml
!include Item.puml
!include Paramters.puml
class DerivedClass  {
    - <<readonly>> Logger : ILogger<DerivedClass> <<get>>
    + <<readonly>> <<override>> Name : string <<get>>
    + <<readonly>> <<override>> Value : int <<get>>
    + Item1 : IList<Item> <<get>> <<set>>
    + Item2 : Item[] <<get>> <<set>>
    + DerivedClass(logger : ILogger<DerivedClass>) : void
    + <<override>> GetNameValue() : string
    + MethodA(parameters : Paramters) : void
    + MethodB(value : int) : int
    + Process(parameters : Paramters) : void
}
"BaseClass`2" "<String, Int32>" <|-- DerivedClass
IInterface <|.. DerivedClass
DerivedClass o-- "ILogger`1" : Logger
DerivedClass o-- "*" Item : Item1
DerivedClass o-- "*" Item : Item2
DerivedClass ..> "<DerivedClass>" "ILogger`1"
DerivedClass ..> Paramters
@enduml
```

## Release Note

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
- [ ] Control visibility of members based on access modifiers.
- [ ] Treatment of type access modifiers. Currently not supported.
- [ ] Hide automatically implemented methods for record types.


## Feedback
As an alpha tester, your input is invaluable in shaping the future of this tool. Please share your thoughts, report bugs, and suggest enhancements on our [discussions page](https://github.com/pierre3/PlantUmlClassDiagramGenerator/discussions).
