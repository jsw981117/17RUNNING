using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // Summary: 게임 UI를 관리하는 클래스
    // - 점수 및 최고 점수 UI 업데이트
    // - 체력 UI 업데이트
    // - 일시정지 및 게임 오버 UI 제어
    // - Singleton 패턴 적용

    public static UIManager Instance; // 싱글톤 인스턴스

    public TextMeshProUGUI scoreText; // 현재 점수 UI
    public TextMeshProUGUI highScoreText; // 최고 점수 UI
    public HealthUI healthUI; // 체력 UI 참조
    public PauseUI pauseUI; // 일시정지 UI 참조
    public GameOverUI gameOverUI; // 게임 오버 UI 참조

    public bool isGameOver = false; // 게임 오버 여부
    private int highScore = 0; // 최고 점수 변수

    [SerializeField] private GameObject achievePanel; // 업적 패널
    [SerializeField] private TextMeshProUGUI achieveTitleTxt; // 업적 제목 txt
    [SerializeField] private TextMeshProUGUI achieveDescTxt; // 업적 설명 txt


    private void Awake()
    {
        // Summary: 싱글톤 패턴 적용 (하나의 UIManager만 유지)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Summary: 저장된 최고 점수 불러오기 및 초기 UI 설정
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // 현재 씬이 MainScene이면 최고 점수를 항상 표시
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = "Top : " + highScore.ToString("D5");
        }
        else
        {
            highScoreText.gameObject.SetActive(false); // GameScene에서는 숨김
        }
    }

    public void UpdateScoreUI(int score)
    {
        // Summary: 점수 UI 업데이트 및 최고 점수 갱신
        if (!isGameOver)
        {
            scoreText.text = "Score : " + score.ToString("D5");

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore); // 최고 점수 저장
                PlayerPrefs.Save();
            }

            // 점수 업적 호출
            if (score == 1000)
                AchievementManager.instance.Achieve(2);
        }
    }

    public void UpdateHealthUI(int health)
    {
        // Summary: 체력 UI 업데이트
        if (healthUI != null)
        {
            healthUI.UpdateHealth(health);
        }
    }

    public void UpdatePauseUI(bool isPaused)
    {
        // Summary: 일시정지 UI 업데이트
        if (pauseUI != null)
        {
            pauseUI.pausePanel.SetActive(isPaused);
        }
    }

    public void ShowGameOverUI()
    {
        // Summary: 게임 오버 UI 표시 및 최고 점수 갱신
        if (gameOverUI != null)
        {
            isGameOver = true;
            gameOverUI.ShowGameOver();

            // 최고 점수 UI 활성화 및 갱신 (GameScene에서는 게임 오버 시 표시)
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = "Top : " + highScore.ToString("D5");

            Debug.Log("게임 오버! 최고 점수 표시");
        }
    }

    public IEnumerator SetAchievementUI(int listIdx)
    {
        List<Achievement> achievementData = AchievementManager.instance.achievementData;

        achievePanel.gameObject.SetActive(true);
        achieveTitleTxt.text = achievementData[listIdx].Name.ToString();
        achieveDescTxt.text = achievementData[listIdx].Description.ToString();

        yield return new WaitForSeconds(3f);

        achievePanel.gameObject.SetActive(false);
    }
}
