using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용 시 필요

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // 로딩 바
    public TextMeshProUGUI loadingText; // 로딩 텍스트

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;
        float minLoadingTime = 3f; // 최소 로딩 시간
        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            // 실제 progress 값과 가짜 progress 값을 비교하여 증가
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            fakeProgress = Mathf.Lerp(fakeProgress, realProgress, Time.deltaTime * 2f);
            progressBar.value = fakeProgress;

            elapsedTime += Time.deltaTime;

            // 최소 로딩 시간이 지나기 전까지 씬 전환 X
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