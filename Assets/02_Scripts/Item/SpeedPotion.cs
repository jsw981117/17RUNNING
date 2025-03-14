using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedPotion : Potion
{

    public override void Use(GameObject game)
    {

        PlayerManager.Instance.PlayerCondition.SpeedUp();
    
    }


}