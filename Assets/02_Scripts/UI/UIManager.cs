using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText; // 점수 UI
    public HealthUI healthUI; // HealthUI 참조
    public PauseUI pauseUI;
    public GameOverUI gameOverUI;

    public bool isGameOver = false; // 게임 오버 상태 변수

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScoreUI(int score)
    {
        if (!isGameOver) // 게임 오버 상태면 점수 업데이트 중단
        {
            scoreText.text = "Score : " + score.ToString("D5");
        }
    }

    public void UpdateHealthUI(int health)
    {
        if (healthUI != null)
        {
            healthUI.UpdateHealth(health);
        }
    }

    public void UpdatePauseUI(bool isPaused)
    {
        if (pauseUI != null)
        {
            pauseUI.pausePanel.SetActive(isPaused);
        }
    }

    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            isGameOver = true; // 게임 오버 상태 설정
            gameOverUI.ShowGameOver();
            Debug.Log("게임 오버! 점수 증가 중단");
        }
    }
}