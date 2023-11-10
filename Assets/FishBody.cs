using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBody : MonoBehaviour
{
    [SerializeField] EnemyFish fish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fish.AttackPlayer();
        }
    }
}
