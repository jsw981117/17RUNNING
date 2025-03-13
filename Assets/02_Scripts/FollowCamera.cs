using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;  // �÷��̾� ĳ����
    public Vector3 offset = new Vector3(0, 5, -8);  // ī�޶� ��ġ ������
    public float smoothSpeed = 5f;  // �ε巯�� �̵� �ӵ�

    void LateUpdate()
    {
        
        Vector3 targetPosition = player.position + offset;

        // �ε巴�� ���󰡱�
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); // ���� 

        // ĳ���� ���� ���󰡱�
        transform.LookAt(player.position + Vector3.forward * 5f);
    }
}
