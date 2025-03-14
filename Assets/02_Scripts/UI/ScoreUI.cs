using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private int score = 0;  // 점수 변수
    private float timer = 0f;  // 시간 측정을 위한 변수

    private void Update()
    {
        // UIManager에서 게임 오버 상태 확인
        if (UIManager.Instance != null && UIManager.Instance.isGameOver)
        {
            return; // 게임 오버 시 점수 증가 중단
        }

        timer += Time.deltaTime;

        if (timer >= 0.05f)
        {
            score += 1;
            UIManager.Instance.UpdateScoreUI(score);
            timer = 0f;
        }
    }
}