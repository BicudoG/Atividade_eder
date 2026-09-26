using UnityEngine;
using TMPro; // ← Necessário para TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Pontuação")]
    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;

    [Header("Valores")]
    public int pointsPerObject = 10;
    public int penaltyPoints = 15;

    [Header("UI - TextMeshPro")]
    public TextMeshProUGUI scoreTextP1;   // Arraste o TextMeshPro do Player 1
    public TextMeshProUGUI scoreTextP2;   // Arraste o TextMeshPro do Player 2

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(string playerTag)
    {
        if (playerTag == "Player1")
        {
            scorePlayer1 += pointsPerObject;
        }
        else if (playerTag == "Player2")
        {
            scorePlayer2 += pointsPerObject;
        }

        UpdateUI();
    }

    public void ApplyPenalty(string playerTag)
    {
        if (playerTag == "Player1")
        {
            scorePlayer1 -= penaltyPoints;
            if (scorePlayer1 < 0) scorePlayer1 = 0;
        }
        else if (playerTag == "Player2")
        {
            scorePlayer2 -= penaltyPoints;
            if (scorePlayer2 < 0) scorePlayer2 = 0;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreTextP1 != null)
            scoreTextP1.text = "Player 1: " + scorePlayer1;

        if (scoreTextP2 != null)
            scoreTextP2.text = "Player 2: " + scorePlayer2;
    }

    public void ResetScores()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        UpdateUI();
    }
}