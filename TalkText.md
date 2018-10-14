# NINNES
NINNES Is Not a NES Emulator System
## Description
Roslyn is the unsung hero of the .Net world behind most, if not all, C# and Visual Basic compilation processes. This talk aims to shine a light in the backstage of .Net and, especifically on Roslyn as the fundamental tool of every single .Net developer. How it understands code, how it compiles said code into a CLR-compatible binary, and more appliccably to the real world, how all of this can be customized for specific use cases to build Domain-Specific Languages based on C# with an imperative API. With a small focus on NES game programming, because... Why not?.
## About the Project
### What?
The NINNES project has the ultimate goal of allowing C# programmers to hit F5 on Visual Studio and see their code running on a NES emulator of their choice (not provided by the project itself) and optionally deploy to NES flash carts for execution on real hardware.

To ground such a lofty ideal the project is structured with 3 pillars: C# Code restrictions, NES dotNet Platform, C# Compilation to the NES CPU.

### How?
####  C# Code restrictions
In order to program any hardware at all the first step is to know and comply with the restrictions of said hardware's ISA (Instruction Set Architecture), memory and I/O capabilities. To ease the casual C# programmer into the limitations (particularly ISA) of the NES CPU the Roslyn C# compiler must be told to invalidate correct C# code that would make no sense or be impossible to implement in hardware within the limits of said CPU, thus a collection of Roslyn analyzers and code-fixes must be crafted to tailor the language. Visual Basic and other .Net-compatible languages are outside of the scope of this project.

Spoiler alert: Almost all of the System namespace is lost.

This pillar is the subject of the talk.

#### NES dotNet SDK/Platform
The NES has it's own particular way of doing things which must be mapped to the .Net realm, so a NuGet package providing a .Net-friendly API to elements like the CPU instructions, the PPU (Picture Processing Unit) and the APU (Audio Processing Unit) of the NES is essential to the success of the main goal. 

A mechanism to run the C# code on the standard desktop CLR with display, audio and input capabilities, similar to how XNA used to enable simple game programming, must also be included in the NuGet package so the code may be easily debugged. Direct2D, OpenGL and WebGL with WebAssembly are possible backend targets for this.

#### C# Compilation to the NES CPU
Actually outputting a valid ROM file that may run on the NES hardware from the C# code, with some possible avenues for this including:
- Code transpilation from C# 
	- To C for compilation with CC65
	- Directly outputting NES-compatible assembly code from the Roslyn code model
- MSIL recompilation with LLILC
- Researching the work of the .Net Micro Framework team

### Why?
The main objective of this project is to showcase Roslyn's flexibility for custom scenarios with an extreme case study relative to the expected use cases on the usual day to day development, so as to produce a library of Roslyn customization examples covering a wide range of scenarios. 

And for 1337 internet points, of course.

## Into the Machine
### CPUs and other Arcane things
#### Operators
The CPU is a machine designed to take sets of bytes and transform them. By using electrical mechanisms designed to operate on incoming bytes with arithmetic and logical procedures the CPU is able to perform the job the software intends for it.
#### Registers
The CPU has small named slots of ultra-fast memory to store data relevant to the execution of the instructions that comprise the program that are called registers. They are used to for temporary data relevant to the execution of both single instructions and the whole program.
Examples:
- Math operation result (Add, Subtract...)
- Integer operation overflow
- Program Counter
- Stack Pointer
#### Random Access Memory
This is a temporary storage medium that is orders of magnitude slower and bigger compared to the registers inside of the CPU and is intended to store the programs and data that is relevant to said programs.

Considering how RAM access is extremely slow to deliver data stored in itself to the CPU compared to how fast the CPU can run instructions one has to wonder how a CPU can avoid stalling execution while waiting for RAM data. To solve this problem the CPU maintains a copy of some parts of the RAM in extremely fast (but comparatively slow versus Registers) memory called Caché, that is tiered in levels (usually 3) scaling up in size but down in speed, yet are comparatively tiny versus the system RAM.
#### Instructions
The instructions tell the CPU what to do. Loading data from RAM into a Register, executing math on Register values and performing conditional branching are examples of simple CPU instructions.

In the most basic form of programming the instructions are usually codified according to this system:
`ADD R3 R4`
ADD being the Addition instruction, and R3 and R4 being Register names. This textual representation closely matches the final 1s and 0s that make up the instructions fed to the CPU, and it is called Assembly language.
#### Basic CPU operation
When the CPU is running it is constantly running a Fetch, Decode, Execute cycle to  run the programs stored in RAM.

