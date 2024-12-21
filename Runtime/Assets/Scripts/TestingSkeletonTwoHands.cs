using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.Input;


// TestingSkeletonTwoHands.cs
// This script is used to expose the finger features of the left and right hands,
// as well as the relative features between the two hands.


public class TestingSkeletonTwoHands : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject leftHandFeature;
    public GameObject rightHand;
    public GameObject rightHandFeature;
    private FingerFeatureStateProvider leftFingersFeatureProvider;
    private FingerFeatureStateProvider rightFingersFeatureProvider;
    public const int TWO_HAND_NUM_FEATURES = 44;
    public static float[] handData;

    // Start is called before the first frame update
    void Start()
    {
        // OVRSkeleton skeleton = rightHand.GetComponent<OVRSkeleton>();
        // int currentBones = skeleton.GetCurrentNumBones();
        // Debug.Log("Current number of bones: " + currentBones);

        leftFingersFeatureProvider = leftHandFeature.GetComponent<FingerFeatureStateProvider>();
        rightFingersFeatureProvider = rightHandFeature.GetComponent<FingerFeatureStateProvider>();
    }

    // Update is called once per frame
    void Update()
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
        float leftX = leftHand.transform.position[0];
        float leftY = leftHand.transform.position[1];
        float leftZ = leftHand.transform.position[2];
        float rightX = rightHand.transform.position[0];
        float rightY = rightHand.transform.position[1];
        float rightZ = rightHand.transform.position[2];
        float xDiff = rightX - leftX;
        float yDiff = rightY - leftY;
        float zDiff = rightZ - leftZ;
        float distance = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);

        float leftRotationX = leftHand.transform.rotation.eulerAngles[0];
        float leftRotationY = leftHand.transform.rotation.eulerAngles[1];
        float leftRotationZ = leftHand.transform.rotation.eulerAngles[2];
        float rightRotationX = rightHand.transform.rotation.eulerAngles[0];
        float rightRotationY = rightHand.transform.rotation.eulerAngles[1];
        float rightRotationZ = rightHand.transform.rotation.eulerAngles[2];
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

        // Add values to JsonWriter.GestureData.handData
        handData = new[] { rightThumbFingerCurl, rightThumbFingerAbduction, rightIndexFingerCurl, rightIndexFingerAbduction, rightIndexFingerFlexion, rightIndexFingerOpposition, rightMiddleFingerCurl, rightMiddleFingerAbduction, rightMiddleFingerFlexion, rightMiddleFingerOpposition, rightRingFingerCurl, rightRingFingerAbduction, rightRingFingerFlexion, rightRingFingerOpposition, rightPinkyFingerCurl, rightPinkyFingerFlexion, rightPinkyFingerOpposition,
                   leftThumbFingerCurl, leftThumbFingerAbduction, leftIndexFingerCurl, leftIndexFingerAbduction, leftIndexFingerFlexion, leftIndexFingerOpposition, leftMiddleFingerCurl, leftMiddleFingerAbduction, leftMiddleFingerFlexion, leftMiddleFingerOpposition, leftRingFingerCurl, leftRingFingerAbduction, leftRingFingerFlexion, leftRingFingerOpposition, leftPinkyFingerCurl, leftPinkyFingerFlexion, leftPinkyFingerOpposition,
                   xDiff, yDiff, zDiff, distance, rotationXSin, rotationXCos, rotationYSin, rotationYCos, rotationZSin, rotationZCos };
    }
}
