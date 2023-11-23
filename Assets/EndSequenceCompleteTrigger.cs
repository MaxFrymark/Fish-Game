using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequenceCompleteTrigger : MonoBehaviour
{
    [SerializeField] EndGameManager endGameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        endGameManager.EndGame();
    }
}
