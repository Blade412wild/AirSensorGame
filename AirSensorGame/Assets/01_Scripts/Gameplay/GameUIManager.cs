using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private PrototypeGameManager gamemanager;
    [SerializeField] private GameObject TutorialUI;
    [SerializeField] private TMPro.TMP_Text scroringText;
    [SerializeField] private GameObject[] hearts;

    private void Start()
    {
        gamemanager.PlayerStartedGame += HandleStartedGame;
        gamemanager.ScoreChanged += HandleScoreChanged;
        gamemanager.PlayerHealthChanged += HandlePlayerHealthChanged;
    }

    private void HandleStartedGame()
    {
        TutorialUI.SetActive(false);
    }
    private void HandlePlayerHealthChanged(sbyte health)
    {
        hearts[health -1].SetActive(false);
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
    }

}
