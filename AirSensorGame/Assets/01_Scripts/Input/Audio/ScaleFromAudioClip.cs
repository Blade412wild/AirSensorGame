using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private AudioloudnessDetection detector;

    public float Scalar = 100;
    public float threshold = 0.1f;


    private void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(audioSource.timeSamples, audioSource.clip) * Scalar;
        if (loudness < threshold)
            loudness = 0;

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }

}
