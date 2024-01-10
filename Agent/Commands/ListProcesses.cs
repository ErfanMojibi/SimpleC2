using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;
using Agent.Native;

namespace Agent.Commands
{
    public class ListProcesses : AgentCommand
    {
        public override string Name => "ps";
        public override string Execute(AgentTask task)
        {
            var processes = Process.GetProcesses();
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{"ProcessName",-20}\t{"PID",-20}\t{"SessionId",-20}\t{"Owner", -20}\t{"Arch", -5}\t{"Path, -60"}");
            foreach (Process p in processes)
            {
                result.AppendLine($"{p.ProcessName,-20}\t {p.Id, -20}\t{p.SessionId,-20}\t{GetProcessOwner(p)}\t{GetProcessArch(p)}\t{GetProcessPath(p), -60}");
            }

            return result.ToString();
        }

        private string GetProcessPath(Process p)
        {
            try
            {
                return p.MainModule.FileName;
            }
            catch (Exception ex)
            {
                return "-";
            }
        }

        private string GetProcessOwner(Process process)
        {
            var hToken = IntPtr.Zero;

            try
            {
                if (!Native.Advapi.OpenProcessToken(process.Handle, DesiredAccess.TOKEN_ALL_ACCESS, out hToken))
                    return "-";

                var identity = new WindowsIdentity(hToken);
                return identity.Name;
            }
            catch
            {
                return "-";
            }
            finally
            {
                Native.Kernel32.CloseHandle(hToken);
            }
        }

        private string GetProcessArch(Process process)
        {
            try
            {
                var is64BitOS = Environment.Is64BitOperatingSystem;

                if (!is64BitOS)
                    return "x86";

                if (!Native.Kernel32.IsWow64Process(process.Handle, out var isWow64))
                    return "-";

                if (is64BitOS && isWow64)
                    return "x86";

                return "x64";
            }
            catch
            {
                return "-";
            }
        }
    }


}
}
