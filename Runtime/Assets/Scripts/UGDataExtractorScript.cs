using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.Input;

public class UGDataExtractorScript : MonoBehaviour
{

    // Data sources
    [Header("Specify Data Sources")]
    public Hand leftHand;
    public Hand rightHand;
    public OVRHand leftOVRHand;
    public OVRHand rightOVRHand;
    private FingerFeatureStateProvider leftFingerFeatureStateProvider;
    private FingerFeatureStateProvider rightFingerFeatureStateProvider;

    // Data gathering settings - determines which features are analyzed
    [Header("Enable/Disable Data Gathering")]
    public bool leftHandDataEnabled = false;
    public bool rightHandDataEnabled = false;
    public bool twoHandDataEnabled = false;

    // Config for transform features
    private TransformConfig transformConfig;

    // Hand data output arrays - used by other scripts
    [HideInInspector]
    public float[] leftHandData;
    [HideInInspector]
    public float[] rightHandData;
    [HideInInspector]
    public float[] twoHandsData;

    // Constants
    public const int ONE_HAND_NUM_FEATURES = 17;
    public const int TWO_HAND_NUM_FEATURES = 44;


    void Start()
    {
        // Set up data sources
        bool setupSuccess = SetupAndValidateConfiguration();
        if (!setupSuccess)
        {
            Debug.LogError("UGDataExtractorScript: Data source setup failed. Disabling script.");
            gameObject.SetActive(false);
            return;
        }
        // Initialize transform config
        transformConfig = new();
    }

    void Update()
    {
        // Update data from enabled sources
        if (leftHandDataEnabled)
        {
            // comment out one hand transform features until they are implemented
            // TransformFeatureStateProvider leftTransformFeatureProvider = leftHandFeature.GetComponent<TransformFeatureStateProvider>();
            leftHandData = GetOneHandData(leftFingerFeatureStateProvider);
        }
        if (rightHandDataEnabled)
        {
            // TransformFeatureStateProvider rightTransformFeatureProvider = rightHandFeature.GetComponent<TransformFeatureStateProvider>();
            rightHandData = GetOneHandData(rightFingerFeatureStateProvider);
        }
        if (twoHandDataEnabled)
        {
            twoHandsData = GetTwoHandsData();
        }
    }

    bool SetupAndValidateConfiguration()
    {
        // ensure all required data sources are provided
        if (leftHand == null || rightHand == null || leftOVRHand == null || rightOVRHand == null)
        {
            Debug.LogError("UGDataExtractorScript: Data source setup failed. Ensure left hand, right hand, left OVR hand, and right OVR hand are provided.");
            return false;
        }

        // get finger feature state providers
        leftFingerFeatureStateProvider = leftHand.GetComponentInChildren<FingerFeatureStateProvider>();
        rightFingerFeatureStateProvider = rightHand.GetComponentInChildren<FingerFeatureStateProvider>();
        if (leftFingerFeatureStateProvider == null || rightFingerFeatureStateProvider == null)
        {
            Debug.LogError("UGDataExtractorScript: Data source setup failed. Ensure left hand and right hand have children with FingerFeatureStateProvider components.");
            return false;
        }
        return true;
    }

