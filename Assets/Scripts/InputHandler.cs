using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] Player player;
    
    PlayerControls playerControls;

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Move.performed += MovePlayer;
        playerControls.Player.Move.canceled += MovePlayer;
        playerControls.Player.Action.performed += PlayerAction;
        playerControls.Enable();
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        Vector2 playerDirection = context.ReadValue<Vector2>();
        player.SetDirection(playerDirection);
    }

    private void PlayerAction(InputAction.CallbackContext context)
    {

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
