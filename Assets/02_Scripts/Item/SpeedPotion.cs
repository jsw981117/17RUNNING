using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Potion
{

    float sp = 3f; // 추가 속도 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(GameObject game)
    {
        Debug.Log("스피드 포션입니다.");
    }
}
