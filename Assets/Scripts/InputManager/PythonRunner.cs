using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UnityEngine;

public class PythonRunnerClass : MonoBehaviour
{
    private Process pythonProcess;
    string pythonScriptPath;
    string pythonCondaPath;

    public void Start()
    {
        // Start the Python process
        pythonCondaPath = System.Environment.GetEnvironmentVariable("PYTHON_CONDA_PATH");
        pythonScriptPath = System.Environment.GetEnvironmentVariable("PYTHON_SCRIPT_PATH");
        StartPythonProcess();
    }

    private void StartPythonProcess()
    {
        pythonProcess = new Process();
        pythonProcess.StartInfo.FileName =  pythonCondaPath;// Use the correct Python executable path if needed
        pythonProcess.StartInfo.Arguments = pythonScriptPath; // Specify your Python script
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.StartInfo.RedirectStandardOutput = true;
        pythonProcess.StartInfo.RedirectStandardError = true;
        pythonProcess.StartInfo.CreateNoWindow = true;
        pythonProcess.OutputDataReceived += PythonOutputHandler;
        pythonProcess.ErrorDataReceived += PythonOutputHandler;
        pythonProcess.Start();
        pythonProcess.BeginOutputReadLine();
        pythonProcess.BeginErrorReadLine();
    }

    private void PythonOutputHandler(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            print($"Python output: {e.Data}");
        }
    }

    // Other methods and variables specific to your game logic

    private void OnDestroy()
    {
        // Clean up when the script is destroyed
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
        }
    }
}
