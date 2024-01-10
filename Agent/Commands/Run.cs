using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class Run : AgentCommand
    {
        public override string Name => "run";
        public override string Execute(AgentTask task)
        {
            string fileName = task.Arguments[0];
            string args = string.Join(" ", task.Arguments.Skip(1));

            return Utils.Execute.ExecuteCommand(fileName, args);
        }
    }
}
