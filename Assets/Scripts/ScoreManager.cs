using TMPro;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TMP_Text scoreText;
    public int CurrentScore { get; private set; }
    public event Action<int> ScoreChanged;

    int score;

    void Awake()
    {
        Instance = this;
        CurrentScore = score;
        UpdateText();
    }

    public void Add(int points)
    {
        score += points;
        CurrentScore = score;
        UpdateText();
        ScoreChanged?.Invoke(CurrentScore);
    }

    // "Skor: " +  yazuyordu 25.satırda ben sadece sayıyı yazdırmak istiyorum. Bu yüzden 25.satırdaki kodu değiştirdim.
    void UpdateText()
    {
        if (scoreText) scoreText.text = score.ToString();
    }
}