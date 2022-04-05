using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poof : MonoBehaviour
{
    
    void OnCollisionEnter2D(Collision2D coll){     
        GameCoreLogic._game_logic.PlayAudioClanking();
        StartCoroutine(DestroyCoroutine());  
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(transform.gameObject);  
    }    

}
