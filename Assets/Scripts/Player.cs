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
    [SerializeField] Checkpoint firstCheckPoint;

    [SerializeField] AudioClip scaleUpSound;
    [SerializeField] AudioClip scaleDownSound;
    
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

    bool cantGrowToLarge = false;
    bool cantGrowToMedium = false;

    bool isFirstPowerUp = true;
    bool isFirstCheckPoint = true;

    bool isStopped = false;

    private void Start()
    {
        wallsLayer = LayerMask.GetMask(LayerMask.LayerToName(7));
        currentCheckPoint = firstCheckPoint;
    }

    void Update()
    {
        if (!isStopped)
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
    }

    public void StopPlayer()
    {
        isStopped = true;
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
                    AudioSource.PlayClipAtPoint(scaleUpSound, transform.position, 1.5f);
                }
                else if (currentSize == PlayerSize.medium)
                {
                    currentSize = PlayerSize.large;
                    currentPowerUp = PowerUpType.none;
                    AudioSource.PlayClipAtPoint(scaleUpSound, transform.position, 1.5f);
                }

                
            }
        }
        else if(currentPowerUp == PowerUpType.scaleDown)
        {
            if(currentSize == PlayerSize.medium)
            {
                currentSize = PlayerSize.small;
                currentPowerUp = PowerUpType.none;
                AudioSource.PlayClipAtPoint(scaleDownSound, transform.position);
            }
            else if(currentSize == PlayerSize.large)
            {
                currentSize = PlayerSize.medium;
                currentPowerUp = PowerUpType.none;
                AudioSource.PlayClipAtPoint(scaleDownSound, transform.position);
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
        
        if(currentSize == PlayerSize.medium && cantGrowToLarge)
        {
            uIHandler.DisplayMessage("Not Enough Space");
            return false;
        }

        if(currentSize == PlayerSize.small && cantGrowToMedium)
        {
            uIHandler.DisplayMessage("Not Enough Space");
            return false;
        }

        return true;
    }

    public void SetCantGrowLarge(bool cantGrowToLarge)
    {
        this.cantGrowToLarge = cantGrowToLarge;
    }

    public void SetCantGrowMedium(bool cantGrowToMedium)
    {
        this.cantGrowToMedium = cantGrowToMedium;
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
        if (isFirstPowerUp)
        {
            uIHandler.DisplayMessage("Press Space to change size");
            isFirstPowerUp = false;
        }

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
        if(checkpoint.CheckPointNumber > currentCheckPoint.CheckPointNumber )
        {
            if (isFirstCheckPoint)
            {
                uIHandler.DisplayMessage("Checkpoint\nHold R to reload to last checkpoint");
                isFirstCheckPoint = false;
            }
            else
            {
                uIHandler.DisplayMessage("Checkpoint");
            }
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
