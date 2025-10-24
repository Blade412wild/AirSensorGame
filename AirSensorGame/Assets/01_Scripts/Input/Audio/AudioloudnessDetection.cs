using UnityEngine;

public class AudioloudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip micClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MicrophoneToAudioClip();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MicrophoneToAudioClip()
    {
        string micName = Microphone.devices[0];

        string mics = "| ";
        for(int i = 0; i < Microphone.devices.Length; i++)
        {
            mics += Microphone.devices[i] + " | ";
        }

        Debug.Log("available mics : " + mics);
        

        Debug.Log("mic = " + micName);
        micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);

    }

    public float GetLoudnessFromMic()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
    }

    public float GetLoudnessFromAudioClip(int clipPostion, AudioClip clip)
    {
        int startPosition = clipPostion - sampleWindow;

        if (startPosition < 0)
            startPosition = 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        // compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }

}


