###############
Compiler Design
###############

The MOSA Compiler framework is designed around pipelines with multiple stages. There are two types of pipelines and their associated stages: 

Compiler Pipeline
    The first is the compiler pipeline. This pipeline consists of the high-level linear steps necessary to compile an application, such as building the type system, compiling each method, linking, and emitting the final object file.

Method Compiler Pipeline

    The other pipeline is the method compiler pipeline. This pipeline consists of stages which progressively lowers the high level instruction to native binary instructions of the target platform. These stages can be grouped into various kinds:

- Compiler Frontends Stage - Creates an instruction stream from a source specific representation, such as .NET CIL instructions
- Transformation Stages - Transforms the instruction stream between various representations
- Optimization Stages - Stages that intended to optimize the code to execute faster
- Register Allocation - Allocates architecture specific registers to operands used in the instruction stream
- Compiler Backends - Generates the binary code from platform specific instruction

The compiler framework provides predefined pieces of this pipeline. Some parts, especially the code generation, are provided by the architecture specific stages, such as for the x86 platform.

.. rubric:: Intermediate representations

The compiler framework uses a linear intermediate representation (vs an expression tree) to transform the source application into machine code. There are several levels of intermediate representations before code generation. These are:

- CIL - Common Intermediate Language
- IR - High-Level Intermediate Representation
- MIR - Machine specific Intermediate Representation

During compilation of an CIL method the instructions are represented by CIL instruction classes and moving forward, the linear instruction stream is modified to use instructions from the intermediate representation. In some cases an instruction from the intermediate representation can not be emitted directly to machine code and it is replaced by a sequence of machine specific instructions. The machine specific instruction classes are provided by the appropriate platform.


Compiler Optimizations
----------------------

Basic Optimizations
  Basic optimizations are a family of common types of optimizations, such as Constant Folding, Strength Reduction, Simplification, and many others.
  
Bit Tracker
  Bit Tracker tracks the known state of bits and value ranges thru the various operations. This enables various optimization and shortcuts. 

Long Expansion
  Expands 64-bit instructions into 32-bit components for platforms without native 64-bit instructions.

Inlined Expansion
  Inlines the code of small methods into the caller. This improves the performance, because calls can be expensive (storing the registers, placing the arguments onto stack, jumping to another location). 

Static Single Assignment Form
  Transforms instructions to Single Static Assignment (SSA) form. In SSA form, all virtual registers may only have one definition. This is not an optimization by itself, but this form enables other optimization opportunities in other types of optimizations.

Sparse Conditional Constant Propagation
  Sparse conditional constant propagation is an optimization applied after conversion to static single assignment form (SSA). It simultaneously removes dead code and propagates constants. It can finds more constant values, and thus more opportunities for improvement, than other optimizations.

Value Numbering
  Value numbering is a technique of determining when two computations in a program are equivalent and eliminating one of them with while preserving the same semantics. 

Two Pass Optimizations
  This options causes the optimization stages to be executed again, possibility unlocked additional optimiztions.

