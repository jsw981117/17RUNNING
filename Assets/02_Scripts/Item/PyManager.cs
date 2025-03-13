using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditorInternal;
using UnityEngine;

public class PyManager : MonoBehaviour
{
    private static PyManager instance;
    public static PyManager Instance
    {

        get { if (instance == null) instance = new GameObject("PyManager").AddComponent<PyManager>();
                return instance; }
    }

    public Py py;
    public Py Ppy
    {
        get { return py; }
        set { py = value;}
       
    }


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(instance== this)
            {
                DontDestroyOnLoad (gameObject);
            }
        }
    }
}
