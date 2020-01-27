######################
Command Line Arguments
######################

The command line arguments serve as shortcuts to common set of :doc:`settings-options` used by the MOSA tool sets.

.. tip:: Specific settings may also be specified on the command line using the ``--setting`` argument. For example to set the ``Compiler.OutputFile`` settings with ``Mosa.HelloWorld.x86.bin``, pass the following two arguments ``--setting Compiler.OutputFile=Mosa.HelloWorld.x86.bin`` on the command line.

Below are the command line arguments available:

.. csv-table:: 
   :header: "Argument","Setting","Value Set"
   :widths: 100, 100, 50
   
    {none},Compiler.SourceFiles,{value}
    --settings,Settings,{value}
    --o,Compiler.OutputFile,{value}
    --threading,Compiler.Multithreading,true
    --threading-off,Compiler.Multithreading,false
    --base,Compiler.BaseAddress,{value}
    --scanner,Compiler.MethodScanner,true
    --no-code,Compiler.Binary,false
    --path,SearchPaths, 
    --inline,Optimizations.Inline,true
    --inline-off,Optimizations.Inline,false
    --ssa,Optimizations.SSA,true
    --ssa-off,Optimizations.SSA,false
    --sccp,Optimizations.SCCP,true
    --sccp-off,Optimizations.SCCP,false
    --basic-optimizations,Optimizations.Basic,true
    --basic-optimizations-off,Optimizations.Basic,false
    --inline-explicit,Optimizations.Inline.ExplicitOnly,true
    --inline-explicit-off,Optimizations.Inline.ExplicitOnly,false
    --long-expansion,Optimizations.LongExpansion,true
    --long-expansion-off,Optimizations.LongExpansion,false
    --two-pass,Optimizations.TwoPass,true
    --two-pass-off,Optimizations.TwoPass,true
    --value-numbering,Optimizations.ValueNumbering,true
    --value-numbering-off,Optimizations.ValueNumbering,false
    --loop-invariant-code-motion,Optimizations.LoopInvariantCodeMotion,true
    --loop-invariant-code-motion-off,Optimizations.LoopInvariantCodeMotion,false
    --platform-optimizations,Optimizations.Platform,true
    --platform-optimizations-off,Optimizations.Platform,false
    --bit-tracker,Optimizations.BitTracker,true
    --bit-tracker-off,Optimizations.BitTracker,false
    --devirtualization,Optimizations.Devirtualization,true
    --devirtualization-off,Optimizations.Devirtualization,false
    --inline-level,Optimizations.Inline.Maximum,{value}
    -o,Compiler.OutputFile,{value}

    --optimizations-off,Optimizations.Inline,false
    --optimizations-off,Optimizations.SSA,false
    --optimizations-off,Optimizations.SCCP,false
    --optimizations-off,Optimizations.Devirtualization,false
    --optimizations-off,Optimizations.Basic,false
    --optimizations-off,Optimizations.LongExpansion,false
    --optimizations-off,Optimizations.Inline,false
    --optimizations-off,Optimizations.LoopInvariantCodeMotion,false
    --optimizations-off,Optimizations.Platform,false
    --optimizations-off,Optimizations.BitTracker,false
    --optimizations-off,Optimizations.ValueNumbering,false
    --optimizations-off,Optimizations.TwoPass,false

    --output-nasm,CompilerDebug.NasmFile,%DEFAULT%
    --output-asm,CompilerDebug.AsmFile,%DEFAULT%
    --output-map,CompilerDebug.MapFile,%DEFAULT%
    --output-time,CompilerDebug.CompilerTimeFile,%DEFAULT%
    --output-debug,CompilerDebug.DebugFile,%DEFAULT%
    --output-inlined,CompilerDebug.InlinedFile,%DEFAULT%
    --output-hash,CompilerDebug.PreLinkHashFile,%DEFAULT%
    --output-hash,CompilerDebug.PostLinkHashFile,%DEFAULT%

    --platform,Compiler.Platform,{value}
    --x86,Compiler.Platform,x86
    --x64,Compiler.Platform,x64
    --armv8a32,Compiler.Platform,armv8a32

    --interrupt-method,X86.InterruptMethodName,{value}

    // Linker:
    --emit-all-symbols,Linker.Symbols,true
    --emit-all-symbols-off,Linker.Symbols,false
    --emit-relocations,Linker.StaticRelocations,true
    --emit-relocations-off,Linker.StaticRelocations,false
    --emit-static-relocations,Linker.StaticRelocations,true
    --emit-drawf,Linker.Drawf,true
    --emit-drawf-off,Linker.Drawf,false
    --drawf,Linker.Drawf,true

    // Explorer:
    --filter,Explorer.Filter,{value}

    // Launcher:
    --autoexit,Launcher.Exit,true
    --autoexit-off,Launcher.Exit,false
    --autostart,Launcher.Start,true
    --autostart-off,Launcher.Start,false
    --autolaunch,Launcher.Launch,true
    --autolaunch-off,Launcher.Launch,false

    --destination,Image.Folder,{value}
    --dest,Image.Folder,{value}

    --launch,Launcher.Launch,true
    --launch-off,Launcher.Launch,false

    // Launcher - Emulator:
    --emulator,Emulator,
    --qemu,Emulator,qemu
    --vmware,Emulator,vmware
    --bochs,Emulator,bochs
    --display,Emulator.Display,on
    --display-off,Emulator.Display,off
    --emulator-memory,Emulator.Memory,
    --qemu-gdb,Emulator.GDB,false

    // Launcher - Image:
    --vhd,Image.Format,vhd
    --img,Image.Format,img
    --vdi,Image.Format,vdi
    --iso,Image.Format,iso
    --vmdk,Image.Format,vmdk
    --image,Image.ImageFile,{value}

    --blocks,Image.DiskBlocks,
    --volume-label,Image.VolumeLabel,
    --mbr,Image.MasterBootRecordFile,
    --boot,Image.BootBlockFile,

    // Launcher - Boot:
    --multiboot-v1,Multiboot.Version,v1
    --multiboot-v2,Multiboot.Version,v2
    --multiboot-none,Multiboot.Version,
    --multiboot,Multiboot.Version,{value}

    // Launcher - Serial:
    --serial-connection,Emulator.Serial,
    --serial-pipe,Emulator.Serial,pipe
    --serial-tcpclient,Emulator.Serial,tcpclient
    --serial-tcpserver,Emulator.Serial,tcpserver
    --serial-connection-port,Emulator.Serial.Port,{value}
    --serial-connection-host,Emulator.Serial.Host,{value}

    --video,Multiboot.Video,true
    --video-width,Multiboot.Video.Width,{value}
    --video-height,Multiboot.Video.Height,{value}
    --video-depth,Multiboot.Video.Depth,{value}

    --gdb,Launcher.Advance.LaunchGDBDebugger,true
    --gdb-port,GDB.Port,{value}
    --gdb-host,GDB.Host,{value}

    --launch-gdb-debugger,Launcher.Advance.LaunchGDBDebugger,true

    --bootloader,Image.BootLoader,{value}
    --grub,Image.BootLoader,grub_v0.97
    --grub-0.97,Image.BootLoader,grub_v0.97
    --grub2,Image.BootLoader,grub_v2.00
    --syslinux,Image.BootLoader,syslinux_v3.72
    --syslinux-3.72,Image.BootLoader,syslinux_v3.72
    --syslinux-6.0,Image.BootLoader,syslinux_v6.03

    // Launcher - Advance:
    --hunt-corlib,Launcher.Advance.HuntForCorLib,true
    --plug-korlib,Launcher.Advance.PlugKorlib,true

    // Debugger:
    --breakpoints,Debugger.BreakpointFile,{value}
    --watch,Debugger.WatchFile,{value}

    // Optimization Levels:
    -o0,Optimizations.Basic,false
    -o0,Optimizations.SSA,false
    -o0,Optimizations.ValueNumbering,false
    -o0,Optimizations.SCCP,false
    -o0,Optimizations.Devirtualization,false
    -o0,Optimizations.LongExpansion,false
    -o0,Optimizations.Platform,false
    -o0,Optimizations.Inline,false
    -o0,Optimizations.LoopInvariantCodeMotion,false
    -o0,Optimizations.BitTracker,false
    -o0,Optimizations.TwoPass,false
    -o0,Optimizations.Inline.Maximum,0

    -o1,Optimizations.Basic,true
    -o1,Optimizations.SSA,false
    -o1,Optimizations.ValueNumbering,false
    -o1,Optimizations.SCCP,false
    -o1,Optimizations.Devirtualization,true
    -o1,Optimizations.LongExpansion,false
    -o1,Optimizations.Platform,false
    -o1,Optimizations.Inline,false
    -o1,Optimizations.LoopInvariantCodeMotion,false
    -o1,Optimizations.BitTracker,false
    -o1,Optimizations.TwoPass,false
    -o1,Optimizations.Inline.Maximum,0

    -o2,Optimizations.Basic,true
    -o2,Optimizations.SSA,true
    -o2,Optimizations.ValueNumbering,true
    -o2,Optimizations.SCCP,false
    -o2,Optimizations.Devirtualization,true
    -o2,Optimizations.LongExpansion,false
    -o2,Optimizations.Platform,false
    -o2,Optimizations.Inline,false
    -o2,Optimizations.LoopInvariantCodeMotion,false
    -o2,Optimizations.BitTracker,false
    -o2,Optimizations.TwoPass,false
    -o2,Optimizations.Inline.Maximum,0

    -o3,Optimizations.Basic,true
    -o3,Optimizations.SSA,true
    -o3,Optimizations.ValueNumbering,true
    -o3,Optimizations.SCCP,true
    -o3,Optimizations.Devirtualization,true
    -o3,Optimizations.LongExpansion,false
    -o3,Optimizations.Platform,false
    -o3,Optimizations.Inline,false
    -o3,Optimizations.LoopInvariantCodeMotion,false
    -o3,Optimizations.BitTracker,false
    -o3,Optimizations.TwoPass,false
    -o3,Optimizations.Inline.Maximum,0

    -o4,Optimizations.Basic,true
    -o4,Optimizations.SSA,true
    -o4,Optimizations.ValueNumbering,true
    -o4,Optimizations.SCCP,true
    -o4,Optimizations.Devirtualization,true
    -o4,Optimizations.LongExpansion,true
    -o4,Optimizations.Platform,false
    -o4,Optimizations.Inline,false
    -o4,Optimizations.LoopInvariantCodeMotion,false
    -o4,Optimizations.BitTracker,false
    -o4,Optimizations.TwoPass,false
    -o4,Optimizations.Inline.Maximum,0

    -o5,Optimizations.Basic,true
    -o5,Optimizations.SSA,true
    -o5,Optimizations.ValueNumbering,true
    -o5,Optimizations.SCCP,true
    -o5,Optimizations.Devirtualization,true
    -o5,Optimizations.LongExpansion,true
    -o5,Optimizations.Platform,true
    -o5,Optimizations.Inline,false
    -o5,Optimizations.LoopInvariantCodeMotion,false
    -o5,Optimizations.BitTracker,false
    -o5,Optimizations.TwoPass,false
    -o5,Optimizations.Inline.Maximum,0

    -o6,Optimizations.Basic,true
    -o6,Optimizations.SSA,true
    -o6,Optimizations.ValueNumbering,true
    -o6,Optimizations.SCCP,true
    -o6,Optimizations.Devirtualization,true
    -o6,Optimizations.LongExpansion,true
    -o6,Optimizations.Platform,true
    -o6,Optimizations.Inline,true
    -o6,Optimizations.LoopInvariantCodeMotion,false
    -o6,Optimizations.BitTracker,false
    -o6,Optimizations.TwoPass,false
    -o6,Optimizations.Inline.Maximum,5

    -o7,Optimizations.Basic,true
    -o7,Optimizations.SSA,true
    -o7,Optimizations.ValueNumbering,true
    -o7,Optimizations.SCCP,true
    -o7,Optimizations.Devirtualization,true
    -o7,Optimizations.LongExpansion,true
    -o7,Optimizations.Platform,true
    -o7,Optimizations.Inline,false
    -o7,Optimizations.LoopInvariantCodeMotion,true
    -o7,Optimizations.BitTracker,false
    -o7,Optimizations.TwoPass,false
    -o7,Optimizations.Inline.Maximum,10

    -o8,Optimizations.Basic,true
    -o8,Optimizations.SSA,true
    -o8,Optimizations.ValueNumbering,true
    -o8,Optimizations.SCCP,true
    -o8,Optimizations.Devirtualization,true
    -o8,Optimizations.LongExpansion,true
    -o8,Optimizations.Platform,true
    -o8,Optimizations.Inline,true
    -o8,Optimizations.LoopInvariantCodeMotion,true
    -o8,Optimizations.BitTracker,true
    -o8,Optimizations.TwoPass,true
    -o8,Optimizations.Inline.Maximum,10

    -o9,Optimizations.Basic,true
    -o9,Optimizations.SSA,true
    -o9,Optimizations.ValueNumbering,true
    -o9,Optimizations.SCCP,true
    -o9,Optimizations.Devirtualization,true
    -o9,Optimizations.LongExpansion,true
    -o9,Optimizations.Platform,true
    -o9,Optimizations.Inline,true
    -o9,Optimizations.LoopInvariantCodeMotion,true
    -o9,Optimizations.BitTracker,true
    -o9,Optimizations.TwoPass,true
    -o9,Optimizations.Inline.Maximum,15

    --Max,Optimizations.Basic,true
    --Max,Optimizations.SSA,true
    --Max,Optimizations.ValueNumbering,true
    --Max,Optimizations.SCCP,true
    --Max,Optimizations.Devirtualization,true
    --Max,Optimizations.LongExpansion,true
    --Max,Optimizations.Platform,true
    --Max,Optimizations.Inline,true
    --Max,Optimizations.LoopInvariantCodeMotion,true
    --Max,Optimizations.BitTracker,true
    --Max,Optimizations.TwoPass,true
    --Max,Optimizations.Inline.Maximum,15

    --Size,Optimizations.Basic,true
    --Size,Optimizations.SSA,true
    --Size,Optimizations.ValueNumbering,true
    --Size,Optimizations.SCCP,true
    --Size,Optimizations.Devirtualization,true
    --Size,Optimizations.LongExpansion,true
    --Size,Optimizations.Platform,true
    --Size,Optimizations.Inline,true
    --Size,Optimizations.LoopInvariantCodeMotion,true
    --Size,Optimizations.BitTracker,true
    --Size,Optimizations.TwoPass,true
    --Size,Optimizations.Inline.Maximum,3

    --Fast,Optimizations.Basic,true
    --Fast,Optimizations.SSA,true
    --Fast,Optimizations.ValueNumbering,true
    --Fast,Optimizations.SCCP,false
    --Fast,Optimizations.Devirtualization,true
    --Fast,Optimizations.LongExpansion,false
    --Fast,Optimizations.Platform,false
    --Fast,Optimizations.Inline,false
    --Fast,Optimizations.LoopInvariantCodeMotion,false
    --Fast,Optimizations.BitTracker,false
    --Fast,Optimizations.TwoPass,false
    --Fast,Optimizations.Inline.Maximum,0

.. note:: ``{value}`` is the next argument