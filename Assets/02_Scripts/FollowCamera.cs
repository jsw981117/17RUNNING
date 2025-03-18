using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour // Summary * 보간을 통해 카메라 부드럽게 따라가기 
{
    public Transform player;  // 플레이어 캐릭터
    public Vector3 offset = new Vector3(0, 5, -8);  // 카메라 위치 오프셋
    public float smoothSpeed = 5f;  // 부드러운 이동 속도

    void LateUpdate()
    {
        
        Vector3 targetPosition = player.position + offset;

        // 부드럽게 따라가기
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); // 보간 

        // 캐릭터 방향 따라가기
        transform.LookAt(player.position + Vector3.forward * 5f);
    }
}
