<div align="center">
<strong><a href="README.md">English</a> | <a href="README.zh-CN.md">简体中文</a></strong>
</div>

# PlantUmlClassDiagramGenerator
这是一个生成器，用于从C#源代码中创建PlantUML的类图。

**README.md 版本修订历史**

| 版本 | 提交 | 备注                                | 
| --------| ------ |-----------------------------------|
| 1.1     | [e73b4fe](https://github.com/pierre3/PlantUmlClassDiagramGenerator/commit/e73b4feed9cd261271eb990a9c859f53536e8d7c) | 新增 "-excludeUmlBeginEndTags" 参数选项 |
| 1.0     | [70bb820](https://github.com/pierre3/PlantUmlClassDiagramGenerator/commit/70bb8202f7f489aa2d85ce9c25c58121c8f63aed) | 因为其它语言的README.md不一定同时更新，所以需要一个版本号 |

## Roslyn Source Generator
类图由 Roslyn 源代码生成器自动生成。详细信息请参见以下链接。
- [PlantUmlClassDiagramGenerator.SourceGenerator](/src/PlantUmlClassDiagramGenerator.SourceGenerator)

## Visual Studio Code 扩展

- [C# to PlantUML](https://marketplace.visualstudio.com/items?itemName=pierre3.csharp-to-plantuml)

## .Net Core 全局工具

Nuget Gallery: https://www.nuget.org/packages/PlantUmlClassDiagramGenerator

### 安装
下载并安装[.NET 8.0 SDK](https://www.microsoft.com/net/download/windows)或更新的版本。安装后，运行以下命令。

```bat
dotnet tool install --global PlantUmlClassDiagramGenerator
```
### 使用
运行 "puml-gen" 命令.

```bat
puml-gen InputPath [OutputPath] [-dir] [-public | -ignore IgnoreAccessibilities] [-excludePaths ExcludePathList] [-createAssociation]
```

- InputPath: (必须) 设置一个输入源文件或目录名称。
- OutputPath: (可选) 设置一个输出文件或目录名称。 
  如果省略此选项，plantuml文件将被输出到与输入文件相同的目录中。
- -dir: (可选) 当InputPath和OutputPath为目录名时，指定。
- -public: (可选)  如果指定，只输出公共可及性成员。
- -ignore: (可选) 指定要忽略的成员的可访问性，用逗号分隔的列表。
- -excludePaths: (可选) 指定排除的文件和目录。  
  指定来自 "InputPath "的相对路径，用逗号分隔的列表。 
要排除包含特定文件夹名称的多个路径，请在名称前加上 "\*\*/"。示例："**/bin"
- -createAssociation: (可选) 从字段和属性的引用中创建对象关联。
- -allInOne: (可选) 只有当-dir被设置时：将所有图表的输出复制到文件include.puml（这允许PlanUMLServer渲染）。
- -attributeRequired: (可选) 当这个开关被启用时，只有类型声明中带有 "PlantUmlDiagramAttribute "的类型会被输出。
- -excludeUmlBeginEndTags: （可选）当启用此开关时，它将从 puml 文件中排除 \"@startuml\" 和 \"@enduml\"l 标签。

例子：
```bat
puml-gen C:\Source\App1\ClassA.cs -public
```

```bat
puml-gen C:\Source\App1 C:\PlantUml\App1 -dir -ignore Private,Protected -createAssociation -allInOne
```

```bat
puml-gen C:\Source\App1 C:\PlantUml\App1 -dir -excludePaths bin,obj,Properties
```

## 转换为PlantUML的规范

### 类型声明

#### Type 关键字

|C#               | PlantUML           |
|:----------------|-------------------:|
| `class`         | `class`            |
| `struct`        | `struct`           |
| `interface`     | `interface`        |
| `enum`          | `enum`             |
| `record`        | `<<record>> class` |

#### Type 修饰符

|C#               | PlantUML           |
|:----------------|-------------------:|
| `abstract`       | `abstract`        |
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
record RecordA {
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
struct StructA {
}
interface InterfaceA {
}
class RecordA <<record>> {
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
@enduml
```

![TypeDeclaration.png](uml/TypeDeclaration.png)

#### 泛型

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

![GenericsTypeDeclaration.png](uml/GenericsTypeDeclaration.png)

### 成员声明

#### 可见性修饰符

|C#                    | PlantUML           |
|:---------------------|-------------------:|
| `public`             | `+`                |
| `internal`           | `<<internal>>`     |
| `protected internal` | `# <<internal>>`   |
| `protected`          | `#`                |
| `private`            | `-`                |

#### 修饰符

|C#            | PlantUML         |
|:-------------|-----------------:|
| `abstract`   | `{abstract}`     |
| `static`     | `{static}`       |
| `virtual`    | `<<virtual>>`    |
| `override`   | `<<override>>`   |
| `new`        | `<<new>>`        |
| `readonly`   | `<<readonly>>`   |
| `event`      | `<<event>>`      |

#### 属性访问器

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

![MemberDeclaration.png](uml/MemberDeclaration.png)

#### 字段和属性初始化

只有**常量**的初始化才会被输出。

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

![Initializer.png](uml/Initializer.png)

### 嵌套类声明

嵌套类被展开并与 "OuterClass + - InnerClass "关联。

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

![NestedClass.png](uml/NestedClass.png)

### 继承关系

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

![InheritanceRelationsips.png](uml/InheritanceRelationsips.png)

### 关联（来自字段和属性的引用）

如果你指定了 "createAssociation "选项，对象关联将从字段和属性引用中创建。

- C#

```cs
class ClassA{
    public IList<string> Strings{get;} = new List<string>();
    public Type1 Prop1{get;set;}
    public Type2 field1;
}

class Type1 {
    public int value1{get;set;}
}

class Type2{
    public string string1{get;set;}
    public ExternalType Prop2 {get;set;}
}
```

- PlantUML

```
@startuml
class ClassA {
}
class Type1 {
    + value1 : int <<get>> <<set>>
}
class Type2 {
    + string1 : string <<get>> <<set>>
}
class "IList`1"<T> {
}
ClassA o-> "Strings<string>" "IList`1"
ClassA --> "Prop1" Type1
ClassA --> "field1" Type2
Type2 --> "Prop2" ExternalType
@enduml
```

![InheritanceRelationsips.png](uml/Associations.png)


### 记录类型（含参数列表）

C# 9中的记录类型可以有一个参数列表。在这些情况下，这些参数
被作为属性添加到类中。


- C#

```cs
record Person(string Name, int Age);

record Group(string GroupName) {
    public Person[] Members { get; init; }
}
```

- PlantUML

```
@startuml
class Person <<record>> {
    + Name : string <<get>> <<init>>
    + Age : int <<get>> <<init>>
}
class Group <<record>> {
    + GroupName : string <<get>> <<init>>
    + Members : Person[] <<get>> <<init>>
}
@enduml
```

![InheritanceRelationsips.png](uml/RecordParameterList.png)

## 基于特性的配置

你可以将[PlantUmlClassDiagramGenerator.Attributes](https://www.nuget.org/packages/PlantUmlClassDiagramGenerator.Attributes)包添加到你的C#项目中，用于基于特性的配置。

### PlantUmlDiagramAttribute
只有被添加了PlantUmlDiagramAttribute的类型才会被输出。
如果-attributeRequired开关被添加到命令行参数中，这个属性就会被启用。

这个属性只能被添加到类型声明中。

- class
- struct
- enum
- record

```cs
class ClassA
{
    public string Name { get; set; }
    public int Age { get; set; }
}

[PlantUmlDiagram]
class ClassB
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

只有带有PlantUmlDiagramAttribute的ClassB会被输出。

```
@startuml
class ClassB {
    + Name : string <<get>> <<set>>
    + Age : int <<get>> <<set>>
}
@enduml
```


### PlantUmlIgnoreAttribute
添加了这个属性的元素被排除在输出之外。

```cs
[PlantUmlIgnore]
class ClassA
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class ClassB
{
    public string Name { get; set; }
    [PlantUmlIgnore]
    public int Age { get; set; }
}

class ClassC
{
    public string Name { get; set; }
    public int Age { get; set; }

    [PlantUmlIgnore]
    public ClassC(string name, int age) => (Name, Age) = (name, age);
    
    public void MethodA();
    
    [PlantUmlIgnore]
    public void MethodB();
}
```

```
@startuml
class ClassB {
    + Name : string
}
class ClassC {
    + Name : string
    + Age : int
    + MethodA() : void
}
@enduml
```

### PlantUmlAssociationAttribute
通过添加这个属性，你可以定义类之间的关联。
这个属性可以被添加到属性、字段和方法参数。

关联的细节被定义在以下属性中。

- _Name_
  - 指定叶子节点一侧的类型名称。
  - 如果省略，则使用添加该属性的元素的名称。
- _Association_
  - 指定关联的边缘部分。在PlantUML中设置一个有效的字符串。
  - 如果省略，则使用"--"。
- _RootLabel_
  - 指定显示在根节点一侧的标签。
  - 如果省略，则不显示。
- _Label_
  - 指定要显示在边缘中心的标签。
  - 如果省略，则不显示。
- _LeafLabel_ 
  - 指定显示在叶子节点一侧的标签。
  - 如果省略，则不显示。 

```cs
class Parameters
{
    public string A { get; set; }
    public string B { get; set; }
}

class CustomAssociationSample
{
    [PlantUmlAssociation(Name = "Name", Association = "*-->", LeafLabel = "LeafLabel", Label= "Label", RootLabel = "RootLabel")] 
    public ClassA A { get; set; }
}

class CollectionItemsSample
{
    [PlantUmlAssociation(Name = "Item", Association = "o--", LeafLabel = "0..*", Label = "Items")]
    public IList<Item> Items { get; set; }
}

class MethodParamtersSample
{
    public void Run([PlantUmlAssociation(Association = "..>", Label = "use")] Parameters p)
    {
        Console.WriteLine($"{p.A},{p.B}");
    }

    private ILogger logger;
    public MyClass([PlantUmlAssociation(Association = "..>", Label = "Injection")] ILogger logger)
    {
        this.logger = logger;
    }
}
```

```
@startuml
class Parameters {
    + A : string <<get>> <<set>>
    + B : string <<get>> <<set>>
}
class CustomAssociationSample {
}
class CollectionItemsSample {
}
class MethodParamtersSample {
    + Run(p:Parameters) : void
    + MyClass(logger:ILogger)
}
CustomAssociationSample "RootLabel" *--> "LeafLabel" Name : "Label"
CollectionItemsSample o-- "0..*" Item : "Items"
MethodParamtersSample ..> Parameters : "use"
MethodParamtersSample ..> ILogger : "Injection"
@enduml
```

![CustomAssociation.png](uml/CustomAssociation.png)

### PlantUmlIgnoreAssociationAttribute

这个属性可以被添加到属性和字段中。
具有此属性的属性（或字段）被描述为类的成员，没有任何关联。

```cs
class User
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class ClassA
{
    public static User DefaultUser { get; }
    public IList<User> Users { get; }

    public ClassA(IList<User> users)
    {
        Users = users;
        DefaultUser = new User()
        {
            Name = "DefaultUser",
            Age = "20"
        };
    }
}

class ClassB
{
    [PlantUmlIgnoreAssociation]
    public static User DefaultUser { get; }

    [PlantUmlIgnoreAssociation]
    public IList<User> Users { get; }

    public ClassB(IList<User> users)
    {
        Users = users;
        DefaultUser = new User()
        {
            Name = "DefaultUser",
            Age = "20"
        };
    }
}
```

```
@startuml
class User {
    + Name : string <<get>> <<set>>
    + Age : int <<get>> <<set>>
}
class ClassA {
    + ClassA(users:IList<User>)
}
class ClassB {
    + {static} DefaultUser : User <<get>>
    + Users : IList<User> <<get>>
    + ClassB(users:IList<User>)
}
class "IList`1"<T> {
}
ClassA --> "DefaultUser" User
ClassA --> "Users<User>" "IList`1"
@enduml
```

![IgnoreAssociation.png](uml/IgnoreAssociation.png)
