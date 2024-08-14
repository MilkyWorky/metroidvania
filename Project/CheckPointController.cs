using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    private bool isCollide = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollide)
        {
            RespawnController.Instance.SetSpawnPoint(this.transform);
            isCollide = true;
        }
    }
}
