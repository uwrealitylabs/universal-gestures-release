using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.Input;

public class UGDataExtractorScript : MonoBehaviour
{

    // Data gathering settings - determines which features are analyzed
    [Header("Select Data to Gather")]
    public bool leftHandDataEnabled = false;
    public bool rightHandDataEnabled = false;
    public bool twoHandDataEnabled = false;

    // Data providers
    [Header("Input Hand Data Sources")]
    [Help("Note: it is possible not all of the input sources below are used/required.\nThe required properties depend on which data gathering is enabled above.\n\nLeft Hand Data Enabled: requires Left Hand Feature only.\nRight Hand Data Enabled: requires Right Hand Feature only.\nTwo Hand Data Enabled: requires both Left and Right Hand Feature, as well as Left and Right Hand Position Providers.")]
    public GameObject leftHandFeature;
    public GameObject rightHandFeature;
    public GameObject leftHandPositionProvider;
    public GameObject rightHandPositionProvider;

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
        // Validate configuration
        bool validConfig = ValidateConfiguration();
        if (!validConfig)
        {
            Debug.LogError("UGDataExtractorScript: Configuration is invalid. Deactivating.");
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
            FingerFeatureStateProvider leftFingersFeatureProvider = leftHandFeature.GetComponent<FingerFeatureStateProvider>();
            TransformFeatureStateProvider leftTransformFeatureProvider = leftHandFeature.GetComponent<TransformFeatureStateProvider>();
            leftHandData = GetOneHandData(leftFingersFeatureProvider, leftTransformFeatureProvider);
        }
        if (rightHandDataEnabled)
        {
            FingerFeatureStateProvider rightFingersFeatureProvider = rightHandFeature.GetComponent<FingerFeatureStateProvider>();
            TransformFeatureStateProvider rightTransformFeatureProvider = rightHandFeature.GetComponent<TransformFeatureStateProvider>();
            rightHandData = GetOneHandData(rightFingersFeatureProvider, rightTransformFeatureProvider);
        }
        if (twoHandDataEnabled)
        {
            FingerFeatureStateProvider leftFingersFeatureProvider = leftHandFeature.GetComponent<FingerFeatureStateProvider>();
            TransformFeatureStateProvider leftTransformFeatureProvider = leftHandFeature.GetComponent<TransformFeatureStateProvider>();
            FingerFeatureStateProvider rightFingersFeatureProvider = rightHandFeature.GetComponent<FingerFeatureStateProvider>();
            TransformFeatureStateProvider rightTransformFeatureProvider = rightHandFeature.GetComponent<TransformFeatureStateProvider>();
            twoHandsData = GetTwoHandsData(leftFingersFeatureProvider, rightFingersFeatureProvider, leftHandPositionProvider, rightHandPositionProvider);
        }
    }

    bool ValidateConfiguration()
    {
        // Make sure the required data sources are provided
        if (leftHandDataEnabled && leftHandFeature == null)
        {
            Debug.LogError("UGDataExtractorScript: Left Hand Feature is required for gathering left hand data.");
            return false;
        }
        if (rightHandDataEnabled && rightHandFeature == null)
        {
            Debug.LogError("UGDataExtractorScript: Right Hand Feature is required for gathering right hand data.");
            return false;
        }
        if (twoHandDataEnabled && (leftHandFeature == null || rightHandFeature == null || leftHandPositionProvider == null || rightHandPositionProvider == null))
        {
            Debug.LogError("UGDataExtractorScript: Left Hand Feature, Right Hand Feature, Left Hand Position Provider, and Right Hand Position Provider are required for gathering two hands data.");
            return false;
        }
        return true;
    }

    private float[] GetOneHandData(FingerFeatureStateProvider fingersFeatureProvider, TransformFeatureStateProvider transformFeatureProvider)
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

        float wristUp = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.WristUp) ?? 0.0f;
        // float wristDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.WristDown) ?? 0.0f;
        float palmUp = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.PalmUp) ?? 0.0f;
        // float palmDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmDown) ?? 0.0f;
        float palmTowardsFace = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.PalmTowardsFace) ?? 0.0f;
        // float palmAwayFromFace = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmAwayFromFace) ?? 0.0f;
        float fingersUp = transformFeatureProvider.GetFeatureValue(transformConfig, TransformFeature.FingersUp) ?? 0.0f;
        // float fingersDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.FingersDown) ?? 0.0f;
        // float pinchClear = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PinchClear) ?? 0.0f;

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
            // wristUp,
            // palmUp,
            // palmTowardsFace,
            // fingersUp,
        };

        return handData;
    }

    private float[] GetTwoHandsData(FingerFeatureStateProvider leftFingersFeatureProvider, FingerFeatureStateProvider rightFingersFeatureProvider, GameObject leftHandPositionProvider, GameObject rightHandPositionProvider)
    {
        // ========================================
        // LEFT HAND FEATURES
        // ========================================
        float leftIndexFingerCurl = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl) ?? 0.0f;
        float leftIndexFingerAbduction = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Abduction) ?? 0.0f;
        float leftIndexFingerFlexion = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Flexion) ?? 0.0f;
        float leftIndexFingerOpposition = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Opposition) ?? 0.0f;

        float leftThumbFingerCurl = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Curl) ?? 0.0f;
        float leftThumbFingerAbduction = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Abduction) ?? 0.0f;
        // Flexion, Opposition not available on thumb

        float leftMiddleFingerCurl = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Curl) ?? 0.0f;
        float leftMiddleFingerAbduction = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Abduction) ?? 0.0f;
        float leftMiddleFingerFlexion = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Flexion) ?? 0.0f;
        float leftMiddleFingerOpposition = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Opposition) ?? 0.0f;

        float leftRingFingerCurl = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Curl) ?? 0.0f;
        float leftRingFingerAbduction = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Abduction) ?? 0.0f;
        float leftRingFingerFlexion = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Flexion) ?? 0.0f;
        float leftRingFingerOpposition = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Opposition) ?? 0.0f;

        float leftPinkyFingerCurl = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Curl) ?? 0.0f;
        // Pinky does not support abduction
        float leftPinkyFingerFlexion = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Flexion) ?? 0.0f;
        float leftPinkyFingerOpposition = leftFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Opposition) ?? 0.0f;


        // ========================================
        // RIGHT HAND FEATURES
        // ========================================
        float rightIndexFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl) ?? 0.0f;
        float rightIndexFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Abduction) ?? 0.0f;
        float rightIndexFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Flexion) ?? 0.0f;
        float rightIndexFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Opposition) ?? 0.0f;

        float rightThumbFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Curl) ?? 0.0f;
        float rightThumbFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Abduction) ?? 0.0f;
        // Flexion, Opposition not available on thumb

        float rightMiddleFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Curl) ?? 0.0f;
        float rightMiddleFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Abduction) ?? 0.0f;
        float rightMiddleFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Flexion) ?? 0.0f;
        float rightMiddleFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Opposition) ?? 0.0f;

        float rightRingFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Curl) ?? 0.0f;
        float rightRingFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Abduction) ?? 0.0f;
        float rightRingFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Flexion) ?? 0.0f;
        float rightRingFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Opposition) ?? 0.0f;

        float rightPinkyFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Curl) ?? 0.0f;
        // Pinky does not support abduction
        float rightPinkyFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Flexion) ?? 0.0f;
        float rightPinkyFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Opposition) ?? 0.0f;


        // ========================================
        // TWO-HAND RELATIVE FEATURES
        // ========================================
        float leftX = leftHandPositionProvider.transform.position[0];
        float leftY = leftHandPositionProvider.transform.position[1];
        float leftZ = leftHandPositionProvider.transform.position[2];
        float rightX = rightHandPositionProvider.transform.position[0];
        float rightY = rightHandPositionProvider.transform.position[1];
        float rightZ = rightHandPositionProvider.transform.position[2];
        float xDiff = rightX - leftX;
        float yDiff = rightY - leftY;
        float zDiff = rightZ - leftZ;
        float distance = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);

        float leftRotationX = leftHandPositionProvider.transform.rotation.eulerAngles[0];
        float leftRotationY = leftHandPositionProvider.transform.rotation.eulerAngles[1];
        float leftRotationZ = leftHandPositionProvider.transform.rotation.eulerAngles[2];
        float rightRotationX = rightHandPositionProvider.transform.rotation.eulerAngles[0];
        float rightRotationY = rightHandPositionProvider.transform.rotation.eulerAngles[1];
        float rightRotationZ = rightHandPositionProvider.transform.rotation.eulerAngles[2];
        float rotationXDiff = rightRotationX - leftRotationX;
        float rotationYDiff = rightRotationY - leftRotationY;
        float rotationZDiff = rightRotationZ - leftRotationZ;
        float rotationXSin = Mathf.Sin(rotationXDiff);
        float rotationXCos = Mathf.Cos(rotationXDiff);
        float rotationYSin = Mathf.Sin(rotationYDiff);
        float rotationYCos = Mathf.Cos(rotationYDiff);
        float rotationZSin = Mathf.Sin(rotationZDiff);
        float rotationZCos = Mathf.Cos(rotationZDiff);
        Debug.Log("Rotation X Diff: " + rotationXDiff + ", Rotation Y Diff: " + rotationYDiff + ", Rotation Z Diff: " + rotationZDiff);

        // Debug.Log("Index finger values: " + rightIndexFingerCurl + ", " + rightIndexFingerAbduction + ", " + rightIndexFingerFlexion + ", " + rightIndexFingerOpposition);

        float[] handData = new[] { rightThumbFingerCurl, rightThumbFingerAbduction, rightIndexFingerCurl, rightIndexFingerAbduction, rightIndexFingerFlexion, rightIndexFingerOpposition, rightMiddleFingerCurl, rightMiddleFingerAbduction, rightMiddleFingerFlexion, rightMiddleFingerOpposition, rightRingFingerCurl, rightRingFingerAbduction, rightRingFingerFlexion, rightRingFingerOpposition, rightPinkyFingerCurl, rightPinkyFingerFlexion, rightPinkyFingerOpposition,
                   leftThumbFingerCurl, leftThumbFingerAbduction, leftIndexFingerCurl, leftIndexFingerAbduction, leftIndexFingerFlexion, leftIndexFingerOpposition, leftMiddleFingerCurl, leftMiddleFingerAbduction, leftMiddleFingerFlexion, leftMiddleFingerOpposition, leftRingFingerCurl, leftRingFingerAbduction, leftRingFingerFlexion, leftRingFingerOpposition, leftPinkyFingerCurl, leftPinkyFingerFlexion, leftPinkyFingerOpposition,
                   xDiff, yDiff, zDiff, distance, rotationXSin, rotationXCos, rotationYSin, rotationYCos, rotationZSin, rotationZCos };
        return handData;
    }
}
