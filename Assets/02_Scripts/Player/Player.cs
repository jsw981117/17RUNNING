using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMotionState motionState;
    public int lane;

    public int life;

    [HideInInspector] public float moveSpeed;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float speedBoostMultiplier;

    private Rigidbody _rigidbody;

    private void FixedUpdate()
    {
        
    }
}
 