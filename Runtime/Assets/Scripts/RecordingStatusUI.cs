using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordingStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private TextMeshProUGUI targetFileText;
    [SerializeField] private GameObject dataWriterObject;
    private UGDataWriterScript dataWriter;


    void Start()
    {
        if (dataWriterObject == null)
        {
            Debug.LogError("Data writer object not set in RecordingStatusUI script.");
            gameObject.SetActive(false);
            return;
        }
        dataWriter = dataWriterObject.GetComponent<UGDataWriterScript>();
    }

    void Update()
    {
        labelText.text = dataWriter.recordingStatus.ToString();
        targetFileText.text = dataWriter.recordingFileName;
    }
}
