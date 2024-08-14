using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController Instance;

    public Slider bossHealthSilder;
    public int currentHealth = 30;
    public BossBattle theBoss;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bossHealthSilder.maxValue = currentHealth;
        bossHealthSilder.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BOSS_IMPACT);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_BOSS_DEATH);
            }

            theBoss.EndBattle();
        }

        bossHealthSilder.value = currentHealth;
    }
}
