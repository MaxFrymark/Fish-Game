using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBody : MonoBehaviour
{
    [SerializeField] EnemyFish fish;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fish.AttackPlayer();
        }
    }
}