Fetch refers to bringing in the next instruction for execution, storing it in the Instruction Register and increasing the Program Counter Register so the next cycle brings in the next instruction. Decode refers to priming the circuitry for the execution of said instruction and Execute refers to actually letting electricity run to the corresponding circuitry for the instruction.
#### The importance of Instructions
One could happily assume that any CPU providing instructions for basic math and conditional execution can eventually perform any task, alas, implementing complex algorithms using basic instructions (Referred to as Software Implementation) can yield extremely slow performance due to the many instructions required. CPU manufacturers regularly take a look at industry standards and publish newer CPUs with specific instructions that are implemented using dedicated electrical circuits (Referred to as Hardware Implementation) that complete the job much, much faster than generic circuits for the standard instructions.

A common example of the effect of this is early 2000's CPUs struggling to play back video with Software, compared to modern CPUs that barely have to raise their clock speed and power consumption values to do so because most common algorithms for audio and video handling are now implemented in dedicated circuits in hardware, such that merely the electric flow is enough to decode video successfully.

## From Visual Studio to F5, dotNet behind the curtains.
In order to more easily exemplify what happens between you pressing F5 and watching your creation come to life, an analogy to a classic play will be enacted.
### C#, the Language of the Script
The language we all know and love.
### BCL, the Acting Academy
The BCL -Base Class Library- is the basic set of classes and methods that Microsoft publishes with every dotNet release. This includes the built-in types like int, Integer, string, byte, Byte and methods ranging from Console.WriteLine to Task.Run and the P/Invoke infrastructure. 

Common extensions to the BCL range from essentially Win32 API wrappers like Windows Forms to wholly dotNet APIs based on Win32 calls like Windows Presentation Foundation and Windows Communication Foundation.
### Roslyn, Costumer Extraordinaire
The dotNet Compiler Platform, or Roslyn as the original codename and the community calls it, was born as a replacement for the ancient C# compiler that was originally written in C++ both as a way to show how the language had been evolving and "strengthening the ecosystem and becoming the best tooled language on the planet", as Mads Torgensen puts it.

Roslyn is not only a compiler though, it is an SDK (that Microsoft dogfoods on for Visual Studio on its 3 different flavors) to implement functionality around the C# and Visual Basic code parsing, analysis and compilation processes, with the express purpose of letting people hook into that information and, if so desired, influence it and modify it to suit diverse needs, even if that means strengthening the value proposition for Microsoft's competitors in the C# tooling space.

These are some of the ways Microsoft and the community uses Roslyn:
- Syntax Highlighting (on Visual Studio and beyond)
- Code Refactoring
- Code Security Analysis
- Code Quality Analysis
- Code Generation
### MSIL, the Script
One of the original goals of dotNet was giving Microsoft a hardware platform-neutral and managed way to program Windows, specially for servers of different hardware architectures. To that end, an intermediate language was created as the language of choice for dotNet execution to be translated into native CPU code with a JIT -Just In Time- model.
### MSBuild, the Playwright
The build system of choice for standard dotNet projects is MSBuild, a tool that reads project files (.csproj, .vbproj and so on) and invokes Roslyn and other associated tools on the input files to finally produce the MSIL .exe file.
### RyuJIT, the Director
RyuJIT falls in the Just In Time model of dotNet execution as the translator from MSIL to native CPU code.
### CLI, the Stage
The Common Language Infrastructure is the open ECMA standard that any platform that wishes to run dotNet code must implement, with CLR -Common Language Runtime- being Microsoft's official implementation. The CLI dictates how MSIL code must be run, including how a JIT (RyuJIT in the CLR) is to be used. 
### DLR, the Improv Master
The Dynamic Language Runtime is a relatively new addition to the dotNet ensemble, primarily to enable support of dynamic languages like Python and Ruby to be compiled into MSIL and hence executable wherever a capable CLI implementation exists.
### dotNet Native, the Cameraman
dotNet native ¨records¨ RyuJIT in action and generates a completely native (yet still BCL and CLI dependent) binary for any target CPU architecture. This is usually done for extremely performance-sensitive workloads, specially so in the case of those not tolerant of startup delays.

# From Visual Studio to the NES
A tale of loss, woe and unexpected viability.
## The limits of the NES 
The NES runs on a MOS6502 8-bit CPU implementation from Ricoh, albeit without the decimal/floating point mode enabled. This brings forth several limitations:
- No floating point calculations
- No multiplication or division implemented in hardware
- No Operating System
- Limited memory access capabilities
## Removing dotNet from C#, 8 bits at a time
To be able to fit a C# program inside the NES some concessions must be made
- No Garbage Collection
- No Threading
- No Operating System services
- No DLR
- No Reflection
- No Sockets/Communication
- And much more NOs!
## MSIL and 8-bit CPUs don't mix
Bringing C# compilation output to a MOS6502-compatible format may entail
- Assembler compilation from Roslyn ASTs with a C# Transpiler
- MSIL recompilation with LLILC
- Researching the work of the .Net Micro Framework
## Roslyn is NOT the enemy!
Demo Time.

