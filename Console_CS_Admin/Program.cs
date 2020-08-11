using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
/*
讓C#程式以系統管理員身分執行(UAC)
資料來源:http://www.dotblogs.com.tw/as15774/archive/2012/02/02/67563.aspx
新增-應用程式資訊清單檔案(app.manifest)
    然後看到有一行
        <requestedExecutionLevel level="asInvoker" uiAccess="false" />
    將改成這樣
        <requestedExecutionLevel level="requireAdministrator" uiAccess="false" />
 
*/
namespace Console_CS_Admin
{
    class Program
    {
        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }
        static void Main(string[] args)
        {
            ExecuteCommand("123.bat");
            Pause();
        }
        static void Pause()
        {
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
