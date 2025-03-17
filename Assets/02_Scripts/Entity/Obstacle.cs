using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour, IObstacle
{
    [Tooltip("장애물 통과 조건")]
    [SerializeField] private PlayerMotionState[] _passableStates;

    [Header("Lane Settings")]
    [SerializeField] private bool useLeftLane = false;
    [SerializeField] private bool useCenterLane = true;
    [SerializeField] private bool useRightLane = false;

    [SerializeField] private float leftLanePosition = -4f;  // 왼쪽 레인의 X 위치
    [SerializeField] private float centerLanePosition = 0f; // 중앙 레인의 X 위치
    [SerializeField] private float rightLanePosition = 4f;  // 오른쪽 레인의 X 위치

    private int[] _lanes = new int[0];
    private int _activeLaneCount = 0;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private int damage = 1;

    [Header("Collider Settings")]
    [SerializeField] private float laneWidth = 1.0f;
    [SerializeField] private float laneSpacing = 0.2f;

    [SerializeField] private float colliderHeight = 1.0f;
    [SerializeField] private float colliderDepth = 0.5f;

    [SerializeField] private float colliderYOffset = 0.5f; // 바닥으로부터의 높이

    private BoxCollider boxCollider;
    public PlayerMotionState[] passable => _passableStates;
    public int lane => _lanes.Length > 0 ? _lanes[0] : 0;
    public int[] lanes => _lanes;

    private void Awake()
    {
        // BoxCollider 가져오기
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        // 레인 배열 초기화
        UpdateLanesFromBooleans();
    }

    private void Start()
    {
        boxCollider.isTrigger = true;
        UpdateColliderShape();
        UpdatePositionBasedOnLanes();
    }

    private void OnValidate()
    {
        UpdateLanesFromBooleans();

        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        if (boxCollider != null)
        {
            UpdateColliderShape();
        }

        UpdatePositionBasedOnLanes();
    }

    /// <summary>
    /// 선택된 레인에 따라 오브젝트의 position.x 바꿔줌
    /// </summary>
    private void UpdatePositionBasedOnLanes()
    {
        if (_lanes.Length == 0)
        {
            return;
        }

        Vector3 currentPosition = transform.position;

        if (useLeftLane && !useCenterLane && !useRightLane)
        {
            currentPosition.x = leftLanePosition;
        }
        else if (!useLeftLane && useCenterLane && !useRightLane)
        {
            currentPosition.x = centerLanePosition;
        }
        else if (!useLeftLane && !useCenterLane && useRightLane)
        {
            currentPosition.x = rightLanePosition;
        }
        else if (_lanes.Length > 1)
        {
            float totalX = 0f;
            int count = 0;

            if (useLeftLane) { totalX += leftLanePosition; count++; }
            if (useCenterLane) { totalX += centerLanePosition; count++; }
            if (useRightLane) { totalX += rightLanePosition; count++; }

            if (count > 0)
            {
                currentPosition.x = totalX / count;
            }
        }

        transform.position = currentPosition;
    }

    /// <summary>
    /// Boolean 값에서 레인 배열 업데이트
    /// </summary>
    private void UpdateLanesFromBooleans()
    {
        List<int> lanesList = new List<int>();

        if (useLeftLane) lanesList.Add(-1);
        if (useCenterLane) lanesList.Add(0);
        if (useRightLane) lanesList.Add(1);

        if (lanesList.Count == 0)
        {
            lanesList.Add(0);
            useCenterLane = true;
        }

        _lanes = lanesList.ToArray();
        _activeLaneCount = lanesList.Count;
    }

    /// <summary>
    /// 레인 값에 따라 콜라이더 크기와 위치 업데이트
    /// </summary>
    private void UpdateColliderShape()
    {
        if (_lanes == null || _lanes.Length == 0)
        {
            _lanes = new int[] { 0 };
            _activeLaneCount = 1;
            useCenterLane = true;
        }

        float totalWidth = (_activeLaneCount * laneWidth) + ((_activeLaneCount - 1) * laneSpacing);

        float centerX = 0f;

        boxCollider.size = new Vector3(totalWidth, colliderHeight, colliderDepth);
        boxCollider.center = new Vector3(centerX, colliderYOffset, 0);
    }

    /// <summary>
    /// 레인 값 변경 후 콜라이더 업데이트 (외부에서 호출 가능)
    /// </summary>
    public void SetLanes(bool left, bool center, bool right)
    {
        useLeftLane = left;
        useCenterLane = center;
        useRightLane = right;

        UpdateLanesFromBooleans();
        UpdateColliderShape();
        UpdatePositionBasedOnLanes();
    }


    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 레이어인지 확인하고 컴포넌트 가져오고
        // PlayerMotionState를 확인해서 통과 가능한지 체크
        // 통과 가능하면 통과되고 아니면 데미지
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                bool isInSameLane = false;
                foreach (int obstacleLane in _lanes)
                {
                    if (obstacleLane == playerController.lane)
                    {
                        isInSameLane = true;
                        break;
                    }
                }

                if (isInSameLane)
                {
                    PlayerMotionState currentPlayerState = playerController.motionState;
                    bool canPass = false;
                    foreach (PlayerMotionState state in _passableStates)
                    {
                        if (state == currentPlayerState)
                        {
                            canPass = true;
                            break;
                        }
                    }

                    if (!canPass)
                    {
                        PlayerCondition playerCondition = other.GetComponent<PlayerCondition>();
                        if (playerCondition != null)
                        {
                            playerCondition.TakeDamage(damage);
                            OnObstacleHit();
                        }
                        else
                        {
                            Debug.LogError("PlayerCondition을 찾을 수 없습니다");
                        }
                    }
                    else
                    {
                        OnObstaclePass();
                    }
                }
                else
                {
                    Debug.Log("플레이어가 장애물과 다른 레인에 있습니다.");
                }
            }
        }
    }

    protected virtual void OnObstacleHit()
    {
        // 충돌 효과 (VFX, SFX 등)
        Debug.Log($"플레이어가 장애물에 충돌했습니다");
    }

    protected virtual void OnObstaclePass()
    {
        // 통과 시 효과. 아마 아무것도 안 할 듯, 디버그 로그만 체크
        Debug.Log($"플레이어가 장애물을 통과했습니다");
    }
}