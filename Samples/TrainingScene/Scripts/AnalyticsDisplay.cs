using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnalyticsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject scripts;
    [SerializeField] private TextMeshProUGUI inferenceOutputText;
    [SerializeField] private TextMeshProUGUI thresholdConfidenceText;
    [SerializeField] private TextMeshProUGUI latencyText;
    [SerializeField] private TextMeshProUGUI CPUText;
    [SerializeField] private TextMeshProUGUI RAMUsageText;

    private UniversalGesturesInference inference;
    private URWLHandPoseDetection confidence;

    private void Start()
    {
        inference = scripts.GetComponent<UniversalGesturesInference>();
        confidence = scripts.GetComponent<URWLHandPoseDetection>();
    }
    private void Update()
    {
        inferenceOutputText.text = "Inference Output: " + inference.inferenceOutput;
        thresholdConfidenceText.text = "Threshold Confidence: " + confidence.getThresholdConfidence();
    }
}
