using UnityEngine;

public class BreathingSystem : MonoBehaviour
{
    [SerializeField] private UnderWater water;
    [SerializeField] private BreathingDeviceData data;
    [SerializeField] private bool update;
    private bool isPlayerUnderwater;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        water.PlayerEnteredWaterEvent += () => isPlayerUnderwater = true;
        water.PlayerExitedWaterEvent += () => isPlayerUnderwater = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!update) return;
        if (!isPlayerUnderwater) return;

        if(data.BreathingState == BreathingState.inhaling)
        {
            Debug.Log("PlayerDied");
        }
    }

    private void OnDisable()
    {
        water.PlayerEnteredWaterEvent -= () => isPlayerUnderwater = true;
        water.PlayerExitedWaterEvent -= () => isPlayerUnderwater = false;
    }

}
