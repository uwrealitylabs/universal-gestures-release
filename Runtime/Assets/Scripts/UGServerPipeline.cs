using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;
using Newtonsoft.Json;


public class UGServerPipeline : MonoBehaviour
{
    [SerializeField] string uri = "http://10.10.48.17:8080";
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] UGDataWriterScript writer;
    [SerializeField] UGInferenceRunnerScript inference;

    private string twoHandEndpoint = "train_model_two_hands/";
    private string oneHandEndpoint = "train_model_one_hand/";
    string apiUrl;
    private void Start()
    {
        apiUrl = uri;
    }

    public void SendTrainingData()
    {
        if (writer.recordingHandMode == HandMode.TwoHands)
        {
            apiUrl = uri + twoHandEndpoint;
        }
        else
        {
            apiUrl = uri + oneHandEndpoint;
        }
        StartCoroutine(SendJsonData());

    }
    IEnumerator SendJsonData()
    {
        // Load the JSON file from Resources (or wherever it's located)
        string jsonPath = writer.writePath;
        // Read the JSON file
        string jsonData = File.ReadAllText(jsonPath);
        // iterate over all but the last one which we read above
        for (int i = 0; i < writer.writePaths.Count - 1; i++)
        {
            jsonData = CombineJsonFiles(jsonData, writer.writePaths[i]);
        }

        // Create a UnityWebRequest POST request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // Set the request headers and body (content type is JSON)
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {

            // Get the binary data from the response (which is the file)
            byte[] fileData = request.downloadHandler.data;

            // Save the file to a directory
            string directory = Application.dataPath + "/../serverModels/";

            if (Application.platform == RuntimePlatform.Android)
            {
                // Save to persistent data path on Android to avoid permission issues and persist data
                directory = Application.persistentDataPath + "/serverModels/";
                // Create JsonData directory if it doesn't exist
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            string filePath = directory;
            if (writer.recordingHandMode == HandMode.TwoHands)
            {
                filePath += "two_hands.onnx";
            }
            else
            {
                filePath += "one_hand.onnx";
            }
            File.WriteAllBytes(filePath, fileData);


            // Debug.Log("File saved at: " + filePath);
            text.text = "File Downloaded! Loading Model";
            if (inference.LoadModel(filePath, writer.recordingHandMode))
            {
                text.text = "Loaded model!";

            }
            else
            {
                text.text = "Failed to load model!";
            }

        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }

    }


    string CombineJsonFiles(string data, string file2)
    {
        // Read the JSON files
        string json1 = data;
        string json2 = File.ReadAllText(file2);

        // Parse the JSON into lists
        List<object> list1 = JsonConvert.DeserializeObject<List<object>>(json1);
        List<object> list2 = JsonConvert.DeserializeObject<List<object>>(json2);

        // Combine the lists
        list1.AddRange(list2);

        // Convert back to a JSON string
        return JsonConvert.SerializeObject(list1, Formatting.Indented);
    }
    // clear the data buffer
    public void clearData()
    {
        writer.writePaths = new();
    }
}
