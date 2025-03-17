using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button playButton;
    public Image countdownImage;
    public Sprite[] countdownSprites;

    private bool isPaused = false;
    private Image pausePanelImage;

    private void Start()
    {
        pausePanelImage = pausePanel.GetComponent<Image>();
        pausePanel.SetActive(false);
        countdownImage.gameObject.SetActive(false);

        pauseButton.onClick.AddListener(TogglePause);
        playButton.onClick.AddListener(StartCountdown);
    }

    public void TogglePause()
    {
        SoundManager.instance.PlaySFX(SFX.BUTTON);
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

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
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        SetPanelTransparency(0.5f);
        countdownImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false); // 카운트다운 시작 시 버튼 숨김

        for (int i = 0; i < countdownSprites.Length; i++)
        {
            countdownImage.sprite = countdownSprites[i];
            yield return new WaitForSecondsRealtime(1f);
        }

        countdownImage.gameObject.SetActive(false);
        ResumeGame();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        UIManager.Instance.UpdatePauseUI(false);
        pausePanel.SetActive(false);
        playButton.gameObject.SetActive(false); // 게임 재개 시 버튼 숨김
    }

    private void SetPanelTransparency(float alpha)
    {
        if (pausePanelImage != null)
        {
            Color color = pausePanelImage.color;
            color.a = alpha;
            pausePanelImage.color = color;
        }
    }
}