using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int maxHealth = 3; // 최대 체력
    public int currentHealth; // 현재 체력
    float accel = 5f;
    float originSpeed;
    bool isAccel = false;

    int playerLane { get => PlayerManager.Instance.PlayerController.lane; }
    PlayerMotionState playerMotionState { get => PlayerManager.Instance.PlayerController.motionState; }
    private void Awake()
    {
        PlayerManager.Instance.PlayerCondition = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
        // UIManager.Instance.UpdateHealthUI(currentHealth); // 초기 체력 UI 설정
    }

    private void OnTriggerEnter(Collider other)
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {
            Destroy(other.gameObject);
            potion.Use(gameObject); // 다형성 포션 호출 (스피드업, 체력)
        }
        if (other.CompareTag("Obstacle")) // 장애물과 충돌 시
        {
            IObstacle obstacle = other.GetComponent<IObstacle>();
            if (obstacle.lane == playerLane)
            {
                foreach (var v in obstacle.passable)
                {
                    if (v != playerMotionState)
                    {
                        //지나갈 수 없는 상태.
                        TakeDamage(1);
                        break;
                    }
                }
            }
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

    public void SpeedUp()
    {
        if (isAccel == false) // 가속이 아닐때 기본속도로 저장
        {
            isAccel = true; // 가속중
            originSpeed = PlayerManager.Instance.PlayerController.moveSpeed;
            PlayerManager.Instance.PlayerController.moveSpeed += accel; // 중첩 해결 어떻게?
        }
       
        CancelInvoke("AfterSpeedUp"); // 포션 배치가 연속일때 마지막포션 기준으로 3초 적용
        Debug.Log("가속적용" + PlayerManager.Instance.PlayerController.moveSpeed);
        Invoke("AfterSpeedUp", 3f);
    }

    public void AfterSpeedUp()
    {
        
        PlayerManager.Instance.PlayerController.moveSpeed = originSpeed; // 3초의 가속이 끝나면 기본속도로 복구
        Debug.Log("원래속도복구" + PlayerManager.Instance.PlayerController.moveSpeed);
        isAccel = false; 
    }
}
