using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public override void Use(GameObject potion)
    {
        
        // 체력 비례 스피드? => Player에서 처리

       /* PlayerManager.Instance.Player.life++;
        if (PlayerManager.Instance.Player.life > 3)
        {
            PlayerManager.Instance.Player.life = 3;
        }
       */
       
        Debug.Log("체력 포션 획득!");
    }

}
