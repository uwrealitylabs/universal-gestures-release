using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.Input;

public class TestingSkeleton : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject rightHandFeature;
    private FingerFeatureStateProvider rightFingersFeatureProvider;
    private TransformFeatureStateProvider handTransformFeatureProvider;
    private TransformConfig handTransformConfig;
    public Transform wristTransform;
    public TransformRecognizerActiveState transformRecognizerActiveState;
    public const int ONE_HAND_NUM_FEATURES = 21;
    public static float[] handData;

    // Start is called before the first frame update
    void Start()
    {
        // OVRSkeleton skeleton = rightHand.GetComponent<OVRSkeleton>();
        // int currentBones = skeleton.GetCurrentNumBones();
        // Debug.Log("Current number of bones: " + currentBones);

        handTransformConfig = new();

        handTransformFeatureProvider = rightHandFeature.GetComponent<TransformFeatureStateProvider>();
        rightFingersFeatureProvider = rightHandFeature.GetComponent<FingerFeatureStateProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        float indexFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl) ?? 0.0f;
        float indexFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Abduction) ?? 0.0f;
        float indexFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Flexion) ?? 0.0f;
        float indexFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Index, FingerFeature.Opposition) ?? 0.0f;

        float thumbFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Curl) ?? 0.0f;
        float thumbFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Thumb, FingerFeature.Abduction) ?? 0.0f;
        // Flexion, Opposition not available on thumb

        float middleFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Curl) ?? 0.0f;
        float middleFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Abduction) ?? 0.0f;
        float middleFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Flexion) ?? 0.0f;
        float middleFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Middle, FingerFeature.Opposition) ?? 0.0f;

        float ringFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Curl) ?? 0.0f;
        float ringFingerAbduction = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Abduction) ?? 0.0f;
        float ringFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Flexion) ?? 0.0f;
        float ringFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Ring, FingerFeature.Opposition) ?? 0.0f;

        float pinkyFingerCurl = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Curl) ?? 0.0f;
        // Pinky does not support abduction
        float pinkyFingerFlexion = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Flexion) ?? 0.0f;
        float pinkyFingerOpposition = rightFingersFeatureProvider.GetFeatureValue(HandFinger.Pinky, FingerFeature.Opposition) ?? 0.0f;

        float wristUp = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.WristUp) ?? 0.0f;
        // float wristDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.WristDown) ?? 0.0f;
        float palmUp = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmUp) ?? 0.0f;
        // float palmDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmDown) ?? 0.0f;
        float palmTowardsFace = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmTowardsFace) ?? 0.0f;
        // float palmAwayFromFace = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PalmAwayFromFace) ?? 0.0f;
        float fingersUp = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.FingersUp) ?? 0.0f;
        // float fingersDown = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.FingersDown) ?? 0.0f;
        // float pinchClear = handTransformFeatureProvider.GetFeatureValue(handTransformConfig, TransformFeature.PinchClear) ?? 0.0f;

        // Debug.Log("Wrist Up: " + wristUp + ", Palm Up: " + palmUp + ", Palm Towards Face: " + palmTowardsFace + ", Fingers Up: " + fingersUp + ", Pinch Clear: " + pinchClear);

        // Add values to JsonWriter.GestureData.handData
        handData = new[] {
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
            wristUp,
            palmUp,
            palmTowardsFace,
            fingersUp,
        };
    }
}
