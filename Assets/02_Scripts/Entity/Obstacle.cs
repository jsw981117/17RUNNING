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

    // 내부적으로 사용할 레인 배열
    private int[] _lanes = new int[0];

    // 플레이어 레이어 번호
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private int damage = 1;

    [Header("Collider Settings")]
    [SerializeField] private float laneDistance = 1.0f;

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
    }

    private void OnValidate()
    {
        // 에디터에서 값 변경 시 레인 배열 업데이트
        UpdateLanesFromBooleans();

        // 에디터에서 값 변경 시 콜라이더 업데이트
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        if (boxCollider != null)
        {
            UpdateColliderShape();
        }
    }

    /// <summary>
    /// Boolean 값에서 레인 배열 업데이트
    /// </summary>
    private void UpdateLanesFromBooleans()
    {
        List<int> lanesList = new List<int>();

        if (useLeftLane) lanesList.Add(1);
        if (useCenterLane) lanesList.Add(0);
        if (useRightLane) lanesList.Add(-1);

        // 레인이 하나도 선택되지 않은 경우, 기본으로 중앙 레인 사용
        if (lanesList.Count == 0)
        {
            lanesList.Add(0);
            useCenterLane = true;
        }

        _lanes = lanesList.ToArray();
    }

    /// <summary>
    /// 레인 값에 따라 콜라이더 크기와 위치 업데이트
    /// </summary>
    private void UpdateColliderShape()
    {
        if (_lanes == null || _lanes.Length == 0)
        {
            // 기본값: 중앙 레인
            _lanes = new int[] { 0 };
            useCenterLane = true;
        }

        int minLane = _lanes.Min();
        int maxLane = _lanes.Max();

        int laneCount = maxLane - minLane + 1;

        // 콜라이더 중심점 계산 (최소 레인과 최대 레인의 중간 지점)
        float centerX = (minLane + maxLane) * laneDistance / 2f;

        // 콜라이더 X축 크기 계산 (커버하는 레인 수에 비례)
        float sizeX = laneCount * laneDistance;

        // 연속되지 않은 레인이 있는 경우 (예: -1과 1만 있고 0은 없는 경우)
        // 모든 레인을 포함하는 크기로 설정
        if (laneCount > _lanes.Length)
        {
            // 모든 레인을 커버
            sizeX = (maxLane - minLane + 1) * laneDistance;
        }

        // 콜라이더 크기 및 중심 설정
        boxCollider.size = new Vector3(sizeX, colliderHeight, colliderDepth);
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