using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    // Summary: 플레이어의 점수를 자동 증가시키는 클래스
    // - 일정 시간마다 점수를 증가시킴
    // - UIManager를 통해 점수 UI 업데이트
    // - 게임 오버 상태에서는 점수 증가 중단

    private int score = 0;  // 현재 점수
    private float timer = 0f;  // 점수 증가를 위한 타이머

    private void Update()
    {
        // Summary: 게임 오버 상태 확인 후 점수 증가 로직 실행
        if (UIManager.Instance != null && UIManager.Instance.isGameOver)
        {
            return; // 게임 오버 시 점수 증가 중단
        }

        timer += Time.deltaTime;

        // 일정 시간이 지나면 점수 증가
        if (timer >= 0.05f)
        {
            score += 1;
            UIManager.Instance.UpdateScoreUI(score);
            timer = 0f;
        }
    }
}