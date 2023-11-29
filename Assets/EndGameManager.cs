using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] InputHandler inputHandler;
    [SerializeField] Rigidbody2D player;
    [SerializeField] Rigidbody2D girlFish;
    [SerializeField] UIHandler uIHandler;
    [SerializeField] Transform playerTarget;
    [SerializeField] CameraMover cameraMover;


    public void StartEndGame()
    {
        inputHandler.SetClosingSequence();
        MovePlayerTowardTarget();
    }

    private void MovePlayerTowardTarget()
    {
        Vector2 playerDirection = playerTarget.position - player.transform.position;
        playerDirection.Normalize();
        player.velocity = playerDirection * 4f;
        
    }

    private void MoveGirlFishToPlayer()
    {
        cameraMover.StopFollowing();
        player.velocity = Vector2.zero;
        girlFish.velocity = Vector2.left * 4f;
    }

    public void BothSwimAway()
    {
        player.velocity = Vector2.right * 5.5f;
        girlFish.velocity = Vector2.right * 5.5f;
        girlFish.transform.localScale = new Vector3 (1, 1, 1);
    }

    public void EndGame()
    {
        inputHandler.SetClosingMenu();
        uIHandler.EndGame();
    }

    public void PlayerAtTarget()
    {
        MoveGirlFishToPlayer();
    }
}
