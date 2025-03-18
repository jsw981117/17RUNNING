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
    SkinnedMeshRenderer[] skinnedRenderers;
    Color playerColor;
    
    int playerLane { get => PlayerManager.Instance.PlayerController.lane; }
    PlayerMotionState playerMotionState { get => PlayerManager.Instance.PlayerController.motionState; }
    private void Awake()
    {
        skinnedRenderers= gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        playerColor = skinnedRenderers[0].materials[0].color;

        PlayerManager.Instance.PlayerCondition = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
        // UIManager.Instance.UpdateHealthUI(currentHealth); // 초기 체력 UI 설정
    }

    private void OnTriggerEnter(Collider other) // Summary * 충돌시 다형성을 통한 포션 호출
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {
            //SoundManager.instance.PlaySFX(SFX.ITEM);
            Destroy(other.gameObject);
            potion.Use(); // 다형성 포션 호출 (스피드업, 체력)
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
        SoundManager.instance.PlaySFX(SFX.CRASH);

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
        SoundManager.instance.PlaySFX(SFX.GAMEOVER);
        // 게임 오버 처리 가능 (씬 리로드, UI 표시 등)
        UIManager.Instance.ShowGameOverUI();
        StartCoroutine(PlayerManager.Instance.PlayerController.DieCoroutine());
    }


    public void Heal() // 생명력 회복 
    {
        currentHealth++;
        if (currentHealth> maxHealth) // 생명력 보정 3개이상이면 3으로 유지
        {
            currentHealth = maxHealth;
        }
        UIManager.Instance.UpdateHealthUI(currentHealth);
    }

    public void SpeedUp() // *Summary 체력포션 획득시 가속이 증가, 무적효과 연속 배치된 포션을 먹을 때 마지막 포션 기준으로 가속이 종료되게끔 구현
    {
        if (isAccel == false) // 가속이 아닐때 기본속도로 저장
        {
            isAccel = true; // 가속중
            isInvincible = true; // 무적중
            originSpeed = PlayerManager.Instance.PlayerController.moveSpeed; // 원래 속도 저장
            PlayerManager.Instance.PlayerController.moveSpeed += accel; // 이속증가
           
        }

        //무적효과
        
        CancelInvoke("AfterSpeedUp"); // 포션 배치가 연속일때 마지막포션 기준으로 3초 적용

        if (!isCorotuine)
        {
            isCorotuine = true;
            StartCoroutine(Invincibility()); // 코루틴이 실행되지 않았다면 코루틴 시작
        }
        Debug.Log("가속적용" + PlayerManager.Instance.PlayerController.moveSpeed);
      
        Invoke("AfterSpeedUp", 3f); // 3초후 가속을 종료
    }

    public void AfterSpeedUp()
    {
        
        PlayerManager.Instance.PlayerController.moveSpeed = originSpeed; // 3초의 가속이 끝나면 기본속도로 복구
        Debug.Log("원래속도복구" + PlayerManager.Instance.PlayerController.moveSpeed);
        isAccel = false; // 가속 종료
        isInvincible = false; // 무적 효과 종료
    }

    IEnumerator Invincibility() // Summary * 코루틴을 통해 무적효과 구현 0.2초마다 몸과 머리가 같이 불투명도를 조절하여 무적진입을 알림
    {
    
        // 주의 : Rendering Mode를 Transparent로 바꿀것
        bool isAlpha = false; // 투명도 체크

        while (isInvincible) // 무적효과일때
        {
            foreach (var renderer in skinnedRenderers) // SkinnedRenderer 머리, 몸통
            {
                // 여기서 material 여러 개 있을 수 있어서 for문 돌려줌
                Material[] mats = renderer.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    if (isAlpha) //  Alpha면
                        mats[i].color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.2f); //투명
                    else // 불투명하다면
                        mats[i].color = playerColor; // 원래 컬러로
                }
            }


            isAlpha = !isAlpha; 

         
            yield return new WaitForSeconds(0.2f); // 0.2초마다 while문 루프 돔
        }
        foreach (var renderer in skinnedRenderers) // 무적 종료후
        {
            Material[] mats = renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = playerColor; // 가속종료 후 몸통과 머리가 원래컬러로 돌아가기 위한 코드
            }
        }
        isCorotuine = false; // 코루틴 종료
    }
}
