# PlantUmlClassDiagramGenerator
This is a generator to create a class-diagram of PlantUML from the C# source code.

## Usage

```bat
C:\> PlantUmlClassDiagramGenerator.exe InputPath [OutputPath] [-dir] [-public | -ignore IgnoreAccessibilities]
```

- InputPath: (Required) Sets a input source file or directory name.    
- OutputPath: (Optional) Sets a output file or directory name.
- -dir: (Optional) Specify when InputPath and OutputPath are directory names.
- -public: (Optional) If specified, only public accessibility members are output. 
- -ignore: (Optional) Specify the accessibility of members to ignore, separated by commas.

examples
```bat
C:\> PlantUmlClassDiagramGenerator.exe C:\Source\App1\ClassA.cs -public
```

```bat
C:\> PlantUmlClassDiagramGenerator.exe C:\Source\App1 C:\PlantUml\App1 -dir -ignore Private,Protected
```


## Using Roslyn

```cs
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
      var code = @"
namespace Test
{
  class ClassA
  {
    public int PropA {get;}
    public string PropB {get;set;}
    publc int Method(){
      return 0;
    }
  }
}
";
      var tree = CSharpSyntaxTree.ParseText(code);
      var root = tree.GetRoot();

      var output = new StringBuilder();
      using (var writer = new StringWriter(output))
      {
          var gen = new ClassDiagramGenerator(writer, indent:"    ");
          gen.Visit(root);
      }
     Console.WriteLine(output);
     }
}

/*** output..
class A{
    + PropA : int <<get>>
    + PropB : string <<get>> <<set>>
    + Method():int
}
***/

```
