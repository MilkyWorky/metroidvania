using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patroilPoints;
    private int currentPoint;

    public float moveSpeed;
    public float waitAtPoints;
    private float waitCounter;

    public float jumpForce;
    public Rigidbody2D enemyRB;
    public Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;

        foreach (Transform pPoint in patroilPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.x - patroilPoints[currentPoint].position.x) > 0.2f)
        {
            if(transform.position.x < patroilPoints[currentPoint].position.x)
            {
                enemyRB.velocity = new Vector2(moveSpeed, enemyRB.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                enemyRB.velocity = new Vector2(-moveSpeed, enemyRB.velocity.y);
                transform.localScale = Vector3.one;
            }

            if(transform.position.y < patroilPoints[currentPoint].position.y && enemyRB.velocity.y < 0.1f)
            {
                enemyRB.velocity = new Vector2(enemyRB.velocity.x, jumpForce);
            }
        }
        else
        {
            enemyRB.velocity = new Vector2(0f, enemyRB.velocity.y);
            waitCounter -= Time.deltaTime;
            if(waitCounter <= 0)
            {
                waitCounter = waitAtPoints;
                currentPoint++;

                if(currentPoint >= patroilPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }

        enemyAnim.SetFloat("Speed", Mathf.Abs(enemyRB.velocity.x));
    }
}
