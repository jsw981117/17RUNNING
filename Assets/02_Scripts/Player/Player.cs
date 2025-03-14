using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int life;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
    }

    public void Heal() // 생명력 회복 
    {
        life++;
        if (life > 3) // 생명력 보정 3개이상이면 3으로 유지
        {
            life = 3;
        }
    }

    public void SpeedUp()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
       Potion potion = other.GetComponent<Potion>();
        if (potion != null) { 
        
            Destroy(other.gameObject);
            potion.Use(gameObject);
        }
    }
    private void FixedUpdate()
    {
        
    }
}
 