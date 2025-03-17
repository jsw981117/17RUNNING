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

        float fakeProgress = 0f; // 가짜 진행도
        float minLoadingTime = 3f; // 최소 로딩 시간
        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            // 실제 progress 값 (0~0.9)
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            //  fakeProgress가 realProgress보다 빨리 증가하지 않도록 설정
            if (fakeProgress < realProgress)
            {
                fakeProgress = Mathf.Lerp(fakeProgress, realProgress, Time.deltaTime * 2f);
            }

            progressBar.value = fakeProgress; // 로딩 바 업데이트

            elapsedTime += Time.deltaTime;

            // 최소 로딩 시간이 지나고, 실제 로딩이 끝났을 경우에만 씬 전환
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