# NINNES Talk
## Introduction
### About the Speaker
### About the Project
- This is just an excuse to talk about Roslyn
## Into the Machine
### From Visual Studio to F5!
- Of CPUs, GPGPUs and other Arcane things
- Languages: Talking to the Black Box
- Base Library: Defining a World-view
- ByteCode: Launchpad to a Multiverse
- Interpreters: Winging it
- Compilers: Everything prepared
- Runtime: Atlas Ex Machina
### The role of the Compiler
- Making sense of the Text
	- Lexemme extraction
- Operations and Operands
	- Syntax analysis
- The Forest for the Trees
	- Code representation inside Roslyn, ASTs
- Nodes of Leaves, Wood and Stone
	- Different kinds of Roslyn AST Nodes
- From Leaf to Leaf, learning all the way
	- AST Node traversal
- Shaking the Tree so Fruits fall off
	- Tree shaking, Code optimization
- Trimming the Bark
	- Linking and removing unused code
- Chopping Wood
	- Producing MSIL
## .Net behind the Curtains
### MSBuild, Playwright 
### Roslyn, Costumer Extraordinaire
### RyuJIT, Actor and Director 
### .Net Native, Phantom of the Opera
- AOT Compilation from MSIL
## What the NES is going on?
### What?
- Allow the NES to be programmed with C#
	- Visual Basic is outside of the scope
### Why?
- Showcase Roslyn's flexibility
- 1337 points, of course
### How?
- Customizing Roslyn to restrict ilegal operations for the NES CPU
- Providing a Class Library that performs the same operations as the NES CPU and PPU components
	- Desktop Direct2D/XNA backend
	- WebAssembly Canvas backend
- Providing re-implementations of forbidden operations on the NES with valid .Net calls
- Compiling .Net to the NES CPU



