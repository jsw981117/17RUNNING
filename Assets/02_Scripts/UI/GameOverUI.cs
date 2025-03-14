using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject retryButton;
    public GameObject homeButton;
    public PauseUI pauseUI;

    public void ShowGameOver()
    {
        retryButton.SetActive(true);
        homeButton.SetActive(true);

        if (pauseUI != null && pauseUI.pauseButton != null)
        {
            pauseUI.pauseButton.interactable = false; // 게임 오버 시 정지 버튼 비활성화
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainScene"); // 메인 메뉴 씬으로 이동 (메뉴 씬 이름에 맞게 변경)
    }
}