using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D playerRB;
    [SerializeField]
    private Transform groundCheckPoint;
    public Animator playerStandingAnim;
    public Animator playerBallAnim;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private BulletController bulletPrefab;
    [SerializeField]
    private Transform shootPoint;

    private float directionX;
    private float directionY;

    private bool isOnGround;
    private bool canDoubleJump;

    public float dashSpeed;
    public float dashTime;
    private float dashCounter;

    public SpriteRenderer playerStandSprite;
    public SpriteRenderer afterImage;
    public float afterImageLifetime;
    public float timeBetweenAfterImages;
    public Color afterImageColor;
    private float afterImageCounter;

    public float waitAfterDashing;
    private float dashRechargerCounter;

    public GameObject standingState;
    public GameObject ballState;
    public float waitToBall;
    private float ballCounter;

    public Transform bombDropPoint;
    public GameObject bombPrefab;
    private PlayerAbilityTracker abilities;

    public bool canMove;

    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
        canMove = true;
    }

    void Update()
    {
        if (canMove) { 

            if(dashRechargerCounter > 0)
            {
                dashRechargerCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetButtonDown("Fire2") && standingState.activeSelf && abilities.canDash)
                {
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_PLAYER_DASH);
                    }
                    dashCounter = dashTime;
                    ShowAfterImage();
                }
            }


            if(dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                playerRB.velocity = new Vector2(dashSpeed * transform.localScale.x, playerRB.velocity.y);
                afterImageCounter -= Time.deltaTime;
                if(afterImageCounter <= 0)
                {
                    ShowAfterImage();
                }
                dashRechargerCounter = waitAfterDashing;
            }
            else
            {
                //Get Input
                directionX = Input.GetAxisRaw("Horizontal");
                directionY = Input.GetAxisRaw("Vertical");

                //Move
                playerRB.velocity = new Vector2(directionX * moveSpeed, playerRB.velocity.y);

                //Flip
                if (playerRB.velocity.x < 0)
                {
                    //Left
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (playerRB.velocity.x > 0)
                {
                    //Right
                    transform.localScale = Vector3.one;
                }
            }

        

            //Check player is on ground
            isOnGround = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, groundLayer);

            //Jump
            if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDounbleJump)))
            {
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
                }
                if (isOnGround)
                {
                    canDoubleJump = true;
                }
                else
                {
                    canDoubleJump = false;
                    playerStandingAnim.SetTrigger("DoubleJump");
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_PLAYER_DOUBLE_JUMP);
                    }
                }
            
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }

            //Shoot
            if (Input.GetButtonDown("Fire1"))
            {
                if (standingState.activeSelf)
                {
                    Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation).SetMoveDirection(new Vector2(transform.localScale.x, 0f));
                    playerStandingAnim.SetTrigger("Shoot");
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_PLAYER_SHOOT);
                    }
                }

                else if (ballState.activeSelf && abilities.canDropBomb)
                {
                    Instantiate(bombPrefab, bombDropPoint.position, bombDropPoint.rotation);
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_PLAYER_MINE);
                    }
                }
            }
        }
        else
        {
            playerRB.velocity = Vector2.zero;
        }

        //Ball mode
        if (!ballState.activeSelf)
        {
            if(directionY < -0.9f && abilities.canBecomeBall)
            {
                ballCounter -= Time.deltaTime;
                if(ballCounter <= 0)
                {
                    ballState.SetActive(true);
                    standingState.SetActive(false);
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_PLAYER_BALL);
                    }
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
        else
        {
            if (directionY > 0.9f)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ballState.SetActive(false);
                    standingState.SetActive(true);
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySE(AUDIO.SE_PLAYER_FROM_BALL);
                    }
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }

        //Animation
        if (standingState.activeSelf)
        {
            playerStandingAnim.SetBool("IsOnGround", isOnGround);
            playerStandingAnim.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));
        }

        if (ballState.activeSelf)
        {
            playerBallAnim.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));
        }
    }

    private void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = playerStandSprite.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);
        afterImageCounter = timeBetweenAfterImages;
    }
}
