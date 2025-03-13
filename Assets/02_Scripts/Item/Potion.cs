using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEffect
{
    public void  Use(GameObject game);
}
public class Potion : MonoBehaviour,IEffect
{
  
    public virtual void Use(GameObject game)
    {
      
    }

  
}
