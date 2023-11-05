using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] EnemyFish enemyFish;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyFish.PlayerEntersRange(collision.GetComponent<Player>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyFish.PlayerExitsRange();
    }
}
