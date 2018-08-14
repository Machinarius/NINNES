# NINNES
NINNES isn't a dotNet NES Emulator System
# About this project
This project aims to be an example on tailoring the Roslyn C# compiler to specific needs by restricting normal code compilation to only compile constructs that are valid in the context of the NES CPU and PPU capabilities, with the ultimate aim to allow people to write NES-compatible programs with C#.

Visual Basic is outside the current and future scope of this project.
# Current situation
We are crafting the initial analyzers that would tailor C# down to the capabilities of the NES, such as disabling integer multiplication and floating point operations.
# End goal
The end goal is to allow the specially crafted C# to compile down to NES machine code with the help of the [llilc](https://github.com/dotnet/llilc) LLVM C# compiler project and a new LLVM backend based on [CC65](http://cc65.github.io/cc65/) code.

