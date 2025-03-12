using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEffect
{
    public void  Use();
}
public class Potion : MonoBehaviour,IEffect
{
  
    public virtual void Use()
    {
      
    }

    // Start is called before the first frame update
  
}
