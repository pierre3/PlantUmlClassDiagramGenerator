# PlantUmlClassDiagramGenerator
This is a generator to create a class-diagram of PlantUML from the C# source code.

## Usage

```bat
C:\> PlantUmlClassDiagramGenerator.exe InputPath [OutputPath] [-dir] [-public | -ignore IgnoreAccessibilities]
```

- InputPath: (Required) Sets a input source file or directory name.
- OutputPath: (Optional) Sets a output file or directory name.  
  If you omit this option, plantuml files are outputted to same directory as the input files.
- -dir: (Optional) Specify when InputPath and OutputPath are directory names.
- -public: (Optional) If specified, only public accessibility members are output. 
- -ignore: (Optional) Specify the accessibility of members to ignore, with a comma separated list.
- -excludePaths: (Optional) Specify the exclude file and directory.   
  Specifies a relative path from the "InputPath", with a comma separated list.

examples
```bat
C:\> PlantUmlClassDiagramGenerator.exe C:\Source\App1\ClassA.cs -public
```

```bat
C:\> PlantUmlClassDiagramGenerator.exe C:\Source\App1 C:\PlantUml\App1 -dir -ignore Private,Protected
```

```bat
C:\> PlantUmlClassDiagramGenerator.exe C:\Source\App1 C:\PlantUml\App1 -dir -excludePaths bin,obj,Properties
```

## Specification for conversion to PlantUML

### Type Declaration

#### Type Keywords

|C#               | PlantUML           |
|:----------------|-------------------:|
| `class`         | `class`            |
| `struct`        | `<<struct>> class` |
| `interface`     | `interface`        |
| `enum`          | `enum`             |

#### Type Modifiers

|C#               | PlantUML           |
|:----------------|-------------------:|
| `abstrct`       | `abstract`         |
| `static`        | `<<static>>`       |
| `partial`       | `<<partial>>`      |
| `sealed`        | `<<sealed>>`       |

- C#

```cs
class ClassA {  
}
struct StructA {
}
interface InterfaceA {
}
abstract class AbstractClass {
}
static class StaticClass {
}
sealed partial class ClassB{
}
enum EnumType{
  Apple,
  Orange,
  Grape
}
```
- PlantUML

```
@startuml
class ClassA {
}
class StructA <<struct>> {
}
interface InterfaceA {
}
abstract class AbstractClass {
}
class StaticClass <<static>> {
}
class ClassB <<sealed>> <<partial>> {
}
enum EnumType {
    Apple,
    Orange,
    Grape,
}
enduml
```

![TypeDeclaration.png](https://github.com/pierre3/PlantUmlClassDiagramGenerator/blob/master/uml/TypeDeclaration.png)

#### Generics Type

- C#

```cs
class GenericsType<T1>{
}
class GenericsType<T1,T2>{
}
```
- PlantUML

```
class "GenericsType`1"<T1>{
}
class "GenericsType`2"<T1,T2>{
}
```

![GenericsTypeDeclaration.png](https://github.com/pierre3/PlantUmlClassDiagramGenerator/blob/master/uml/GenericsTypeDeclaration.png)

### Member Declaration

#### Accessibility Modifiers

|C#                    | PlantUML           |
|:---------------------|-------------------:|
| `public`             | `+`                |
| `internal`           | `<<internal>>`     |
| `protected internal` | `# <<internal>>`   |
| `protected`          | `#`                |
| `private`            | `-`                |

#### Modifiers

|C#            | PlantUML         |
|:-------------|-----------------:|
| `abstract`   | `{abstract}`     |
| `static`     | `{static}`       |
| `virtual`    | `<<virtual>>`    |
| `override`   | `<<override>>`   |
| `new`        | `<<new>>`        |
| `readonly`   | `<<readonly>>`   |
| `event`      | `<<event>>`      |

#### Property Accessors

|C#                              | PlantUML                            |
|:-------------------------------|------------------------------------:|
| `int Prop {get; set;}`         | `Prop : int <<get>> <<set>>`        |
| `int Prop {get;}`              | `Prop : int <get>`                  |
| `int Prop {get; private set }` | `Prop : int <<get>><<private set>>` |
| `int Prop => 100;`             | `Prop : int <<get>>`                |

