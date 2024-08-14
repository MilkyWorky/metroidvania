using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D bulletRB;
    public GameObject impactEffect;
    public Vector2 moveDirection;
    public int damageAmount;

    void Update()
    {
        bulletRB.velocity = moveDirection * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if (collision.CompareTag("Boss"))
        {
            BossHealthController.Instance.TakeDamage(damageAmount);
        }

        if(impactEffect != null)
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_BULLET_IMPACT);
            }

            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }
}
