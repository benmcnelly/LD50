using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufo_logic : MonoBehaviour
{

    public Vector3 from = new Vector3(0f, 0f, 135f);
    public Vector3 to   = new Vector3(0f, 0f, 225f);
    public float speed = 1.0f; 
        

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed * 2.0f, 1.0f);
        transform.eulerAngles = Vector3.Lerp (from, to, t);        
    }
}
