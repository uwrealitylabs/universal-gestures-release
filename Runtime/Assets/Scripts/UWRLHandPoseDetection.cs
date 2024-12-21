using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

// Duplicate: seems like this overlaps with JsonWriter.HandMode. We should merge them at some point.
enum NumHands
{
    OneHanded,
    TwoHanded
}
public class URWLHandPoseDetection : MonoBehaviour
{
    //dropdown list of # of hands required
    [SerializeField] NumHands numHands;

    [SerializeField] GameObject RightHand;
    [SerializeField] GameObject LeftHand;

    //add enum for dropdown list of recognizable hand gestures

    //field to attach gameobject with script to run function when gesture is recognized
    [SerializeField] private UnityEvent handGestureRecognized;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float thresholdConfidenceLevel;

    public Boolean loopFunctionWhilePoseIsHeld;


    private Boolean eventTriggered = false;

    private UniversalGesturesInference inference;

    private void Start()
    {
        inference = GetComponent<UniversalGesturesInference>();
    }
    private void Update()
    {
        if (loopFunctionWhilePoseIsHeld)
        {
            //loops function while the pose is being held
            if(inference.inferenceOutput >= thresholdConfidenceLevel)
            {
                handGestureRecognized.Invoke();
            }
        }
        else
        {
            //triggers function once if the pose is detected
            //  function can be triggered again only after the pose is not being held anymore
            if (!eventTriggered && inference.inferenceOutput >= thresholdConfidenceLevel)
            {
                handGestureRecognized.Invoke();
                eventTriggered = true;
            }else if(eventTriggered && inference.inferenceOutput < thresholdConfidenceLevel)
            {
                eventTriggered = false;
            }
        }
    }
    public float getThresholdConfidence()
    {
        return thresholdConfidenceLevel;
    }
}
