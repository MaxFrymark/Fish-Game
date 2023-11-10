using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    PowerUp[] powerUps;
    PowerUpData[] storedPowerUpData;

    private void Start()
    {
        powerUps = FindObjectsOfType<PowerUp>();
        storedPowerUpData = new PowerUpData[powerUps.Length];
    }

    public void SaveData()
    {
        for(int i = 0; i < powerUps.Length; i++)
        {
            storedPowerUpData[i] = new PowerUpData(powerUps[i].gameObject.activeInHierarchy, powerUps[i].PowerUpType);
        }
    }

    public void LoadData()
    {
        for(int i = 0; i < storedPowerUpData.Length; i++)
        {
            storedPowerUpData[i].CompareToPowerUp(powerUps[i]);
        }
    }
}

public struct PowerUpData
{
    bool active;
    Player.PowerUpType powerUpType;

    public PowerUpData(bool active, Player.PowerUpType powerUpType)
    {
        this.active = active;
        this.powerUpType = powerUpType;
    }

    public void CompareToPowerUp(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(active);
        if(powerUpType != powerUp.PowerUpType)
        {
            powerUp.SwitchPowerUpType();
        }
    }
}
