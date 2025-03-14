using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // 최대 체력
    private int currentHealth; // 현재 체력

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthUI(currentHealth); // 초기 체력 UI 설정
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle")) // 장애물과 충돌 시
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;

        UIManager.Instance.UpdateHealthUI(currentHealth); // 체력 UI 갱신

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(" 플레이어 사망!");
        // 게임 오버 처리 가능 (씬 리로드, UI 표시 등)
        Destroy(gameObject);
    }
}
