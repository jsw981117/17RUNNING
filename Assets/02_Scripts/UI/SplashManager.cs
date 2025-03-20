using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f); // 3초 동안 로고 표시
        SceneManager.LoadScene("MainScene"); // 로딩 씬으로 이동
    }
}