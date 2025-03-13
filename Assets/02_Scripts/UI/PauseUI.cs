using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePanel; // 정지 UI 패널
    public Button pauseButton; // 정지 버튼
    public Button playButton; // 플레이 버튼
    public Image countdownImage; // 카운트다운 이미지 UI
    public Sprite[] countdownSprites; // 3, 2, 1 숫자 이미지 배열

    private bool isPaused = false; // 정지 여부

    private void Start()
    {
        pausePanel.SetActive(false); // 시작 시 정지 UI 숨김
        countdownImage.gameObject.SetActive(false); // 카운트다운 UI 숨김

        // 버튼 이벤트 등록
        pauseButton.onClick.AddListener(TogglePause);
        playButton.onClick.AddListener(StartCountdown);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        pausePanel.SetActive(false); // 정지 UI 숨기기
        countdownImage.gameObject.SetActive(true); // 카운트다운 UI 활성화

        for (int i = 0; i < countdownSprites.Length; i++)
        {
            countdownImage.sprite = countdownSprites[i]; // 이미지 변경
            yield return new WaitForSecondsRealtime(1f); // Time.timeScale = 0f 상태에서도 동작
        }

        countdownImage.gameObject.SetActive(false); // 카운트다운 UI 숨김
        ResumeGame();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        UIManager.Instance.UpdatePauseUI(false);
    }
}