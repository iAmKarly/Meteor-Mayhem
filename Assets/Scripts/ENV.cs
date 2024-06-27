using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string pythonCondaPath = @"C:\path\to\python.exe"; // Replace with the actual path
        string pythonScriptPath = @"D:\path\to\handGesture.py"; // Replace with the actual path
        System.Environment.SetEnvironmentVariable("PYTHON_CONDA_PATH", $"\"{pythonCondaPath}\"");
        System.Environment.SetEnvironmentVariable("PYTHON_SCRIPT_PATH", $"\"{pythonScriptPath}\"");
    }
}
