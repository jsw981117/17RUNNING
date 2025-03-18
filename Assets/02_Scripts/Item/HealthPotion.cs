using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    
    public override void Use() // Summary * Potion 상속받고 Condition에서 구현한 Heal 호출
    {

        PlayerManager.Instance.PlayerCondition.Heal(); // Player에서 호출 

        Debug.Log("체력 포션 획득!");
    }

}
