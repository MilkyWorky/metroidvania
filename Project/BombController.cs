using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float timeToExplode = 0.5f;
    public GameObject explosionEffect;
    public float blastRange;
    public LayerMask destructibleLayer;

    public int damageAmount = 1;
    public LayerMask enemyLayer;
    public LayerMask bossLayer;

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if(timeToExplode <= 0)
        {
            if(explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }

            Collider2D[] objestsToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, destructibleLayer);

            if(objestsToDamage.Length > 0)
            {
                foreach (Collider2D col in objestsToDamage)
                {
                    Destroy(col.gameObject);
                }
            }

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, enemyLayer);

            if(enemiesToDamage.Length > 0)
            {
                foreach (Collider2D enemy in enemiesToDamage)
                {
                    EnemyHealthController enemyHealth = enemy.GetComponent<EnemyHealthController>();
                    if(enemyHealth != null)
                    {
                        enemyHealth.DamageEnemy(damageAmount);
                    }
                }
            }

            Collider2D[] bossToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, bossLayer);

            if (bossToDamage.Length > 0)
            {
                foreach (Collider2D boss in bossToDamage)
                {
                    if(boss != null)
                    {
                        BossHealthController.Instance.TakeDamage(damageAmount);
                    }
                }
            }


            Destroy(this.gameObject);
        }
    }
}
