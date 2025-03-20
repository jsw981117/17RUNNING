using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "LoadingScene"; // 전환할 씬 이름

    void Update()
    {
        if (Input.anyKeyDown) // 아무 키나 누르면
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName); // 씬 전환
    }
}