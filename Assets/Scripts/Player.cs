using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    enum PlayerSize { small, medium, large }
    PlayerSize currentSize = PlayerSize.medium;

    public enum PowerUpType { none, scaleUp, scaleDown }
    PowerUpType currentPowerUp = PowerUpType.none;

    float smallScale = 0.5f;
    float mediumScale = 1f;
    float largeScale = 2f;
    float scaleSpeed = 0.01f;
    
    float currentSpeed = 5f;
    Vector2 currentDirection = Vector2.zero;

    void Update()
    {
        MovePlayer();
        SetPlayerFacing();
        CheckScale();
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

    private void CheckScale()
    {
        switch(currentSize)
        {
            case PlayerSize.small:
                if(transform.localScale.y != smallScale)
                {
                    SetScale(smallScale);
                }
                break;
            case PlayerSize.medium:
                if(transform.localScale.y != mediumScale)
                {
                    SetScale(mediumScale);
                }
                break;
            case PlayerSize.large:
                if(transform.localScale.y != largeScale)
                {
                    SetScale(largeScale);
                }
                break;
        }
    }

    private void SetScale(float target)
    {
        if(Mathf.Abs(transform.localScale.y - target) < scaleSpeed)
        {
            transform.localScale = new Vector3(target * Mathf.Sign(transform.localScale.x), target, target);
        }
        
        else if(transform.localScale.y > target)
        {
            Shrink();
        }
        else if(transform.localScale.y < target)
        {
            Grow();
        }
    }

    private void Grow()
    {
        transform.localScale += new Vector3(scaleSpeed * Mathf.Sign(transform.localScale.x), scaleSpeed, scaleSpeed);
    }

    private void Shrink()
    {
        transform.localScale -= new Vector3(scaleSpeed * Mathf.Sign(transform.localScale.x), scaleSpeed, scaleSpeed / 2);
    }

    public void SetDirection(Vector2 direction)
    {
        currentDirection = direction;
    }

    public void ChangeScale()
    {
        if(currentPowerUp == PowerUpType.scaleUp)
        {
            if(currentSize == PlayerSize.small)
            {
                currentSize = PlayerSize.medium;
                currentPowerUp = PowerUpType.none;
            }
            else if(currentSize == PlayerSize.medium)
            {
                currentSize = PlayerSize.large; 
                currentPowerUp = PowerUpType.none;
            }
        }
        else if(currentPowerUp == PowerUpType.scaleDown)
        {
            if(currentSize == PlayerSize.medium)
            {
                currentSize = PlayerSize.small;
                currentPowerUp = PowerUpType.none;
            }
            else if(currentSize == PlayerSize.large)
            {
                currentSize = PlayerSize.medium;
                currentPowerUp = PowerUpType.none;
            }
        }
    }

    public void GetPowerUp(PowerUp powerUp)
    {
        PowerUpType incomingPowerUp = powerUp.GetPowerUpType();
        if(incomingPowerUp == currentPowerUp)
        {
            return;
        }
        if(currentPowerUp == PowerUpType.none)
        {
            powerUp.DisablePowerUp();
        }
        else
        {
            powerUp.SwitchPowerUpType();
        }
        currentPowerUp = incomingPowerUp;
    }
}
