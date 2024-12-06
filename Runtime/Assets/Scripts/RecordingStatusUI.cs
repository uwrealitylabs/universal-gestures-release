using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum RecordingStatus
{
    NotRecording,
    RecordingNegative,
    RecordingPositive
}

public class RecordingStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private TextMeshProUGUI targetFileText;

    public RecordingStatus recordingStatus;
    public string targetFile;


    void Update()
    {
        labelText.text = recordingStatus.ToString();
        targetFileText.text = targetFile;
    }
}
