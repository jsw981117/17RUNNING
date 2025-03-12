using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;  // �̱��� ���� ����
    private int score = 0;  // ���� ����
    private float timer = 0f;  // �ð� ������ ���� ����

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;  // �ð� ����

        if (timer >= 0.05f)  // 1�ʸ��� ���� ����
        {
            score += 1;  // ���� ���� (�ʴ� 10��)
            UIManager.Instance.UpdateScoreUI(score);  // UI ������Ʈ
            timer = 0f;  // Ÿ�̸� �ʱ�ȭ
        }
    }
}