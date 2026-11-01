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

    // "Skor: " +  yazuyordu 25.satırda ben sadece sayıyı yazdırmak istiyorum. Bu yüzden 25.satırdaki kodu değiştirdim.
    void UpdateText()
    {
        if (scoreText) scoreText.text = score.ToString();
    }
}