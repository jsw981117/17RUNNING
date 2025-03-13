using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Py : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 5f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {
           

            potion.Use(gameObject);
            Destroy(other.gameObject);
        }
    }
}
