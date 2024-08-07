using System.Diagnostics;
using System.IO;
using UnityEngine;
using System;

public class RunPythonScript : MonoBehaviour
{
    // Path to the Python script
    public string pythonScriptPath = "handGesture.py";
    private const string ShutdownFlagFile = "shutdown.flag";
    private Process pythonProcess;
    // Path to the virtual environment activation script
    public string venvActivationPath;
    void Awake()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        venvActivationPath = Path.Combine(appDataPath, "Meteor Mayhem", "PythonEnv", "Scripts", "activate.bat");
    }
    // Method to run the Python script
    public void runPythonScript()
    {
        string command = $"/C call \"{venvActivationPath}\" && python \"{pythonScriptPath}\"";
        UnityEngine.Debug.Log("Executing command: " + command);

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = command;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        pythonProcess = new Process();
        pythonProcess.StartInfo = startInfo;

        // Capture standard output and error
        pythonProcess.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("Python Output: " + args.Data);
        pythonProcess.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError("Python Error: " + args.Data);

        pythonProcess.Start();
        pythonProcess.BeginOutputReadLine();
        pythonProcess.BeginErrorReadLine();

        UnityEngine.Debug.Log("Python script started asynchronously.");
    }

    // Method to stop the Python script
    public void StopPythonScript()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            try
            {
                // Create the shutdown flag file
                File.Create(ShutdownFlagFile).Dispose();
                UnityEngine.Debug.Log("Shutdown signal sent to Python script.");

                // Wait for the Python script to exit
                if (pythonProcess.WaitForExit(5000))  // Wait up to 5 seconds
                {
                    UnityEngine.Debug.Log("Python script exited gracefully.");
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Python script did not exit in time. Forcing termination.");
                    pythonProcess.Kill();
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Error stopping Python script: {ex.Message}");
            }
            finally
            {
                // Clean up the flag file
                if (File.Exists(ShutdownFlagFile))
                {
                    File.Delete(ShutdownFlagFile);
                }
            }
        }
    }

    // Called when the script instance is being loaded
    void Start()
    {
        if (File.Exists(pythonScriptPath))
        {
            runPythonScript();
        }
        else
        {
            UnityEngine.Debug.LogError($"Python script not found: {pythonScriptPath}");
        }
    }

    // Called when the application is quitting
    void OnApplicationQuit()
    {
        StopPythonScript();
    }
}
