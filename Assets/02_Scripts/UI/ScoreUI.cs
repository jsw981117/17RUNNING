using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;  // 싱글턴 패턴 적용
    private int score = 0;  // 점수 변수
    private float timer = 0f;  // 시간 측정을 위한 변수

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;  // 시간 누적

        if (timer >= 0.05f)  // 1초마다 점수 증가
        {
            score += 1;  // 점수 증가 (초당 10점)
            UIManager.Instance.UpdateScoreUI(score);  // UI 업데이트
            timer = 0f;  // 타이머 초기화
        }
    }
}