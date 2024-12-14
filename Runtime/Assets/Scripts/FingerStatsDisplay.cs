using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FingerStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshObjects; // Array of TextMeshPro objects
    public GameObject dataExtractorObject;
    private UGDataExtractorScript dataExtractor;


    void Start()
    {
        // Ensure both arrays are of the same length
        if (textMeshObjects.Length != TestingSkeleton.ONE_HAND_NUM_FEATURES)
        {
            Debug.LogError("TextMeshPro objects and hand data arrays must be of the same length!");
            gameObject.SetActive(false);
            return;
        }

        // Ensure data source is configured correctly
        if (dataExtractorObject == null)
        {
            Debug.LogError("Data extractor object not set in FingerStatsDisplay script.");
            gameObject.SetActive(false);
            return;
        }
        dataExtractor = dataExtractorObject.GetComponent<UGDataExtractorScript>();
        if (!dataExtractor.rightHandDataEnabled)
        {
            Debug.LogError("Right hand data not enabled in UGDataExtractorScript. Right hand data is used by default in the FingerStatsDisplay script, please enable in data extractor for FingerStatsDisplay to work.");
            gameObject.SetActive(false);
            return;
        }
    }

    void LateUpdate()
    {
        // Update each TextMeshPro object with the corresponding value
        for (int i = 0; i < textMeshObjects.Length; i++)
        {
            textMeshObjects[i].text = System.Math.Round(dataExtractor.rightHandData[i], 2).ToString();
        }
    }

}
