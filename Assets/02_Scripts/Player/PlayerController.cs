using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;


//플레이어 상태. 점프중인지 슬라이드 중인지, 달리는 중인지.
public enum PlayerMotionState
{
    Run,
    Jump,
    Slide
}


//사용자 입력 처리, 플레이어 좌우 이동 등의 동작 처리.
public class PlayerController : MonoBehaviour
{
    //이동 속도
    [HideInInspector] public float moveSpeed;
    //아래 필드를 통해 이동 속도를 조절
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float speedBoostMultiplier;

    //플레이어의 상태. 점프중인지, 슬라이드중인지.
    public PlayerMotionState motionState;

    //지금 달리는 레인. 왼쪽 < 중앙 < 오른쪽, 중앙 = 0
    public int lane = 0;


    //레인 간 간격
    public float laneDistance;
    //레인 이동에 걸리는 시간
    public float laneChangeTime;
    //이동 후 잠시 이동을 막기
    public float laneChangeBlockTime;
    private bool canChangeLane = true;

    //점프 관련
    public LayerMask groundLayerMask;
    public bool canJump;
    public float jumpMaxAlt;
    public float holdTimeAtMaxAlt;
    [SerializeField][ReadOnly(true)] private float jumpPower;

    //슬라이드 관련
    private bool isSliding;


    Rigidbody _rigidbody;

    //플레이어 rigidbody , 중력가속도 수정하면 이 함수를 호출.
    private void CalculateJumpPower()
    {
        float m = _rigidbody.mass;
        float g = Physics.gravity.magnitude;
        //위치에너지: Ep = mgh
        //에너지 != 힘
        //impulse, imp = N*s
        // imp = m * a * s = m * v
        //위치에너지-운동에너지 보존법칙 -> v = sqrt(2gh
        //질량 m인 물체가 고도 h 에 도달하기 위해 필요한 impulse
        jumpPower = m * Mathf.Sqrt(2 * g * jumpMaxAlt);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        CalculateJumpPower();
        moveSpeed = defaultMoveSpeed;
    }

    private void Update()
    {
        //땅에 붙어있으면
        if (CheckGround())
        {
            if (isSliding)
            {
                motionState = PlayerMotionState.Slide;
            }
            else
            {
                motionState = PlayerMotionState.Run;
            }
        }
        else
        {
            motionState = PlayerMotionState.Jump;
        }
    }

    private void FixedUpdate()
    {
        MoveForward();
    }



    void MoveForward()
    {
        Vector3 vector3 = transform.forward * moveSpeed;
        vector3.y = _rigidbody.velocity.y;
        _rigidbody.velocity = vector3;
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    void Slide()
    {

    }

    #region 레인 변경 관련 로직
    void MoveLane(int direction)
    {
        //레인 갯수는 3으로.

        //왼쪽 끝에서 왼쪽 이동, 오른쪽 끝에서 오른쪽 이동 시도시 return
        if (lane == -1)
        {
            if (direction == -1) return;
        }
        else if (lane == 1)
        {
            if (direction == 1) return;
        }

        //그 외 경우 레인 변경.
        if (canChangeLane)
        {
            lane += direction;
            StartCoroutine(BlockLaneChangeCoroutine());
            StartCoroutine(MoveLaneCoroutine(direction));
        }
    }

    IEnumerator BlockLaneChangeCoroutine()
    {
        canChangeLane = false;
        yield return new WaitForSeconds(laneChangeBlockTime);
        canChangeLane = true;
    }

    IEnumerator MoveLaneCoroutine(int direction)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        // lane * 벌리는 거리 => 목표 위치
        //float target = lane * laneDistance;

        float spd = laneDistance / laneChangeTime;
        while (elapsed < laneChangeTime)
        {
            //이 로직 수정 필요
            transform.position += transform.right * spd * direction * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Vector3 vector = transform.right * laneDistance * lane;
        //vector = transform.position * transform.forward;
        //transform.position = vector;
    }
    #endregion;









    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started && motionState != PlayerMotionState.Jump)
        {
            Debug.Log("점프");
            Jump();
        }
    }
    public void OnSlideInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSliding = true;
            Slide();
        }
        else if (context.canceled)
        {
            isSliding = false;
        }
    }

    public void OnLeftInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MoveLane(-1);
        }
    }

    public void OnRightInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MoveLane(1);
        }
    }

    private bool CheckGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.02f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.02f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.02f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.02f) + (Vector3.up * 0.01f), Vector3.down)
        };
        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray, 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}
