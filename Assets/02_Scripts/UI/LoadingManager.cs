using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용 시 필요

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // 로딩 바
    public TextMeshProUGUI loadingText; // 로딩 진행률 텍스트
    public TextMeshProUGUI tipText; // 팁을 표시할 텍스트 UI

    private string[] tips =
    {
        "Tip : Press the arrow keys to control the character's position!",
        "Tip : If the character touches an obstacle, their health will decrease!",
        "Tip : Items increase health and speed!"
    };

    void Start()
    {
        ShowRandomTip(); // 랜덤 팁 표시
        StartCoroutine(LoadGameScene());
    }

    void ShowRandomTip()
    {
        int randomIndex = Random.Range(0, tips.Length); // 0~2 사이의 랜덤 숫자 선택
        tipText.text = tips[randomIndex]; // 선택된 팁을 UI에 표시
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;

        float fakeProgress = 0f; // 가짜 진행도
        float minLoadingTime = 3f; // 최소 로딩 시간
        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            if (fakeProgress < realProgress)
            {
                fakeProgress = Mathf.Lerp(fakeProgress, realProgress, Time.deltaTime * 2f);
            }

            progressBar.value = fakeProgress; // 로딩 바 업데이트

            elapsedTime += Time.deltaTime;

            if (realProgress >= 0.9f && elapsedTime >= minLoadingTime)
            {
                yield return new WaitForSeconds(1f);

                while (progressBar.value < 1f)
                {
                    progressBar.value += Time.deltaTime;
                    yield return null;
                }

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}