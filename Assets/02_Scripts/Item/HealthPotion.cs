using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public override void Use(GameObject potion)
    {

        PlayerManager.Instance.Player.Heal(); // Player에서 호출 

        Debug.Log("체력 포션 획득!");
    }

}
