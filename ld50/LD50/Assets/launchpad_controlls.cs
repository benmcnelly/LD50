using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launchpad_controlls : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("GameObject2 collided with player");
            CharacterController2D._controller.on_launchpad = true;
            CharacterController2D._controller.tracking_airtime = false;
        }
    }


}
