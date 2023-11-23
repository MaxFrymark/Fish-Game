using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] UIHandler uiHandler;

    private enum ControlMode { OpeningScreen, Gameplay, ClosingSequence, ClosingScreen }
    private ControlMode currentControlMode = ControlMode.OpeningScreen;
    
    PlayerControls playerControls;

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Move.performed += MovePlayer;
        playerControls.Player.Move.canceled += MovePlayer;
        playerControls.Player.Action.performed += PlayerAction;
        playerControls.Player.Reload.performed += StartReloading;
        playerControls.Player.Reload.canceled += CancelReloading;
        playerControls.Enable();
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        if (currentControlMode == ControlMode.Gameplay)
        {
            Vector2 playerDirection = context.ReadValue<Vector2>();
            player.SetDirection(playerDirection);
        }
    }

    private void PlayerAction(InputAction.CallbackContext context)
    {
        if (currentControlMode == ControlMode.Gameplay)
        {
            player.ChangeScale();
        }
        else if(currentControlMode == ControlMode.OpeningScreen)
        {
            uiHandler.StartGame();
            currentControlMode = ControlMode.Gameplay;
        }
        else if(currentControlMode == ControlMode.ClosingScreen)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void StartReloading(InputAction.CallbackContext context)
    {
        if (currentControlMode == ControlMode.Gameplay)
        {
            uiHandler.StartReloading();
        }
    }

    private void CancelReloading(InputAction.CallbackContext context)
    {
        if(currentControlMode == ControlMode.Gameplay)
        {
            uiHandler.EndReloading();
        }
    }

    public void SetClosingSequence()
    {
        player.StopPlayer();
        currentControlMode = ControlMode.ClosingSequence;
    }

    public void SetClosingMenu()
    {
        currentControlMode = ControlMode.ClosingScreen;
    }
}
