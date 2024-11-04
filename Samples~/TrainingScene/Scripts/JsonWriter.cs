using System;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;


// -- JSON File Writer --
// Press the Rec. Pos. or Rec. Neg. buttons when scene is running to
// start recording positive or negative data.

// Upon pressing the buttons, recording will start after a delay of recordingStartDelay seconds
// and will continue for recordingDuration seconds.

// Each recording is saved to its own json file with name "{gestureName}_{timestamp}.json"
// in JsonData directory.

// JSON files currently saved at: /universal-gestures-unity/JsonData/{gestureName}.json
// JSON file will look like this:
// [
//    {"confidence":0, "handData":[...]},
//    {"confidence":1, "handData":[...]},
//    ...
// ]



public class JsonWriter : MonoBehaviour
{
    [SerializeField] private RecordingStatusUI recordingStatusUI;
    private RecordingStatus desiredRecordingStatus; // Recording status to start recording
    private float timeToStartRecording = -1; // Time to start recording (used to delay recording start)
    private float startRecordingTime; // Time when data recording started
    private string recordingFileName; // Name of file to save data to
    // recordingHandMode = OneHand to record data for one hand, TwoHands to record data for two hands
    public HandMode recordingHandMode = HandMode.TwoHands;
    public float recordingDuration = 10.0f; // Duration of recording in seconds
    public float recordingStartDelay = 3.0f; // Delay before recording starts
    public string gestureName;
    class GestureData
    {
        public int confidence; // confidence of gesture (label)
        public float[] handData; // float array of hand position data (features)
    }

    // JsonWrite(gestureData) writes gestureData to json file with name "{gestureName}.json" in JsonData directory.  If file doesn't exist, creates it.
    void JsonWrite(GestureData gestureData)
    {
        string prefix = ",\n    "; // Prefix & Suffix for each entry for proper json formatting
        string suffix = "\n]";
        string jsonDir = Application.dataPath + "/../JsonData/"; // Current directory to save json files
        // Check if running on Android
        if (Application.platform == RuntimePlatform.Android)
        {
            // Save to persistent data path on Android to avoid permission issues and persist data
            jsonDir = Application.persistentDataPath + "/JsonData/";
            // Create JsonData directory if it doesn't exist
            if (!Directory.Exists(jsonDir))
            {
                Directory.CreateDirectory(jsonDir);
            }
        }
        // record file name includes timestamp
        string path = jsonDir + recordingFileName;
        if (!File.Exists(path))
        {
            File.Create(path);
        }
        FileStream stream = new FileStream(path, FileMode.Open);
        if (stream.Length == 0)
        {
            prefix = "[\n    ";
        }
        stream.Position = Math.Max(stream.Length - 2, 0);
        string jsonString = prefix + JsonUtility.ToJson(gestureData) + suffix;
        byte[] insertBytes = Encoding.ASCII.GetBytes(jsonString);
        stream.Write(insertBytes);
        Debug.Log("Writing to " + gestureName + ".json: '" + jsonString + "'");
        stream.Close();
    }

    void LateUpdate()
    {
        // Start recording if timeToStartRecording is set and current time is greater than timeToStartRecording
        if (timeToStartRecording > 0 && Time.time >= timeToStartRecording)
        {
            StartRecording();
            timeToStartRecording = -1;
        }

        // Record data if recordingStatus is not NotRecording
        if (recordingStatusUI.recordingStatus != RecordingStatus.NotRecording)
        {
            GestureData gestureData = new GestureData();

            // Get hand data source, which depends on recordingHandMode
            if (recordingHandMode == HandMode.OneHand)
            {
                gestureData.handData = TestingSkeleton.handData;
            }
            else if (recordingHandMode == HandMode.TwoHands)
            {
                gestureData.handData = TestingSkeletonTwoHands.handData;
            }

            // Set confidence based on recordingStatus (positive or negative data)
            if (recordingStatusUI.recordingStatus == RecordingStatus.RecordingPositive)
            {
                gestureData.confidence = 1;
            }
            else if (recordingStatusUI.recordingStatus == RecordingStatus.RecordingNegative)
            {
                gestureData.confidence = 0;
            }

            JsonWrite(gestureData);

            // If time since recording started is greater than duration, stop recording
            if (Time.time - startRecordingTime >= recordingDuration)
            {
                StopRecording();
            }
        }
    }


    // Begins delay before positive data recording starts
    public void StartRecordingPositiveIntent()
    {
        desiredRecordingStatus = RecordingStatus.RecordingPositive;
        timeToStartRecording = Time.time + recordingStartDelay;
    }

    // Begins delay before negative data recording starts
    public void StartRecordingNegativeIntent()
    {
        desiredRecordingStatus = RecordingStatus.RecordingNegative;
        timeToStartRecording = Time.time + recordingStartDelay;
    }

    // Begins recording data
    public void StartRecording()
    {
        recordingStatusUI.recordingStatus = desiredRecordingStatus;
        recordingFileName = gestureName + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
        recordingStatusUI.targetFile = recordingFileName;
        startRecordingTime = Time.time;
    }

    // Stops recording data
    public void StopRecording()
    {
        recordingStatusUI.recordingStatus = RecordingStatus.NotRecording;
    }

    // Sets recording hand mode to one hand
    public void SetRecordingHandModeOneHand()
    {
        recordingHandMode = HandMode.OneHand;
    }

    // Sets recording hand mode to two hands
    public void SetRecordingHandModeTwoHands()
    {
        recordingHandMode = HandMode.TwoHands;
    }
}