    private float[] GetOneHandData(FingerFeatureStateProvider fingersFeatureProvider)
    // comment out one hand transform features until they are implemented
    // private float[] GetOneHandData(FingerFeatureStateProvider fingersFeatureProvider, TransformFeatureStateProvider transformFeatureProvider)
    {

        float indexFingerCurl = fingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl) ?? 0.0f;
        float indexFingerAbduction = fingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Abduction) ?? 0.0f;
        float indexFingerFlexion = fingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Flexion) ?? 0.0f;
        float indexFingerOpposition = fingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Opposition) ?? 0.0f;

        float thumbFingerCurl = fingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Curl) ?? 0.0f;
        float thumbFingerAbduction = fingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Abduction) ?? 0.0f;
        // Flexion, Opposition not available on thumb

        float middleFingerCurl = fingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Curl) ?? 0.0f;
        float middleFingerAbduction = fingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Abduction) ?? 0.0f;
        float middleFingerFlexion = fingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Flexion) ?? 0.0f;
        float middleFingerOpposition = fingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Opposition) ?? 0.0f;

        float ringFingerCurl = fingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Curl) ?? 0.0f;
        float ringFingerAbduction = fingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Abduction) ?? 0.0f;
        float ringFingerFlexion = fingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Flexion) ?? 0.0f;
        float ringFingerOpposition = fingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Opposition) ?? 0.0f;

        float pinkyFingerCurl = fingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Curl) ?? 0.0f;
        // Pinky does not support abduction
        float pinkyFingerFlexion = fingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Flexion) ?? 0.0f;
        float pinkyFingerOpposition = fingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Opposition) ?? 0.0f;

        // comment out one hand transform features until they are implemented
        // float wristUp = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.WristUp) ?? 0.0f;
        // // float wristDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.WristDown) ?? 0.0f;
        // float palmUp = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.PalmUp) ?? 0.0f;
        // // float palmDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmDown) ?? 0.0f;
        // float palmTowardsFace = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.PalmTowardsFace) ?? 0.0f;
        // // float palmAwayFromFace = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmAwayFromFace) ?? 0.0f;
        // float fingersUp = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.FingersUp) ?? 0.0f;
        // // float fingersDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.FingersDown) ?? 0.0f;
        // // float pinchClear = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PinchClear) ?? 0.0f;

        // Debug.Log("Wrist Up: " + wristUp + ", Palm Up: " + palmUp + ", Palm Towards Face: " + palmTowardsFace + ", Fingers Up: " + fingersUp + ", Pinch Clear: " + pinchClear);

        float[] handData = new[] {
            thumbFingerCurl,
            thumbFingerAbduction,
            indexFingerCurl,
            indexFingerAbduction,
            indexFingerFlexion,
            indexFingerOpposition,
            middleFingerCurl,
            middleFingerAbduction,
            middleFingerFlexion,
            middleFingerOpposition,
            ringFingerCurl,
            ringFingerAbduction,
            ringFingerFlexion,
            ringFingerOpposition,
            pinkyFingerCurl,
            pinkyFingerFlexion,
            pinkyFingerOpposition,
            // comment out one hand transform features until they are implemented
            // wristUp,
            // palmUp,
            // palmTowardsFace,
            // fingersUp,
        };

        return handData;
    }

    private float[] GetTwoHandsData()
    {
        // ========================================
        // LEFT HAND FEATURES
        // ========================================
        float leftIndexFingerCurl = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl) ?? 0.0f;
        float leftIndexFingerAbduction = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Abduction) ?? 0.0f;
        float leftIndexFingerFlexion = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Flexion) ?? 0.0f;
        float leftIndexFingerOpposition = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Opposition) ?? 0.0f;

        float leftThumbFingerCurl = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Curl) ?? 0.0f;
        float leftThumbFingerAbduction = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Abduction) ?? 0.0f;
        // Flexion, Opposition not available on thumb

        float leftMiddleFingerCurl = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Curl) ?? 0.0f;
        float leftMiddleFingerAbduction = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Abduction) ?? 0.0f;
        float leftMiddleFingerFlexion = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Flexion) ?? 0.0f;
        float leftMiddleFingerOpposition = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Opposition) ?? 0.0f;

        float leftRingFingerCurl = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Curl) ?? 0.0f;
        float leftRingFingerAbduction = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Abduction) ?? 0.0f;
        float leftRingFingerFlexion = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Flexion) ?? 0.0f;
        float leftRingFingerOpposition = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Opposition) ?? 0.0f;

        float leftPinkyFingerCurl = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Curl) ?? 0.0f;
        // Pinky does not support abduction
        float leftPinkyFingerFlexion = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Flexion) ?? 0.0f;
        float leftPinkyFingerOpposition = leftFingerFeatureStateProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Opposition) ?? 0.0f;


        // ========================================
        // RIGHT HAND FEATURES
        // ========================================
        float rightIndexFingerCurl = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl) ?? 0.0f;
        float rightIndexFingerAbduction = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Abduction) ?? 0.0f;
        float rightIndexFingerFlexion = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Flexion) ?? 0.0f;
        float rightIndexFingerOpposition = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Opposition) ?? 0.0f;

        float rightThumbFingerCurl = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Curl) ?? 0.0f;
        float rightThumbFingerAbduction = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Abduction) ?? 0.0f;
        // Flexion, Opposition not available on thumb

        float rightMiddleFingerCurl = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Curl) ?? 0.0f;
        float rightMiddleFingerAbduction = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Abduction) ?? 0.0f;
        float rightMiddleFingerFlexion = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Flexion) ?? 0.0f;
        float rightMiddleFingerOpposition = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Opposition) ?? 0.0f;

        float rightRingFingerCurl = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Curl) ?? 0.0f;
        float rightRingFingerAbduction = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Abduction) ?? 0.0f;
        float rightRingFingerFlexion = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Flexion) ?? 0.0f;
        float rightRingFingerOpposition = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Opposition) ?? 0.0f;

        float rightPinkyFingerCurl = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Curl) ?? 0.0f;
        // Pinky does not support abduction
        float rightPinkyFingerFlexion = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Flexion) ?? 0.0f;
        float rightPinkyFingerOpposition = rightFingerFeatureStateProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Opposition) ?? 0.0f;


        // ========================================
        // TWO-HAND RELATIVE FEATURES
        // ========================================
        float leftX = leftOVRHand.transform.position[0];
        float leftY = leftOVRHand.transform.position[1];
        float leftZ = leftOVRHand.transform.position[2];
        float rightX = rightOVRHand.transform.position[0];
        float rightY = rightOVRHand.transform.position[1];
        float rightZ = rightOVRHand.transform.position[2];
        float xDiff = rightX - leftX;
        float yDiff = rightY - leftY;
        float zDiff = rightZ - leftZ;
        float distance = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);

        float leftRotationX = leftOVRHand.transform.rotation.eulerAngles[0];
        float leftRotationY = leftOVRHand.transform.rotation.eulerAngles[1];
        float leftRotationZ = leftOVRHand.transform.rotation.eulerAngles[2];
        float rightRotationX = rightOVRHand.transform.rotation.eulerAngles[0];
        float rightRotationY = rightOVRHand.transform.rotation.eulerAngles[1];
        float rightRotationZ = rightOVRHand.transform.rotation.eulerAngles[2];
        float rotationXDiff = rightRotationX - leftRotationX;
        float rotationYDiff = rightRotationY - leftRotationY;
        float rotationZDiff = rightRotationZ - leftRotationZ;
        float rotationXSin = Mathf.Sin(rotationXDiff);
        float rotationXCos = Mathf.Cos(rotationXDiff);
        float rotationYSin = Mathf.Sin(rotationYDiff);
        float rotationYCos = Mathf.Cos(rotationYDiff);
        float rotationZSin = Mathf.Sin(rotationZDiff);
        float rotationZCos = Mathf.Cos(rotationZDiff);
        // Debug.Log("Rotation X Diff: " + rotationXDiff + ", Rotation Y Diff: " + rotationYDiff + ", Rotation Z Diff: " + rotationZDiff);

        // Debug.Log("Index finger values: " + rightIndexFingerCurl + ", " + rightIndexFingerAbduction + ", " + rightIndexFingerFlexion + ", " + rightIndexFingerOpposition);

        float[] handData = new[] { rightThumbFingerCurl, rightThumbFingerAbduction, rightIndexFingerCurl, rightIndexFingerAbduction, rightIndexFingerFlexion, rightIndexFingerOpposition, rightMiddleFingerCurl, rightMiddleFingerAbduction, rightMiddleFingerFlexion, rightMiddleFingerOpposition, rightRingFingerCurl, rightRingFingerAbduction, rightRingFingerFlexion, rightRingFingerOpposition, rightPinkyFingerCurl, rightPinkyFingerFlexion, rightPinkyFingerOpposition,
                   leftThumbFingerCurl, leftThumbFingerAbduction, leftIndexFingerCurl, leftIndexFingerAbduction, leftIndexFingerFlexion, leftIndexFingerOpposition, leftMiddleFingerCurl, leftMiddleFingerAbduction, leftMiddleFingerFlexion, leftMiddleFingerOpposition, leftRingFingerCurl, leftRingFingerAbduction, leftRingFingerFlexion, leftRingFingerOpposition, leftPinkyFingerCurl, leftPinkyFingerFlexion, leftPinkyFingerOpposition,
                   xDiff, yDiff, zDiff, distance, rotationXSin, rotationXCos, rotationYSin, rotationYCos, rotationZSin, rotationZCos };
        return handData;
    }
}
