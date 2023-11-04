using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Image powerUpDisplay;

    [SerializeField] Sprite scaleUpSprite;
    [SerializeField] Sprite scaleDownSprite;

    public void UpdatePowerUpDisplay(int powerup)
    {
        switch (powerup) 
        { 
            case 0:
                powerUpDisplay.gameObject.SetActive(false);
                break;
            case 1:
                powerUpDisplay.gameObject.SetActive(true);
                powerUpDisplay.sprite = scaleUpSprite;
                break;
            case 2:
                powerUpDisplay.gameObject.SetActive(true);
                powerUpDisplay.sprite = scaleDownSprite;
                break;
        }
    }
}
