using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedPotion : Potion
{
    private static readonly WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(3f); // ✅ 재사용
    float accel = 5f; // 추가 속도 
    float originSpeed;
    float timer = 0f;

    bool isSpeeding=false;
    public override void Use(GameObject game)
    {
        originSpeed = PyManager.Instance.Ppy.speed;
        Debug.Log(originSpeed);
        PyManager.Instance.Ppy.SpeedUp();
     
    
    }


}