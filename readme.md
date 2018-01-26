# LSP Analyzer

A Windows C# [GUI](../../wiki/Gui) to analyse code with the help of a Language Server. LSP Analyzer is based on the C# framework [OmniSharp/csharp-language-server-protocol](https://github.com/OmniSharp/csharp-language-server-protocol). As a language server, I decided to use C/C++ language server [CQuery](https://github.com/cquery-project/cquery).

The concept is to use arbitrary language servers and to configure them by JSON as it is done by editors.

![](../../wiki/images/LspAnalyzer.png)

## Goals

- Have a GUI to play with [LSP](https://microsoft.github.io/language-server-protocol/) and language servers
- Get experiences with LSP [LSP](https://microsoft.github.io/language-server-protocol/)
- Analyze code
  - Required Interfaces
  - Provided Interfaces
  - Connect an UML/SysML tool with code
- Arbitrary language servers

## Features

LSP Analyzer supports the following features:

- [GUI](../../wiki/Gui)
  - Grid to visualize found results
  - [Filter](../../wiki/Filter)
  - Open implementation in Editor (currently onle VS Code) 
- Choose language server
- Choose wokspace with code or whatever
  - Symbol
  - Search global
  - Reference
  - Highlight
  - Hover
- Extensible

## Supported Servers

-  [CQuery C/C++ language server](https://github.com/cquery-project/cquery)
-  Servers with stdio protocol and of course the standard RPC-JSON
-  The basic framework [CQuery C/C++ language server](https://github.com/cquery-project/cquery) supports all types


## Based on

### CQuery

C/C++ language server.

### OmniSharp/csharp-language-server-protocol

The implementation of the language-server-protocol in C#. 

## State

-  In migration from local usage to GitHub
-  Tested with [CQuery C/C++ language server](https://github.com/cquery-project/cquery) 

## Installation

-  Windows, .net framework 4.6.1
-  Visual Studio 2017 community
-  Install and compile Language Server [CQuery C/C++ language server](https://github.com/cquery-project/cquery)
-  [CQuery C/C++ language server](https://github.com/cquery-project/cquery) is shipped inside solution
-  see also: [Installation](../../wiki/Installation)

## Thanks

Great support by 

- [CQuery C/C++ language server](https://github.com/cquery-project/cquery) and 
- [OmniSharp/csharp-language-server-protocol](https://github.com/OmniSharp/csharp-language-server-protocol).

## Glossary

- LSP Language Server Protocol
- Project
- Workspace The code or whatever to analyze

## Reference 

- [CQuery C/C++ language server](https://github.com/cquery-project/cquery)
- [Language Server Protocol](https://microsoft.github.io/language-server-protocol/)
- [OmniSharp/csharp-language-server-protocol](https://github.com/OmniSharp/csharp-language-server-protocol), the C# implementation of LSP here used
- [Wiki](../../wiki)




 