- C#

```cs
abstract class AbstractClass
{
    protected int _x;
    internal int _y;
    protected internal int _z;
    public abstract void AbstractMethod();
    protected virtual void VirtualMethod(string s){

    }
    public string BaseMethod(int n){
        return "";
    }
}
class ClassM : AbstractClass
{
    public static readonly double PI =3.141592;
    public int PropA { get; set; }
    public int PropB { get; protected set; }
    public event EventHandler SomeEvent;
    public override void AbstractMethod(){
        
    }
    protected override void VirtualMethod(string s)
    {

    }
    public override string ToString()
    {
        return "override";
    }
    public new string BaseMethod(int n){
        return "new";
    }
}
```

- PlantUML

```
abstract class AbstractClass {
    # _x : int
    <<internal>> _y : int
    # <<internal>> _z : int
    + {abstract} AbstractMethod() : void
    # <<virtual>> VirtualMethod(s:string) : void
    + BaseMethod(n:int) : string
}
class ClassM {
    + {static} <<readonly>> PI : double = 3.141592
    + PropA : int <<get>> <<set>>
    + PropB : int <<get>> <<protected set>>
    +  <<event>> SomeEvent : EventHandler 
    + <<override>> AbstractMethod() : void
    # <<override>> VirtualMethod(s:string) : void
    + <<override>> ToString() : string
    + <<new>> BaseMethod(n:int) : string
}
AbstractClass <|-- ClassM
```

![MemberDeclaration.png](https://github.com/pierre3/PlantUmlClassDiagramGenerator/blob/master/uml/MemberDeclaration.png)

#### Field and Property Initializers

Only __literal__ initializers are output.

- C#

```cs
class ClassC
{
    private int fieldA = 123;
    public double Pi {get;} = 3.14159;
    protected List<string> Items = new List<string>(); 
}
```

- PlantUML

```
class ClassC {
  - fieldA : int = 123
  + Pi : double = 3.14159
  # Items : List<string>
}
```

![Initializer.png](https://github.com/pierre3/PlantUmlClassDiagramGenerator/blob/master/uml/Initializer.png)

### Nested Class Declaration

Nested classes are expanded and associated with "OuterClass + - InnerClass".

- C#

```cs
class OuterClass 
{
  class InnerClass 
  {
    struct InnerStruct 
    {

    }
  }
}
```

- PlantUML

```
class OuterClass{

}
class InnerClass{

}
<<struct>> class InnerStruct {

}
OuterClass +- InnerClass
InnerClass +- InnerStruct
```

![NestedClass.png](https://github.com/pierre3/PlantUmlClassDiagramGenerator/blob/master/uml/NestedClass.png)

### Inheritance Relationsips

- C#

```cs
abstract class BaseClass
{
    public abstract void AbstractMethod();
    protected virtual int VirtualMethod(string s) => 0;
}
class SubClass : BaseClass
{
    public override void AbstractMethod() { }
    protected override int VirtualMethod(string s) => 1;
}

interface IInterfaceA {}
interface IInterfaceA<T>:IInterfaceA
{
    T Value { get; }
}
class ImplementClass : IInterfaceA<int>
{
    public int Value { get; }
}
```

- PlantUML

```
abstract class BaseClass {
    + {abstract} AbstractMethod() : void
    # <<virtual>> VirtualMethod(s:string) : int
}
class SubClass {
    + <<override>> AbstractMethod() : void
    # <<override>> VirtualMethod(s:string) : int
}
interface IInterfaceA {
}
interface "IInterfaceA`1"<T> {
    Value : T <<get>>
}
class ImplementClass {
    + Value : int <<get>>
}
BaseClass <|-- SubClass
IInterfaceA <|-- "IInterfaceA`1"
"IInterfaceA`1" "<int>" <|-- ImplementClass
```

![InheritanceRelationsips.png](https://github.com/pierre3/PlantUmlClassDiagramGenerator/blob/master/uml/InheritanceRelationsips.png)
