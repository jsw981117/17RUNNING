using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Py : MonoBehaviour
{
    // Start is called before the first frame update
    float originSpeed;
    public float speed = 5f;
    bool isSpeeding = false;
    private void Awake()
    {
        PyManager.Instance.Ppy = this;   
    }
  
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

    public void SpeedUp()
    {
        isSpeeding = true;
        // 가속도 누적방지
        if (isSpeeding)  // ex) 연속으로 배치된 포션을 먹고 기본속도가 늘어나는 걸 방지
        {
            originSpeed = speed; // 원
            speed += 5f;
        }
        
        
        // 무적함수 호출
        Invoke("AfterSpeedUp", 3f);
        isSpeeding=false;
    }

    public void AfterSpeedUp()
    {
        speed = originSpeed;
        Debug.Log("스피드업 종료");
        
    }
    public void OnTriggerEnter(Collider other)
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {
            Destroy(other.gameObject);
            potion.Use(gameObject);
           
        }
    }
}
