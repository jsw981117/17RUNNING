using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{

    float heatlh = 50; // 회복량


    public override void Use(GameObject potion)
    {
        // 무적 효과 까지 
        // Player 무적함수 호출
        // Player.hp += health;

        /*if(Player.hp> Player.MaxHp)
        {
            Playerhp = Player.MaxHp;
        }*/

        // 체력 비례 스피드?

        Debug.Log("체력 포션입니다.");
    }

}
