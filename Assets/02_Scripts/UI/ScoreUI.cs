using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private int score = 0;  // 점수 변수
    private float timer = 0f;  // 시간 측정을 위한 변수

    private void Update()
    {
        timer += Time.deltaTime;  // 시간 누적

        if (timer >= 0.05f)  
        {
            score += 1;
            UIManager.Instance.UpdateScoreUI(score);  // UI 업데이트
            timer = 0f;  // 타이머 초기화
        }
    }
}