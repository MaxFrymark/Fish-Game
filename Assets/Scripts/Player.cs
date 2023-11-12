using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] UIHandler uIHandler;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PowerUpManager powerUpManager;
    
    enum PlayerSize { small, medium, large }
    PlayerSize currentSize = PlayerSize.medium;
    PlayerSize savedSize = PlayerSize.medium;

    public enum PowerUpType { none, scaleUp, scaleDown }
    PowerUpType currentPowerUp = PowerUpType.none;
    PowerUpType savedPowerup = PowerUpType.none;

    float smallScale = 0.5f;
    float mediumScale = 1f;
    float largeScale = 2f;
    float scaleSpeed = 0.01f;
    
    float currentSpeed = 5f;
    Vector2 currentDirection = Vector2.zero;
    float returnToCheckPointSpeed = 10f;

    Checkpoint currentCheckPoint;
    bool returningToCheckPoint;

    LayerMask wallsLayer;

    private void Start()
    {
        wallsLayer = LayerMask.GetMask(LayerMask.LayerToName(7));
    }

    void Update()
    {
        if (returningToCheckPoint)
        {
            ReturnToCheckPoint();
            return;
        }
        
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
            if (CheckIfEnoughSpaceToGrow())
            {
                if (currentSize == PlayerSize.small)
                {
                    currentSize = PlayerSize.medium;
                    currentPowerUp = PowerUpType.none;
                }
                else if (currentSize == PlayerSize.medium)
                {
                    currentSize = PlayerSize.large;
                    currentPowerUp = PowerUpType.none;
                }
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

        UpdateUIPowerUpDisplay();
    }

    private bool CheckIfEnoughSpaceToGrow()
    {
        if(currentSize == PlayerSize.large)
        {
            return false;
        }

        float spaceNeeded = 0f;
        if(currentSize == PlayerSize.small)
        {
            spaceNeeded = 0.4f;
        }
        else if(currentSize == PlayerSize.medium)
        {
            spaceNeeded = 0.7f;
        }

        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, spaceNeeded, wallsLayer);
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, spaceNeeded, wallsLayer);
        if(upHit && downHit)
        {
            return false;
        }

        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, spaceNeeded, wallsLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, spaceNeeded, wallsLayer);
        if (rightHit && leftHit)
        {
            return false;
        }


        return true;
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
        UpdateUIPowerUpDisplay();
    }

    private void LoadPowerUpFromSave()
    {
        currentPowerUp = savedPowerup;
        UpdateUIPowerUpDisplay();
    }

    private void UpdateUIPowerUpDisplay()
    {
        uIHandler.UpdatePowerUpDisplay((int)currentPowerUp);
    }

    public int GetPlayerSize()
    {
        return (int)currentSize;
    }

    public void UpdateCheckPoint(Checkpoint checkpoint)
    {
        if(currentCheckPoint == null || checkpoint.CheckPointNumber > currentCheckPoint.CheckPointNumber )
        {
            currentCheckPoint = checkpoint;
            powerUpManager.SaveData();
            savedPowerup = currentPowerUp;
            savedSize = currentSize;
        }
    }

    public void Die()
    {
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        returningToCheckPoint = true;
    }

    public void Restore()
    {
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        returningToCheckPoint = false;
        powerUpManager.LoadData();
        LoadPowerUpFromSave();
        currentSize = savedSize;
    }

    public void ReturnToCheckPoint()
    {
        Vector2 directionToCheckpoint = currentCheckPoint.transform.position - transform.position;
        if(directionToCheckpoint.magnitude < 0.1f)
        {
            Restore();
            return;
        }
        directionToCheckpoint.Normalize();
        rb.velocity = directionToCheckpoint * returnToCheckPointSpeed;

    }
}
