
# NINNES Talk
## Introduction (5 minutes)
### About the Speaker
### About the Project
- This is just an excuse to talk about Roslyn
## What the NES is going on? (5 minutes)
### What?
- Allow the NES to be programmed with C#
	- Visual Basic is outside of the scope
	- Running C# on the NES CPU is a far-away target
### Why?
- Showcase Roslyn's flexibility
- 1337 Internet points, of course
- Exemplify tailoring Roslyn to a extremely specific business use case
### How?
- Customizing Roslyn to restrict ilegal operations for the NES CPU
- Providing a NuGet Class Library that performs equivalent operations as the NES CPU, PPU and APU components
	- Desktop Direct2D/OpenGL/XNA backend
	- WebAssembly Canvas backend
- Providing software re-implementations of forbidden/unsupported operations on the NES with valid .Net calls
- Compiling C# to the NES CPU 
# Into the Machine
### CPUs and other Arcane things (5 minutes)
- Circuitry
- Registers
- Memory
- Instructions
- Basic CPU Loop
- "Hardware Instructions" vs "Implemented in Software"?
### From Visual Studio to F5, Modern dotNet Behind the Curtains. (5 minutes)
- C# Language, 
- BCL, The Stage for the Actors
- Roslyn, Costumer Extraordinaire
- MSIL, Playwright
- RyuJIT, Director
- .Net Native, Anti-Director
### The role of the Compiler (10 minutes)
- Tending the ASCII seeds
	- Lexeme extraction
	- Syntax analysis
- The Forest for the Trees
	- Code representation inside Roslyn, ASTs
	- Different kinds of Roslyn AST Nodes
-  Preparing the shipment
	- Producing MSIL
## From Roslyn to the NES (15 minutes)
### What can't the NES CPU do?
### Code diagnostics and Fixes
### What will be lost in the way
- This is NOT dotNet, this is C#
- Garbage Collection
- Threading
- Possibly more?
### Compiling down to the NES
- AST interpretation down to C or ASM instructions - C# Transpiler
- MSIL recompilation with LLILC
- Continuing the work of the .NET Micro Framework
## Little Demo (5 minutes)
- Code analysis makes Roslyn reject valid C# code that can't run on the NES


