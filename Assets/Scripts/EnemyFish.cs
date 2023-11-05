using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField] int size;

    [SerializeField] Transform[] waypoints;
    [SerializeField] Rigidbody2D fishRigidBody;

    int currentWaypoint = 0;
    Transform currentDestination;

    float currentSpeed;
    float patrolSpeed = 3f;
    float chaseSpeed = 6f;
    float retreatSpeed = 8f;

    LayerMask playerLayer;
    LayerMask wallsLayer;

    enum Behavior { patrol, chase, retreat }
    Behavior currentBehavior = Behavior.patrol;

    bool isPlayerInRange = false;
    Player player = null;

    private void Start()
    {
        currentDestination = waypoints[0];
        currentSpeed = patrolSpeed;
        playerLayer = LayerMask.GetMask(LayerMask.LayerToName(6));
        wallsLayer = LayerMask.GetMask(LayerMask.LayerToName(7));
        MoveTowards();
    }

    private void Update()
    {
        switch (currentBehavior)
        {
            case Behavior.patrol:
                Patrol();
                break;
            case Behavior.chase:
                Chase();
                break;
            case Behavior.retreat:
                Retreat();
                break;

        }

        SetFacing();
    }

    private void Patrol()
    {
        if (Vector2.Distance(fishRigidBody.transform.position, currentDestination.position) < 0.1f)
        {
            UpdateWaypoint();
        }
        if (isPlayerInRange)
        {
            CheckLineOfSightToPlayer();
        }
    }

    private void Chase()
    {
        MoveTowards();
        CheckLineOfSightToPlayer();
    }

    private void Retreat()
    {

    }

    private void UpdateWaypoint()
    {
        if (currentWaypoint >= waypoints.Length - 1)
        {
            currentWaypoint = 0;
        }

        else
        {
            currentWaypoint++;
        }

        currentDestination = waypoints[currentWaypoint];
        MoveTowards();
    }

    private void MoveTowards()
    {
        Vector2 direction = currentDestination.position - fishRigidBody.transform.position;
        direction.Normalize();
        fishRigidBody.velocity = direction * currentSpeed;
    }

    private void SetFacing()
    {
        if (fishRigidBody.transform.localScale.x < 0 && fishRigidBody.velocity.x > 0)
        {
            fishRigidBody.transform.localScale = new Vector3(Mathf.Abs(fishRigidBody.transform.localScale.x),
                fishRigidBody.transform.localScale.y, fishRigidBody.transform.localScale.z);
        }

        else if (fishRigidBody.transform.localScale.x > 0 && fishRigidBody.velocity.x < 0)
        {
            fishRigidBody.transform.localScale = new Vector3(-fishRigidBody.transform.localScale.x,
                fishRigidBody.transform.localScale.y, fishRigidBody.transform.localScale.z);
        }
    }

    public void PlayerEntersRange(Player player)
    {
        if(this.player == null)
        {
            this.player = player;
        }
        isPlayerInRange = true;
    }

    public void PlayerExitsRange()
    {
        isPlayerInRange = false;
    }

    private void CheckLineOfSightToPlayer()
    {

        Vector2 directionToPlayer = player.transform.position - fishRigidBody.transform.position;
        directionToPlayer.Normalize();
        RaycastHit2D raycastHit2D = Physics2D.Raycast(fishRigidBody.transform.position, directionToPlayer, 15f, playerLayer | wallsLayer);
        Debug.DrawLine(fishRigidBody.transform.position, raycastHit2D.transform.position, Color.green);
        if(raycastHit2D.transform.gameObject.layer == 6)
        {
            Debug.Log("meow");

            if (player.GetPlayerSize() > size)
            {
                if(currentBehavior != Behavior.retreat)
                {
                    SwitchToRetreat();
                }
            }

            if(player.GetPlayerSize() < size)
            {
                if(currentBehavior != Behavior.chase)
                {
                    SwitchToChase();
                }
            }
        }
        else
        {
            Debug.Log(raycastHit2D.transform.gameObject.layer);

            if (currentBehavior == Behavior.chase)
            {
                SwitchToPatrol();
            }
        }
    }

    private void SwitchToPatrol()
    {
        currentBehavior = Behavior.patrol;
        currentSpeed = patrolSpeed;
        currentDestination = waypoints[0];
    }

    private void SwitchToChase()
    {
        currentBehavior = Behavior.chase;
        currentSpeed = chaseSpeed;
        currentDestination = player.transform;
    }

    private void SwitchToRetreat()
    {
        currentBehavior = Behavior.retreat;
        currentSpeed = retreatSpeed;
    }
}