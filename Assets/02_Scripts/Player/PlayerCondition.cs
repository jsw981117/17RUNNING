using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int maxHealth = 3; // 최대 체력
    public int currentHealth; // 현재 체력

    private void Awake()
    {
        PlayerManager.Instance.PlayerCondition = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthUI(currentHealth); // 초기 체력 UI 설정
    }

    private void OnTriggerEnter(Collider other)
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {

            Destroy(other.gameObject);
            potion.Use(gameObject);
        }
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
        UIManager.Instance.ShowGameOverUI();
        Destroy(gameObject);
    }


    public void Heal() // 생명력 회복 
    {
        currentHealth++;
        if (currentHealth> maxHealth) // 생명력 보정 3개이상이면 3으로 유지
        {
            currentHealth = maxHealth;
        }
    }

}
