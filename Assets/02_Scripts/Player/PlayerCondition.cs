using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int maxHealth = 3; // 최대 체력
    public int currentHealth; // 현재 체력
    float accel = 5f;
    float originSpeed;
    bool isAccel = false; // 가속중인가?
    public bool isInvincible = false; //무적효과인가?
    bool isCorotuine = false;
    Color playerColor;
    
    int playerLane { get => PlayerManager.Instance.PlayerController.lane; }
    PlayerMotionState playerMotionState { get => PlayerManager.Instance.PlayerController.motionState; }
    private void Awake()
    {
      
        playerColor = gameObject.GetComponentInChildren<MeshRenderer>().material.color;
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
            if(!isInvincible){
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

       /* if (other.gameObject.CompareTag("Enemy"))
        {
            Die();
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!isInvincible)
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
            isInvincible = true;
            originSpeed = PlayerManager.Instance.PlayerController.moveSpeed;
            PlayerManager.Instance.PlayerController.moveSpeed += accel; // 중첩 해결 어떻게?
           
        }

        //무적효과
        
        CancelInvoke("AfterSpeedUp"); // 포션 배치가 연속일때 마지막포션 기준으로 3초 적용

        if (!isCorotuine)
        {
            isCorotuine = true;
            StartCoroutine(Invincibility());
        }
        Debug.Log("가속적용" + PlayerManager.Instance.PlayerController.moveSpeed);
      
        Invoke("AfterSpeedUp", 3f);
    }

    public void AfterSpeedUp()
    {
        
        PlayerManager.Instance.PlayerController.moveSpeed = originSpeed; // 3초의 가속이 끝나면 기본속도로 복구
        Debug.Log("원래속도복구" + PlayerManager.Instance.PlayerController.moveSpeed);
        isAccel = false;
        isInvincible = false;
    }

    IEnumerator Invincibility()
    {
    
        // 주의 : Rendering Mode를 Transparent로 바꿀것
        bool isAlpha = false; // 투명도 체크

        float time = 0f;
        while (isInvincible)
        {
            if (isAlpha)
            {
                gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.2f);
                isAlpha = false;
            }
            else
            {
                gameObject.GetComponentInChildren<MeshRenderer>().material.color = playerColor;
                isAlpha = true; 
            }
          
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponentInChildren<MeshRenderer>().material.color = playerColor; // 깜빡임이 끝나면 원래색으로
        isCorotuine = false;
    }
}
