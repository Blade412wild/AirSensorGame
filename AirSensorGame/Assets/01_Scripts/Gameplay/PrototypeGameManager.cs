using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeGameManager : MonoBehaviour
{
    public event Action PlayerDead;
    public event Action<sbyte> PlayerHealthChanged;
    public event Action PlayerExitedFirstRoom;
    public event Action PlayerStartedGame;
    public event Action<sbyte> ScoreChanged;
    public event Action ResetLevelEvent;


    [SerializeField] private InputActionReference startlevelAction;

    [SerializeField] private MicrocontrollerManager microcontrollerManager;
    [SerializeField] private MovingLevel levelMoving;
    [SerializeField] private GameUIManager uiManager;
    [SerializeField] private PrototypePlayer player;
    [SerializeField] private ScoreManager scoreManager;

    [Space]
    [SerializeField] private TriggerArea ExitFirstRoomTrigger;

    public bool Reset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startlevelAction.action.Enable();
        startlevelAction.action.started += StartGame;
        CheckMicorControllerStatus();
        ExitFirstRoomTrigger.TriggerEvent += HandlePlayerExitFirstRoom;
        player.PlayerHit += HandlePlayerHitEvent;
        player.PlayerDied += HandplePlayerDeathEvent;
        scoreManager.ScoreChangeEvent += (x) => ScoreChanged?.Invoke(x);

    }

    // Update is called once per frame
    void Update()
    {
        if (Reset)
        {
            Reset = false;
            ResetLevel();
            ResetLevelEvent?.Invoke();
        }
    }
    private void StartGame(InputAction.CallbackContext obj)
    {
        startlevelAction.action.started -= StartGame;
        PlayerStartedGame?.Invoke();
    }

    private void CheckMicorControllerStatus()
    {
        if (microcontrollerManager.IsConnected()) return;
        microcontrollerManager.TryToConnect = true; //TODO laat de microcontroller in zijn eigen scene zijn
    }

    private void HandlePlayerExitFirstRoom(TriggerArea triggerArea)
    {
        ExitFirstRoomTrigger.TriggerEvent -= HandlePlayerExitFirstRoom;
        PlayerExitedFirstRoom?.Invoke();

    }

    private void HandplePlayerDeathEvent()
    {
        PlayerHealthChanged?.Invoke(0);
    }

    private void HandlePlayerHitEvent(sbyte health)
    {
        PlayerHealthChanged?.Invoke(health);
    }

    private void ResetLevel()
    {
        startlevelAction.action.started += StartGame;
        ExitFirstRoomTrigger.TriggerEvent += HandlePlayerExitFirstRoom;

    }


    private void OnDisable()
    {
        scoreManager.ScoreChangeEvent -= (x) => ScoreChanged?.Invoke(x);
        player.PlayerHit -= HandlePlayerHitEvent;
        player.PlayerDied -= HandplePlayerDeathEvent;

    }

}
