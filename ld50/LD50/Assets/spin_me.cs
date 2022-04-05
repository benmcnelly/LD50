using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin_me : MonoBehaviour
{

    public float degreesPerSec = 25f;


    // Update is called once per frame
    void Update()
    {
        float rotAmount = degreesPerSec * Time.deltaTime;
        float curRot = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0,0,curRot+rotAmount));
    }
}
