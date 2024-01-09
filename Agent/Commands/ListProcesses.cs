using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class ListProcesses : AgentCommand
    {
        public override string Name => "ps";
        public override string Execute(AgentTask task)
        {
            var processes = Process.GetProcesses();
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{"ProcessName",-20}\t{"PID",-20}\t{"SessionId",-20}");
            foreach (Process p in processes)
            {
                result.AppendLine($"{p.ProcessName,-20}\t {p.Id, -20}\t{p.SessionId,-20}\t{GetProcessPath(p), -30}");
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
                return "";
            }
        }

    }
}
