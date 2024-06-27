using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string pythonCondaPath = @"C:\Users\Carlos\miniconda3\envs\FYP\python.exe"; // Replace with the actual path
        string pythonScriptPath = @"D:\School\FYP CSCI321\REPO\Meteor-Mayhem\Assets\Scripts\InputManager\handGesture.py"; // Replace with the actual path
        System.Environment.SetEnvironmentVariable("PYTHON_CONDA_PATH", $"\"{pythonCondaPath}\"");
        System.Environment.SetEnvironmentVariable("PYTHON_SCRIPT_PATH", $"\"{pythonScriptPath}\"");
    }
}
