using UnityEngine;

public class HandAudioManager : MonoBehaviour
{
    [SerializeField] HandAudio leftHand;
    [SerializeField] HandAudio rightHand;

    [Space]
    [SerializeField] AudioClip[] clips;

    int counter;

    public bool playSound;

    public void PlaySound()
    {
        AudioClip clip = GetAudioClip();
        leftHand.PlaySound(clip);
        rightHand.PlaySound(clip);
    }

    public AudioClip GetAudioClip()
    {
        AudioClip clip = null;

        if (counter == clips.Length)
        {
            counter = 0;
        }

        clip = clips[counter];
        counter++;
        return clip;
    }


}
