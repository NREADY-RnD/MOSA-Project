// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.Launcher
{
	public static class CheckOptions
	{
		public static string Verify(LauncherSettingsWrapper options)
		{
			if (options.Emulator != null && options.Emulator.ToLower() == "qemu" && options.ImageFormat.ToUpper() == "VDI")
			{
				return "QEMU does not support the VDI image format";
			}

			if (options.Emulator != null && options.Emulator.ToLower() == "bochs" && options.ImageFormat.ToUpper() == "VDI")
			{
				return "Bochs does not support the VDI image format";
			}

			if (options.Emulator != null && options.Emulator.ToLower() == "bochs" && options.ImageFormat.ToUpper() == "VMDK")
			{
				return "Bochs does not support the VMDK image format";
			}

			if (options.Emulator != null && options.Emulator.ToLower() == "vmware" && options.ImageFormat.ToUpper() == "IMG")
			{
				return "VMware does not support the IMG image format";
			}

			if (options.Emulator != null && options.Emulator.ToLower() == "vmware" && options.ImageFormat.ToUpper() == "VDI")
			{
				return "VMware does not support the VHD image format";
			}

			if (options.BootLoader.ToLower() == "grub0.97" && options.ImageFormat.ToUpper() != "ISO")
			{
				return "Grub boot loader does not support virtual disk formats";
			}

			if (options.BootLoader.ToLower() == "grub2.00" && options.ImageFormat.ToUpper() != "ISO")
			{
				return "Grub boot loader does not support virtual disk formats";
			}

			if (options.BootLoader.ToLower() == "syslinux6.03" && options.ImageFormat.ToUpper() != "ISO")
			{
				return "Syslinux boot loader v6.03 does not support virtual disk format";
			}

			if (options.PlatformType.ToLower() != "x86" && options.PlatformType.ToLower() != "x64")
			{
				return "Platform not supported";
			}

			return null;
		}
	}
}
