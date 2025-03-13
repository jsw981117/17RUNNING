using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;


//�÷��̾� ����. ���������� �����̵� ������, �޸��� ������.
public enum PlayerMotionState
{
    Run,
    Jump,
    Slide
}


//����� �Է� ó��, �÷��̾� �¿� �̵� ���� ���� ó��.
public class PlayerController : MonoBehaviour
{
    //�̵� �ӵ�
    [HideInInspector] public float moveSpeed;
    //�Ʒ� �ʵ带 ���� �̵� �ӵ��� ����
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float speedBoostMultiplier;

    //�÷��̾��� ����. ����������, �����̵�������.
    public PlayerMotionState motionState;

    //���� �޸��� ����. ���� < �߾� < ������, �߾� = 0
    public int lane = 0;


    //���� �� ����
    public float laneDistance;
    //���� �̵��� �ɸ��� �ð�
    public float laneChangeTime;
    //�̵� �� ��� �̵��� ����
    public float laneChangeBlockTime;
    private bool canChangeLane = true;

    //���� ����
    public LayerMask groundLayerMask;
    public bool canJump;
    public float jumpMaxAlt;
    public float holdTimeAtMaxAlt;
    [SerializeField][ReadOnly(true)] private float jumpPower;

    //�����̵� ����
    private bool isSliding;


    Rigidbody _rigidbody;

    //�÷��̾� rigidbody , �߷°��ӵ� �����ϸ� �� �Լ��� ȣ��.
    private void CalculateJumpPower()
    {
        float m = _rigidbody.mass;
        float g = Physics.gravity.magnitude;
        //��ġ������: Ep = mgh
        //������ != ��
        //impulse, imp = N*s
        // imp = m * a * s = m * v
        //��ġ������-������� ������Ģ -> v = sqrt(2gh
        //���� m�� ��ü�� �� h �� �����ϱ� ���� �ʿ��� impulse
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
        //���� �پ�������
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

    #region ���� ���� ���� ����
    void MoveLane(int direction)
    {
        //���� ������ 3����.

        //���� ������ ���� �̵�, ������ ������ ������ �̵� �õ��� return
        if (lane == -1)
        {
            if (direction == -1) return;
        }
        else if (lane == 1)
        {
            if (direction == 1) return;
        }

        //�� �� ��� ���� ����.
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

        // lane * ������ �Ÿ� => ��ǥ ��ġ
        //float target = lane * laneDistance;

        float spd = laneDistance / laneChangeTime;
        while (elapsed < laneChangeTime)
        {
            //�� ���� ���� �ʿ�
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
            Debug.Log("����");
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
