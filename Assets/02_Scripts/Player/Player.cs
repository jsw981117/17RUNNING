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

 
    private void FixedUpdate()
    {
        
    }
}
 