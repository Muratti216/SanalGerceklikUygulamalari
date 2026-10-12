using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TMP_Text scoreText;
    int score;

    void Awake()
    {
        Instance = this;
        UpdateText();
    }

    public void Add(int points)
    {
        score += points;
        UpdateText();
    }

    void UpdateText()
    {
        if (scoreText) scoreText.text = "Skor: " + score;
    }
}