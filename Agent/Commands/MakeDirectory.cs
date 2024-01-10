using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class MakeDirectory : AgentCommand
    {
        public override string Name => "mkdir";
        public override string Execute(AgentTask task)
        {
            string path = task.Arguments[0];

            var directoryInfo = Directory.CreateDirectory(path);
            return $"{directoryInfo.FullName} created";


        }
    }
}
