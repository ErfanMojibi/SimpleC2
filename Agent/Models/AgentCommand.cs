using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Models
{
    public abstract class AgentCommand
    {
        public string Name { get; set; }
        public abstract string Execute(AgentTask task);
    }
}
