using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    public bool unlockDoubleJump;
    public bool unlockDash;
    public bool unlockBecomeball;
    public bool unlockDropBomb;
    public GameObject pickupEffect;
    public string unlockMessage;
    public TMP_Text unlockText;
    public GameObject platformObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_PICKUP_GEM);
            }

            if (platformObject != null)
            {
                platformObject.SetActive(true);
            }

            PlayerAbilityTracker playerAbilityTracker = collision.GetComponentInParent<PlayerAbilityTracker>();

            if (unlockDoubleJump)
            {
                playerAbilityTracker.canDounbleJump = true;
            }

            if (unlockDash)
            {
                playerAbilityTracker.canDash = true;
            }

            if (unlockBecomeball)
            {
                playerAbilityTracker.canBecomeBall = true;
            }

            if (unlockDropBomb)
            {
                playerAbilityTracker.canDropBomb = true;
            }

            Instantiate(pickupEffect, transform.position, transform.rotation);
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;
            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);
            Destroy(unlockText.transform.parent.gameObject, 3f);
            Destroy(gameObject);
        }
    }
}
