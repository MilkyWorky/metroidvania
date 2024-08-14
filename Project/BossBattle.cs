using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    public int threshold1;
    public int threshold2;
    public float activeTime;
    public float fadeOutTime;
    public float inActiveTime;
    public float moveSpeed;

    private float activeCounter;
    private float fadeCounter;
    private float inActiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public Animator bossAnim;
    public Transform theBoss;
    public float timeBetweenShotS1;
    public float timeBetweenShotS2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;
    public GameObject winObject;
    private bool battleEnded;

    private void Start()
    {
        activeCounter = activeTime;
        shotCounter = timeBetweenShotS1;
    }

    private void Update()
    {
        if (!battleEnded)
        {
            if(BossHealthController.Instance.currentHealth > threshold1)
            {
                if(activeCounter > 0)
                {
                    activeCounter -= Time.deltaTime;
                    if(activeCounter <= 0)
                    {
                        fadeCounter = fadeOutTime;
                        bossAnim.SetTrigger("Vanish");
                    }
                    shotCounter -= Time.deltaTime;
                    if(shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShotS1;
                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }
                }
                else if(fadeCounter > 0)
                {
                    fadeCounter -= Time.deltaTime;
                    if(fadeCounter <= 0)
                    {
                        theBoss.gameObject.SetActive(false);
                        inActiveCounter = inActiveTime;
                    }
                }
                else if(inActiveCounter > 0)
                {
                    inActiveCounter -= Time.deltaTime;
                    if(inActiveCounter <= 0)
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                        theBoss.gameObject.SetActive(true);
                        activeCounter = activeTime;
                        shotCounter = timeBetweenShotS1;
                    }
                }
            }
            else
            {
                if(targetPoint == null)
                {
                    targetPoint = theBoss;
                    fadeCounter = fadeOutTime;
                    bossAnim.SetTrigger("Vanish");
                }
                else
                {
                    if (Vector3.Distance(theBoss.position, targetPoint.position) > 0.02f)
                    {
                        theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                        if (Vector3.Distance(theBoss.position, targetPoint.position) <= 0.02f)
                        {
                            fadeCounter = fadeOutTime;
                            bossAnim.SetTrigger("Vanish");
                        }

                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if(PlayerHealthController.Instance.currentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShotS1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShotS2;
                            }
                        
                            Instantiate(bullet, shotPoint.position, Quaternion.identity);
                        }
                    }
                    else if (fadeCounter > 0)
                    {
                        fadeCounter -= Time.deltaTime;
                        if (fadeCounter <= 0)
                        {
                            theBoss.gameObject.SetActive(false);
                            inActiveCounter = inActiveTime;
                        }
                    }
                    else if (inActiveCounter > 0)
                    {
                        inActiveCounter -= Time.deltaTime;
                        if (inActiveCounter <= 0)
                        {
                            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            int whileBreaker = 0;
                            while (targetPoint.position.Equals(theBoss.position) && whileBreaker < 100)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                                whileBreaker++;
                            }

                            theBoss.gameObject.SetActive(true);

                            if (PlayerHealthController.Instance.currentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShotS1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShotS2;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            fadeCounter -= Time.deltaTime;
            if(fadeCounter < 0)
            {
                if(winObject != null)
                {
                    winObject.SetActive(true);
                }

                gameObject.SetActive(false);
            }
        }
    }

    public void EndBattle()
    {
        battleEnded = true;
        fadeCounter = fadeOutTime;
        bossAnim.SetTrigger("Vanish");
        theBoss.GetComponent<CircleCollider2D>().enabled = false;

        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        if(bullets.Length > 0)
        {
            foreach (BossBullet bossBullet in bullets)
            {
                Destroy(bossBullet.gameObject);
            }
        }
    }
}
