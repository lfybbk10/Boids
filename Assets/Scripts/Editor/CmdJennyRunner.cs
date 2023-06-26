#if UNITY_EDITOR

using System.Diagnostics;
using UnityEditor;

public static class CmdJennyRunner
{
    private const string TERMINAL_APPLICATION_NAME = "cmd.exe";
    private const string TERMINAL_GENERATE_PROMPT = "dotnet Jenny/Jenny.Generator.Cli.dll gen";
    private const string TERMINAL_EXIT_PROMPT = "exit";
    
    [MenuItem(("Tools/Run Jenny Terminal Prompt"))]
    private static void RunJennyTerminalPrompt()
    {
        using Process jennyTerminalProcess = new();
        ProcessStartInfo processStartInfo = new()
        {
            FileName = TERMINAL_APPLICATION_NAME,
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
            
        jennyTerminalProcess.StartInfo = processStartInfo;
        jennyTerminalProcess.Start();
        jennyTerminalProcess.StandardInput.WriteLine(TERMINAL_GENERATE_PROMPT);
        jennyTerminalProcess.StandardInput.WriteLine(TERMINAL_EXIT_PROMPT);
        jennyTerminalProcess.WaitForExit();
    }
}

#endif