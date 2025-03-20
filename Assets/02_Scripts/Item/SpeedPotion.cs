using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedPotion : Potion 
{
   
    public override void Use() // Summary * Speed Potion 호출
    {

        PlayerManager.Instance.PlayerCondition.SpeedUp();



    }


}