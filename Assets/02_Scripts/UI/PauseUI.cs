using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{
    // Summary: 게임의 일시정지 및 재개 UI를 관리하는 클래스
    // - 게임을 일시정지하거나 재개할 수 있음
    // - 3초 카운트다운 후 게임 재개
    // - UI 투명도 조절 및 버튼 제어

    public GameObject pausePanel; // 일시정지 UI 패널
    public Button pauseButton; // 일시정지 버튼
    public Button playButton; // 재개 버튼
    public Image countdownImage; // 카운트다운 이미지
    public Sprite[] countdownSprites; // 카운트다운 스프라이트 배열

    private bool isPaused = false; // 게임이 일시정지 상태인지 여부
    private Image pausePanelImage; // 패널 배경 이미지

    private void Start()
    {
        // Summary: UI 초기 설정 및 버튼 클릭 이벤트 등록
        pausePanelImage = pausePanel.GetComponent<Image>();
        pausePanel.SetActive(false); // 초기에는 일시정지 패널 숨김
        countdownImage.gameObject.SetActive(false); // 카운트다운 이미지 숨김

        pauseButton.onClick.AddListener(TogglePause);
        playButton.onClick.AddListener(StartCountdown);
    }

    public void TogglePause()
    {
        // Summary: 게임 일시정지 또는 재개
        SoundManager.instance.PlaySFX(SFX.BUTTON);
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // 시간 멈춤 또는 재개

        if (isPaused)
        {
            pausePanel.SetActive(true);
            SetPanelTransparency(0.5f);
            playButton.gameObject.SetActive(true); // 정지 시 시작 버튼 활성화
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public void StartCountdown()
    {
        // Summary: 게임 재개 전 3초 카운트다운 시작
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        // Summary: 3초 카운트다운 후 게임 재개
        SetPanelTransparency(0.5f);
        countdownImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false); // 카운트다운 시작 시 버튼 숨김

        for (int i = 0; i < countdownSprites.Length; i++)
        {
            countdownImage.sprite = countdownSprites[i]; // 카운트다운 이미지 변경
            yield return new WaitForSecondsRealtime(1f); // 시간 멈춘 상태에서도 진행
        }

        countdownImage.gameObject.SetActive(false);
        ResumeGame();
    }

    public void ResumeGame()
    {
        // Summary: 게임 재개
        isPaused = false;
        Time.timeScale = 1f;
        UIManager.Instance.UpdatePauseUI(false);
        pausePanel.SetActive(false);
        playButton.gameObject.SetActive(false); // 게임 재개 시 버튼 숨김
    }

    private void SetPanelTransparency(float alpha)
    {
        // Summary: 일시정지 패널의 투명도 조절
        if (pausePanelImage != null)
        {
            Color color = pausePanelImage.color;
            color.a = alpha;
            pausePanelImage.color = color;
        }
    }
}