# PlantUmlClassDiagramGenerator.SourceGenerator

## Overview
This tool is designed to generate PlantUML class diagrams from C# source code. Leveraging SourceGenerator functionality, it analyzes the source code and produces PlantUML class diagrams.

### Alpha Release
Please note that this is an alpha test version, and we appreciate your feedback to help improve and refine the tool.

## Features
- SourceGenerator Integration: Utilizing SourceGenerator, this tool seamlessly integrates with the C# compilation process to automatically generate PlantUML class diagrams.
- Improved Analysis with Symbols: In contrast to the previous version, PlantUmlClassDiagramGenerator, which relied on SyntaxTree for class analysis, the SourceGenerator version utilizes Symbols for a more efficient and accurate parsing of the source code.