using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText; // 현재 점수 UI
    public TextMeshProUGUI highScoreText; // 최고 점수 UI
    public HealthUI healthUI; // HealthUI 참조
    public PauseUI pauseUI;
    public GameOverUI gameOverUI;

    public bool isGameOver = false; // 게임 오버 상태 변수
    private int highScore = 0; // 최고 점수 변수

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); // 저장된 최고 점수 불러오기
        CheckSceneForHighScore(); // 씬에 따라 최고 점수 UI 표시 여부 결정
    }

    private void Update()
    {
        // 씬이 바뀌었을 때 최고 점수 UI 표시 여부 확인
        CheckSceneForHighScore();
    }

    public void UpdateScoreUI(int score)
    {
        if (!isGameOver) // 게임 오버 상태면 점수 업데이트 중단
        {
            scoreText.text = "Score : " + score.ToString("D5");

            // 최고 점수 갱신
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore); // 최고 점수 저장
                PlayerPrefs.Save();
            }
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

            // 게임 오버 시 최고 점수 UI 활성화
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = "Top : " + highScore.ToString("D5");

            Debug.Log("게임 오버! 최고 점수 표시");
        }
    }

    private void CheckSceneForHighScore()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // 게임 오버 화면이거나 특정 씬에서는 항상 최고 점수 표시
        if (currentScene == "MainScene" || currentScene == "GameOverScene")
        {
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = "Top : " + highScore.ToString("D5");
        }
        else if (!isGameOver) // 게임 진행 중에는 숨김
        {
            highScoreText.gameObject.SetActive(false);
        }
    }
}