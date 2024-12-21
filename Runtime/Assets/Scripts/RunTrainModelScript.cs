
using System;
using SD = System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq; // idk if its good to have this installed
using Unity.Barracuda;
using TMPro;

public class RunTrainModelScript : MonoBehaviour
{
    public void TrainModel()
    {
        // run python script from unity
        // python script is run here for testing purposes at the moment,
        // will be moved to a separate script later
        SD.ProcessStartInfo start = new SD.ProcessStartInfo();
        // change FileName to the path of your python3 executable (virtual env is recommended)
        start.FileName = "/Users/brianzhang/venv/bin/python3";
        start.Arguments = "Assets/Scripts/Python/model.py";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        using (SD.Process process = SD.Process.Start(start))
        {
            // forward the output of the python script to the unity console
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                // Debug.Log(result);
            }
        }
    }
}
