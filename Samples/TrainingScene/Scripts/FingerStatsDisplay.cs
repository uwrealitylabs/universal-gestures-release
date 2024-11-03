using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FingerStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshObjects; // Array of TextMeshPro objects

    void Start()
    {
        // Ensure both arrays are of the same length
        if (textMeshObjects.Length != TestingSkeleton.ONE_HAND_NUM_FEATURES)
        {
            Debug.LogError("TextMeshPro objects and hand data arrays must be of the same length!");
            return;
        }
    }

    void LateUpdate()
    {
        // Update each TextMeshPro object with the corresponding value
        for (int i = 0; i < textMeshObjects.Length; i++)
        {
            textMeshObjects[i].text = System.Math.Round(TestingSkeleton.handData[i], 2).ToString();
        }
    }
}
