using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEffect  
{
    public void  Use(); // 포션 use
}
public class Potion : MonoBehaviour, IEffect
{
  
    public virtual void Use() // 상속받은 각 포션들이 재정의 가능하도록 구현 
    {
      
    }

  
}
