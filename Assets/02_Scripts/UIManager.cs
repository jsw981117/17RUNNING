using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;  
    public TextMeshProUGUI scoreText;  

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score : " + score.ToString("D5");
    }
}