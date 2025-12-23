using UnityEngine;


public class HandAudio : MonoBehaviour
{
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
