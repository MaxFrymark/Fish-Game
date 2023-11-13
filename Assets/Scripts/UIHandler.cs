using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Image powerUpDisplay;

    [SerializeField] Sprite scaleUpSprite;
    [SerializeField] Sprite scaleDownSprite;

    [SerializeField] TextMeshProUGUI messages;

    private void Start()
    {
        DisplayMessage("WASD to Move");
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
}
