using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject openingScreen;
    [SerializeField] GameObject closingScreen;
    
    [SerializeField] Player player;
    [SerializeField] Image powerUpDisplay;
    [SerializeField] GameObject powerUpParent;

    [SerializeField] Sprite scaleUpSprite;
    [SerializeField] Sprite scaleDownSprite;

    [SerializeField] TextMeshProUGUI messages;

    [SerializeField] GameObject reloadDisplay;
    [SerializeField] Slider reloadSlider;
    bool reloading = false;


    private void Update()
    {
        if (reloading)
        {
            HandleReloading();
        }
    }

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

    public void DisplayMessage(string message)
    {
        messages.gameObject.SetActive (true);
        messages.text = message;
        StartCoroutine(DisableMessageDisplay());

    }

    private IEnumerator DisableMessageDisplay()
    {
        yield return new WaitForSeconds(3f);
        messages.gameObject.SetActive (false);
    }

    public void StartReloading()
    {
        reloading = true;
        reloadDisplay.SetActive (true);
    }

    private void HandleReloading()
    {
        reloadSlider.value += 1f * Time.deltaTime;
        if(reloadSlider.value == 1f)
        {
            player.Die();
            EndReloading();
        }
    }

    public void EndReloading()
    {
        if (reloading)
        {
            reloadSlider.value = 0f;
            reloadDisplay.SetActive(false);
            reloading = false;
        }
    }

    public void StartGame()
    {
        openingScreen.SetActive (false );
        player.gameObject.SetActive (true );
        powerUpParent.SetActive(true);
        DisplayMessage("WASD to Move");
    }

    public void EndGame()
    {
        closingScreen.SetActive(true);
    }
}
