using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ScaleFromMic : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private AudioloudnessDetection detector;

    public float Scalar = 100;
    public float threshold = 0.1f;
    public int iterations;

    private int counter;
    private List<float> loudnessListData = new List<float>();


    private void Update()
    {
        if (loudnessListData.Count < iterations)
        {
            float loudness = detector.GetLoudnessFromMic();
            loudnessListData.Add(loudness);
            counter++;
            return;
        }

        float avarageLoudnessFromSample = GetAvarage(loudnessListData) * Scalar;

        if (avarageLoudnessFromSample < threshold)
            avarageLoudnessFromSample = 0;

        transform.localScale = Vector3.Lerp(minScale, maxScale, avarageLoudnessFromSample);


        counter = 0;
        loudnessListData.Clear();
    }
    private float GetAvarage(List<float> list)
    {
        float total = 0;
        foreach (float data in list)
        {
            total += data;
        }

        return total / list.Count;
    }
}
