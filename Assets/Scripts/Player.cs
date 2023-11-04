using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    
    float currentSpeed = 5f;
    Vector2 currentDirection = Vector2.zero;

    void Update()
    {
        MovePlayer();
        SetPlayerFacing();
    }

    private void MovePlayer()
    {
        rb.velocity = currentDirection * currentSpeed;
    }

    private void SetPlayerFacing()
    {
        if(transform.localScale.x < 0 && rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        else if(transform.localScale.x > 0 && rb.velocity.x < 0) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }


    public void SetDirection(Vector2 direction)
    {
        currentDirection = direction;
    }
}
