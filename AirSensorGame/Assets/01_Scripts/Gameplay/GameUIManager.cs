using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private PrototypeGameManager gamemanager;
    [SerializeField] private GameObject TutorialUI;
    [SerializeField] private TMPro.TMP_Text scroringText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject arrow;

    private void Start()
    {
        gamemanager.PlayerStartedGame += HandleStartedGame;
        gamemanager.ScoreChanged += HandleScoreChanged;
        gamemanager.PlayerHealthChanged += HandlePlayerHealthChanged;
        gamemanager.PlayerExitedFirstRoom += () => arrow.SetActive(false);
    }

    private void HandleStartedGame()
    {
        TutorialUI.SetActive(false);
    }
    private void HandlePlayerHealthChanged(sbyte health)
    {
        if (health >= hearts.Length || health < 0) return;
        hearts[health].SetActive(false);
    }

    private void HandleScoreChanged(sbyte score)
    {
        scroringText.text = score.ToString();
    }

    private void OnDisable()
    {
        gamemanager.PlayerStartedGame -= HandleStartedGame;
        gamemanager.ScoreChanged -= HandleScoreChanged;
        gamemanager.PlayerHealthChanged -= HandlePlayerHealthChanged;
        gamemanager.PlayerExitedFirstRoom -= () => arrow.SetActive(false);

    }

}
