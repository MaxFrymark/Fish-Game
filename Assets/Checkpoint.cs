using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    private float checkPointNumber;
    public float CheckPointNumber {  get { return checkPointNumber; } }

    private void Start()
    {
        checkPointNumber = transform.position.x;
        checkpointManager = GetComponentInParent<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().UpdateCheckPoint(this);
    }
}
