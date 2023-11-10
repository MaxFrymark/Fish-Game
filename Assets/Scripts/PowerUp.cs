using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    
    [SerializeField] Player.PowerUpType powerUpType;
    public Player.PowerUpType PowerUpType { get { return powerUpType; } }

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite scaleUpSprite;
    [SerializeField] Sprite scaleDownSprite;
    
    void Start()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        switch (powerUpType)
        {
            case Player.PowerUpType.none:
                Debug.Log("No Power Up Type Set");
                DisablePowerUp();
                break;
            case Player.PowerUpType.scaleUp:
                spriteRenderer.sprite = scaleUpSprite;
                break;
            case Player.PowerUpType.scaleDown:
                spriteRenderer.sprite = scaleDownSprite;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Player>().GetPowerUp(this);
    }

    public Player.PowerUpType GetPowerUpType()
    {
        return powerUpType;
    }

    public void SwitchPowerUpType()
    {
        switch (powerUpType)
        {
            case Player.PowerUpType.scaleUp:
                powerUpType = Player.PowerUpType.scaleDown;
                break;
            case Player.PowerUpType.scaleDown:
                powerUpType = Player.PowerUpType.scaleUp;
                break;
        }
        SetSprite();
    }

    public void DisablePowerUp()
    {
        gameObject.SetActive(false);
    }
}
