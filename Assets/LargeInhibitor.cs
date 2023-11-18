using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeInhibitor : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.SetCantGrowLarge(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.SetCantGrowLarge(false);
    }
}
