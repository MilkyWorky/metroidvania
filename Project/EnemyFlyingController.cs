using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    public float rangeToStartChase;
    private bool isChasing;

    public float moveSpeed;
    public float turnSpeed;

    private Transform playerTF;
    public Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerTF = PlayerHealthController.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasing)
        {
            if(Vector2.Distance(transform.position, playerTF.position) < rangeToStartChase)
            {
                isChasing = true;
                enemyAnim.SetBool("IsChasing", isChasing);
            }
        }
        else
        {
            if (playerTF.gameObject.activeSelf)
            {
                Vector3 direction = transform.position - playerTF.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, playerTF.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
