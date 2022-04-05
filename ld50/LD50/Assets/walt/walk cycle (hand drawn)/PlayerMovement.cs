using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private Vector2 _moveInput;
    private WaltActions _playerActions;

    // private Animator animation;
    

    // options
    public float run_speed = 400f;
    float horizontal_move = 0f;
    bool jump = false;

    private void OnEnable()
    {
        _playerActions.Player.Enable();
    }
    

    void Awake()
    {
        _playerActions = new WaltActions();
        // animation = GetComponent<Animator>();
    }


    void Update()
    {
        if (_playerActions.Player.Jump.triggered) {
            jump = true;
        }     

        if (_playerActions.Player.Fire.triggered) {
            CharacterController2D._controller.LaunchPlayer();
        }             
      
    }

    void FixedUpdate () 
    {
        _moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
        var horz_move = _moveInput.x;
        CharacterController2D._controller.Move(horz_move, false, jump);
        jump = false;
    }

}
