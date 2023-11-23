using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    [SerializeField] EndGameManager endGameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Tada");
        endGameManager.PlayerAtTarget();
    }
}
