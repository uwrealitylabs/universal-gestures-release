using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnalyticsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject inferenceRunnerObject;
    private UGInferenceRunnerScript inferenceRunner;
    [SerializeField] private TextMeshProUGUI inferenceOutputText;
    [SerializeField] private TextMeshProUGUI thresholdConfidenceText;
    [SerializeField] private TextMeshProUGUI latencyText;
    [SerializeField] private TextMeshProUGUI CPUText;
    [SerializeField] private TextMeshProUGUI RAMUsageText;


    private void Start()
    {
        // Ensure data source is configured correctly
        if (inferenceRunnerObject == null)
        {
            Debug.LogError("Inference runner object not set in AnalyticsDisplay script.");
            gameObject.SetActive(false);
            return;
        }
        inferenceRunner = inferenceRunnerObject.GetComponent<UGInferenceRunnerScript>();
    }
    private void Update()
    {
        inferenceOutputText.text = "Inference Output: " + inferenceRunner.inferenceOutput;
        thresholdConfidenceText.text = "Threshold Confidence: " + inferenceRunner.thresholdConfidenceLevel;
    }
}
