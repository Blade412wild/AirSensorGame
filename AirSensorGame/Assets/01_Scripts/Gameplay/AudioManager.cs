using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip startLevelAudioClip;
    [SerializeField] private PrototypeGameManager gameManager;
    [SerializeField] private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.PlayerExitedFirstRoom += HandlePlayerExcitedFirstRoom;
        gameManager.PlayerDead += PlayerDeadEvent;
        gameManager.ResetLevelEvent += PlayerDeadEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandlePlayerExcitedFirstRoom()
    {
        audioSource.Play();
    }

    private void PlayerDeadEvent()
    {
        audioSource.Stop();
    }

    private void OnDisable()
    {
        gameManager.PlayerExitedFirstRoom -= HandlePlayerExcitedFirstRoom;
        gameManager.PlayerDead -= PlayerDeadEvent;
    }
}
