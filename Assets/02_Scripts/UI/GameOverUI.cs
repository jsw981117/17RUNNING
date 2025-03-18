using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Summary: 게임 오버 UI를 관리하는 클래스
    // - 게임 오버 시 버튼 활성화
    // - 재시작 및 메인 메뉴 이동 기능 제공
    // - 일시정지 버튼 비활성화 처리

    public GameObject retryButton; // 재시작 버튼
    public GameObject homeButton; // 메인 메뉴 버튼
    public PauseUI pauseUI; // 일시정지 UI 참조

    public void ShowGameOver()
    {
        // Summary: 게임 오버 시 버튼 활성화 및 정지 버튼 비활성화
        retryButton.SetActive(true);
        homeButton.SetActive(true);

        if (pauseUI != null && pauseUI.pauseButton != null)
        {
            pauseUI.pauseButton.interactable = false; // 게임 오버 시 정지 버튼 비활성화
        }
    }

    public void RestartGame()
    {
        // Summary: 현재 씬을 다시 로드하여 게임 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        // Summary: 메인 메뉴 씬으로 이동 (씬 이름은 프로젝트에 맞게 변경)
        SceneManager.LoadScene("MainScene");
    }
}