using System;
using SD = System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq; // idk if its good to have this installed
using Unity.Barracuda;
using TMPro;
using UnityEditor;
using Unity.Barracuda.ONNX;


// Replaces HandMode enum in JsonWriter.cs in release
// Enum for selecting which hand(s) the model should use for inference
public enum HandMode
{
    LeftHand,
    RightHand,
    TwoHands
}

public class UGInferenceRunnerScript : MonoBehaviour
{
    // Public parameters
    [Header("Setup")]
    public NNModel modelAsset;
    public HandMode inferenceHandMode;
    public GameObject dataExtractorObject; // the game object that has the data extractor script attached
    private UGDataExtractorScript dataExtractor; // the data extractor script, taken from dataExtractorObject
    public float inferenceInterval = 0.5f; // how often to run inference (in seconds)

    [Header("Run Function on Detection")]
    [SerializeField] private UnityEvent functionToRun;
    [Range(0.0f, 1.0f)]
    public float thresholdConfidenceLevel;
    public Boolean loopFunctionWhilePoseIsHeld;


    // Inference variables
    private Model m_RuntimeModel;
    private float inferenceTimer = 0;
    private IWorker worker;
    private Tensor inputTensor;
    private Tensor outputTensor;
    [HideInInspector]
    public float inferenceOutput;

    // Run function variables
    private Boolean eventTriggered = false;

    void Start()
    {
        dataExtractor = dataExtractorObject.GetComponent<UGDataExtractorScript>();
        bool configurationIsValid = ValidateConfiguration();
        if (!configurationIsValid)
        {
            Debug.LogError("UGInferenceRunnerScript: Configuration is not valid, inference will not run. See console logs for more information.");
            gameObject.SetActive(false);
            return;
        }

        SetupInference();
    }

    void Update()
    {
        // Run inference every inferenceInterval seconds
        inferenceTimer += Time.deltaTime;
        if (inferenceTimer >= inferenceInterval)
        {
            RunInference();
        }

        RunFunctionIfPoseDetected();
    }

    bool ValidateConfiguration()
    {
        return ValidateModel() && ValidateDataExtractor();
    }

    bool ValidateModel()
    {
        // Check if model is set
        if (modelAsset == null)
        {
            Debug.LogError("UGInferenceRunnerScript: modelAsset is not set.");
            return false;
        }
        return true;
    }

    bool ValidateDataExtractor()
    {
        // Check if dataExtractor is set and enabled for the correct hand mode
        if (dataExtractor == null)
        {
            Debug.LogError("UGInferenceRunnerScript: dataExtractor is not set.");
            return false;
        }
        if (inferenceHandMode == HandMode.LeftHand && !dataExtractor.leftHandDataEnabled)
        {
            Debug.LogError("UGInferenceRunnerScript: dataExtractor is not enabled for left hand data gathering.");
            return false;
        }
        else if (inferenceHandMode == HandMode.RightHand && !dataExtractor.rightHandDataEnabled)
        {
            Debug.LogError("UGInferenceRunnerScript: dataExtractor is not enabled for right hand data gathering.");
            return false;
        }
        else if (inferenceHandMode == HandMode.TwoHands && !dataExtractor.twoHandDataEnabled)
        {
            Debug.LogError("UGInferenceRunnerScript: dataExtractor is not enabled for two hands data gathering.");
            return false;
        }
        return true;
    }

    void SetupInference()
    {
        // Initialize model and worker needed to run inference
        // see docs for more information on this script: https://docs.unity3d.com/Packages/com.unity.barracuda%401.0/manual/GettingStarted.html
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);
        int modelInputSize;
        if (inferenceHandMode == HandMode.LeftHand || inferenceHandMode == HandMode.RightHand)
        {
            modelInputSize = UGDataExtractorScript.ONE_HAND_NUM_FEATURES;
        }
        else
        {
            modelInputSize = UGDataExtractorScript.TWO_HAND_NUM_FEATURES;
        }
        inputTensor = new Tensor(1, 0, 0, modelInputSize);
    }

    void OnDestroy()
    {
        // Cleanup resources
        inputTensor.Dispose();
        outputTensor.Dispose();
        worker.Dispose();
    }

    void RunInference()
    {
        inferenceTimer = 0;
        // select hand data based on inferenceHandMode
        float[] handData;
        if (inferenceHandMode == HandMode.LeftHand)
        {
            handData = dataExtractor.leftHandData;
        }
        else if (inferenceHandMode == HandMode.RightHand)
        {
            handData = dataExtractor.rightHandData;
        }
        else
        {
            handData = dataExtractor.twoHandsData;
        }
        // update input tensor with new hand data
        for (int i = 0; i < handData.Length; i++)
        {
            inputTensor[i] = handData[i];
        }
        worker.Execute(inputTensor);
        outputTensor = worker.PeekOutput();
        inferenceOutput = outputTensor[0];
        Debug.Log("Inference Output (UGInferenceRunnerScript): " + inferenceOutput);
    }

    void RunFunctionIfPoseDetected()
    {
        if (loopFunctionWhilePoseIsHeld)
        {
            // loops function while the pose is being held
            if (inferenceOutput >= thresholdConfidenceLevel)
            {
                functionToRun.Invoke();
            }
        }
        else
        {
            // triggers function once if the pose is detected
            // function can be triggered again only after the pose is not being held anymore
            if (!eventTriggered && inferenceOutput >= thresholdConfidenceLevel)
            {
                functionToRun.Invoke();
                eventTriggered = true;
            }
            else if (eventTriggered && inferenceOutput < thresholdConfidenceLevel)
            {
                eventTriggered = false;
            }
        }
    }
}
