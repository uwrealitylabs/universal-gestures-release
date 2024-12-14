using System;
using SD = System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq; // idk if its good to have this installed
using Unity.Barracuda;
using TMPro;
using UnityEditor;
using Unity.Barracuda.ONNX;



// UniversalGesturesInference.cs
// This script is used to load a trained neural network model and run inference on hand data.



public class UniversalGesturesInference : MonoBehaviour
{
    public NNModel modelAsset;
    private Model m_RuntimeModel;
    public GameObject handObject;
    private float inferenceTimer = 0;
    public float inferenceInterval = 0.5f; // how often to run inference (in seconds)
    public HandMode inferenceHandMode = HandMode.TwoHands; // whether to run inference using data from one hand or two hands
    private IWorker worker;
    public string modelName;
    private Dictionary<string, float[,]> weights;
    private Tensor inputTensor;
    private Tensor outputTensor;
    public float inferenceOutput { get; private set; }
    //  [SerializeField] private TextMeshProUGUI inferenceText; //moved to AnalyticsDisplay

    void Start()
    {
        // see docs for more information on this script: https://docs.unity3d.com/Packages/com.unity.barracuda%401.0/manual/GettingStarted.html
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);
        int modelInputSize;
        if (inferenceHandMode == HandMode.RightHand || inferenceHandMode == HandMode.LeftHand)
        {
            modelInputSize = TestingSkeleton.ONE_HAND_NUM_FEATURES;
        }
        else
        {
            modelInputSize = TestingSkeletonTwoHands.TWO_HAND_NUM_FEATURES;
        }
        inputTensor = new Tensor(1, 0, 0, modelInputSize);
    }

    void Update()
    {
        inferenceTimer += Time.deltaTime;
        // only run inference every inferenceInterval seconds
        if (inferenceTimer >= inferenceInterval)
        {
            inferenceTimer = 0;
            // select hand data based on inferenceHandMode
            float[] handData;
            if (inferenceHandMode == HandMode.RightHand || inferenceHandMode == HandMode.LeftHand)
            {
                handData = TestingSkeleton.handData;
            }
            else
            {
                handData = TestingSkeletonTwoHands.handData;
            }
            // update input tensor with new hand data
            for (int i = 0; i < handData.Length; i++)
            {
                inputTensor[i] = handData[i];
            }
            worker.Execute(inputTensor);
            outputTensor = worker.PeekOutput();
            inferenceOutput = outputTensor[0];
            Debug.Log("Inference Output: " + inferenceOutput);
            //inferenceText.text = "Inference Output: " + inferenceOutput; //moved to AnalyticsDisplay
        }
    }

    void OnDestroy()
    {
        inputTensor.Dispose();
        outputTensor.Dispose();
        worker.Dispose();
    }

    // load model at runtime
    public void LoadModel(string filePath)
    {
        // Start loading the NNModel asset using Addressables

        // Check if file exists
        if (File.Exists(filePath))
        {

            var nnModel = LoadNNModel(filePath, "name");



            var loadedModel = ModelLoader.Load(nnModel);


            // Dispose of the existing worker if necessary
            if (worker != null)
            {
                worker.Dispose();
            }

            // Set the loaded model as the runtime model and create a new worker
            m_RuntimeModel = loadedModel;
            worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);

            Debug.Log("Model loaded successfully from: " + filePath);
        }
        else
        {
            Debug.LogError("Model file not found at path: " + filePath);
        }

    }
    NNModel LoadNNModel(string modelPath, string modelName)
    {
        var converter = new ONNXModelConverter(true);
        Model model = converter.Convert(modelPath);
        NNModelData modelData = ScriptableObject.CreateInstance<NNModelData>();
        using (var memoryStream = new MemoryStream())
        using (var writer = new BinaryWriter(memoryStream))
        {
            ModelWriter.Save(writer, model);
            modelData.Value = memoryStream.ToArray();
        }
        modelData.name = "Data";
        modelData.hideFlags = HideFlags.HideInHierarchy;
        NNModel result = ScriptableObject.CreateInstance<NNModel>();
        result.modelData = modelData;
        result.name = modelName;
        return result;
    }



    // ============================================================
    // Below are deprecated methods for manual inference
    // ============================================================
    void LoadWeights(string filePath)
    {
        try
        {
            string jsonString = File.ReadAllText(filePath);
            JObject json = JObject.Parse(jsonString);

            weights = new Dictionary<string, float[,]>();

            foreach (var item in json)
            {
                string key = item.Key;

                JArray valueArray = (JArray)item.Value;
                int rows = valueArray.Count;
                int cols = valueArray[0].Count();
                float[,] valueMatrix = new float[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        valueMatrix[i, j] = (float)valueArray[i][j];
                    }
                }

                weights[key] = valueMatrix;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading weights: " + e.Message);
        }
    }

    public float GetInference()
    {
        float[] inputVector = CreateInputVector();
        // Debug.Log("Input Vector: " + inputVector);
        float output = RunInference(inputVector);
        return output;
    }

    float[] CreateInputVector()
    {
        float[] inputVector = TestingSkeleton.handData;
        // Print out inputVector to see if it's correct
        for (int i = 0; i < inputVector.Length; i++)
        {
            Debug.Log("Input Vector[" + i + "]: " + inputVector[i]);
        }
        return inputVector;
    }

    float RunInference(float[] inputVector)
    {
        // again also assuming a two-layer neural network, can change to be general if needed (did this based off test data)
        float[] fc1Output = new float[weights["fc1.weight"].GetLength(0)];

        for (int i = 0; i < fc1Output.Length; i++)
        {
            for (int j = 0; j < inputVector.Length; j++)
            {
                fc1Output[i] += inputVector[j] * weights["fc1.weight"][i, j];
            }
            fc1Output[i] = (float)Math.Tanh(fc1Output[i]); // Activation function (not too sure, test to see if it works)
        }

        float[] fc2Output = new float[weights["fc2.weight"].GetLength(0)];

        for (int i = 0; i < fc2Output.Length; i++)
        {
            for (int j = 0; j < fc1Output.Length; j++)
            {
                fc2Output[i] += fc1Output[j] * weights["fc2.weight"][i, j];
            }
            fc2Output[i] = (float)Math.Tanh(fc2Output[i]); // Activation function (not sure same as above)
        }

        return fc2Output[0];
    }
}
