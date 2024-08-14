using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerTransform;
    public BoxCollider2D boundsBox;
    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    void Update()
    {
        if(playerTransform != null)
        {
            this.transform.position = new Vector3(
                Mathf.Clamp(playerTransform.transform.position.x, boundsBox.bounds.min.x + halfWidth, boundsBox.bounds.max.x - halfWidth),
                Mathf.Clamp(playerTransform.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y - halfHeight),
                this.transform.position.z);
        }
        else
        {
            playerTransform = FindObjectOfType<PlayerController>();
        }
    }
}
