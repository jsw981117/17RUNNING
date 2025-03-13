using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText; // 점수 UI
    public HealthUI healthUI; // HealthUI 참조

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score : " + score.ToString("D5");
    }

    public void UpdateHealthUI(int health)
    {
        if (healthUI != null)
        {
            healthUI.UpdateHealth(health); // HealthUI 스크립트에서 체력 UI 업데이트 실행
        }
    }
}