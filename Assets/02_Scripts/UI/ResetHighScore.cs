using UnityEngine;

public class ResetHighScore : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("HighScore"); // 최고 점수 초기화
        PlayerPrefs.Save();
        Debug.Log("최고 점수가 0으로 초기화되었습니다!");
    }
}
