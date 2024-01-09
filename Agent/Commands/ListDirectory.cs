using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class ListDirectory : AgentCommand
    {
        public override string Name => "ls";
        public override string Execute(AgentTask task)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());

            // Create a single StringBuilder to store both directory and file information
            StringBuilder result = new StringBuilder();

            // Iterate through each subdirectory and append its information to the 'result' string
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                result.AppendLine($"{d.Name,-30}\t directory");
            }

            // Iterate through each file and append its information to the 'result' string
            foreach (FileInfo f in dir.GetFiles())
            {
                result.AppendLine($"{f.Name,-30}\t File");
            }

            return result.ToString();
        }
    }
}
