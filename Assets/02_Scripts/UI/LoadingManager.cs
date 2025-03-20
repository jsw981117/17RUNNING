using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용 시 필요

public class LoadingManager : MonoBehaviour
{
    // Summary: 게임 로딩 화면을 관리하는 클래스
    // - 로딩 바(progress bar) 업데이트
    // - 랜덤 팁 표시
    // - 비동기 씬 로딩 및 최소 로딩 시간 유지

    public Slider progressBar; // 로딩 바 UI
    public TextMeshProUGUI loadingText; // 로딩 진행률 텍스트
    public TextMeshProUGUI tipText; // 랜덤 팁 표시 텍스트

    private string[] tips =
    {
        "Tip : Press the arrow keys to control the character's position!",
        "Tip : If the character touches an obstacle, their health will decrease!",
        "Tip : Items increase health and speed!"
    };

    void Start()
    {
        ShowRandomTip(); // 랜덤 팁 표시
        StartCoroutine(LoadGameScene()); // 비동기 씬 로드 시작
    }

    void ShowRandomTip()
    {
        // Summary: 랜덤한 팁을 선택하여 UI에 표시
        int randomIndex = Random.Range(0, tips.Length);
        tipText.text = tips[randomIndex];
    }

    IEnumerator LoadGameScene()
    {
        // Summary: 비동기적으로 씬을 로드하고 진행도를 업데이트하는 코루틴

        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false; // 즉시 씬 전환 방지

        float fakeProgress = 0f; // 가짜 진행도
        float minLoadingTime = 3f; // 최소 로딩 시간
        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f); // 실제 로딩 진행도 계산

            // 진행도를 자연스럽게 증가
            if (fakeProgress < realProgress)
            {
                fakeProgress = Mathf.Lerp(fakeProgress, realProgress, Time.deltaTime * 2f);
            }

            progressBar.value = fakeProgress; // 로딩 바 업데이트
            elapsedTime += Time.deltaTime;

            // 90% 이상 로딩 + 최소 로딩 시간 경과 시 씬 전환 준비
            if (realProgress >= 0.9f && elapsedTime >= minLoadingTime)
            {
                yield return new WaitForSeconds(1f);

                // 로딩 바 100%까지 자연스럽게 증가
                while (progressBar.value < 1f)
                {
                    progressBar.value += Time.deltaTime;
                    yield return null;
                }

                operation.allowSceneActivation = true; // 씬 전환 실행

                // 배경 음악 변경
                SoundManager.instance.StopBGM();
                SoundManager.instance.PlayBGM(BGM.GAME);
            }

            yield return null;
        }
    }
}