using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator doorAnim;
    public float distanceToOpen;
    private PlayerController player;
    private bool isPlayerExiting;
    public Transform exitPoint;
    public float moveSpeed;
    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.Instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < distanceToOpen)
        {
            doorAnim.SetBool("DoorOpen", true);
        }
        else
        {
            doorAnim.SetBool("DoorOpen", false);
        }

        if (isPlayerExiting)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isPlayerExiting)
            {
                player.canMove = false;
                StartCoroutine(UseDoorCoroutine());
            }
        }
    }

    private IEnumerator UseDoorCoroutine()
    {
        isPlayerExiting = true;
        player.playerStandingAnim.enabled = false;
        player.playerBallAnim.enabled = false;
        if (UIController.HasInstance)
        {
            UIController.Instance.ActiveFadePanel(true);
            UIController.Instance.FadePanel.StartFadeToBlack();
        }

        yield return new WaitForSeconds(1.5f);

        RespawnController.Instance.SetSpawnPoint(exitPoint);
        player.canMove = true;
        player.playerStandingAnim.enabled = true;
        player.playerBallAnim.enabled = true;
        UIController.Instance.FadePanel.StartFadeFromBlack();
        if (!string.IsNullOrEmpty(levelToLoad))
        {
            SceneManager.LoadScene(levelToLoad);

            if (levelToLoad.Equals("Level2"))
            {
                if (UIController.HasInstance)
                {
                    UIController.Instance.GamePanel.SetTimeRemain(180);
                }
            }
            else if (levelToLoad.Equals("LevelBoss"))
            {
                if (UIController.HasInstance)
                {
                    UIController.Instance.GamePanel.SetTimeRemain(300);
                }
            }
        }
    }
}
