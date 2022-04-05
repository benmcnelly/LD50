using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poof_parent : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D coll){     
        PlayerController.control.PlayClanking();
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(transform.parent.gameObject);  
    }    

}
