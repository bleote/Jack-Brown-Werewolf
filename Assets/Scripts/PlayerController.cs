using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Move")]
    [SerializeField] float moveSpeed = 6;
    private float inputXDirection = 0;
    private bool bodyDirection;

    [Header("Jump")]
    [SerializeField] float jumpForce = 15;
    float coyoteTime = 0.1f;
    float coyoteTimeCounter;
    float jumpBufferTime = 0.2f;
    float jumpBufferCounter;

    [Header("Attack")]
    [SerializeField] int attackDamage = 1;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayer;
    private float attackCD = 0.3f;
    private float attackTimer;

    [Header("Player Hp")]
    public static bool isDead = false;
    private bool gameOver;
    public int playerHp = 3;
    public static bool damageHit = false;
    [SerializeField] GameObject lavaSplash;

    [Header("Moon Phases")]
    public static bool moonFreeze = false;
    bool isWolf = false;

    Rigidbody2D rb;
    Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool inputFreeze = false;
    public static bool pause = false;
    private GameObject pauseText;

    HealthPoints healthPoints;
    DarkScreen darkScreen;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI lifeCountText;

    private void Awake()
    {
        if (Score.newGame)
        {
            playerHp = 3;
            LifeCount.playerLives = 3;
            Score.scorePoints = 0;
            Score.newGame = false;
        }

        lifeCountText = GameObject.FindGameObjectWithTag("LifeCountText").GetComponent<TextMeshProUGUI>();
        lifeCountText.text = LifeCount.playerLives.ToString();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + Score.scorePoints.ToString("D6");
        gameOver = false;
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthPoints = FindObjectOfType<HealthPoints>();
        darkScreen = FindObjectOfType<DarkScreen>();
        healthPoints.HeartsUp();
        MoonTimer.fullMoon = false;
        pauseText = GameObject.FindGameObjectWithTag("PauseText");
        pauseText.SetActive(false);
        CheckSoundStatus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !gameOver)
        {
            if (pause == false)
                PauseGame();
            else
                ContinueGame();
        }

        if (pause)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ContinueGame();
                SceneManager.LoadScene("Main Menu");
            }
        }

        if (!isDead)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (!inputFreeze && Time.timeScale != 0)
            {
                inputXDirection = Input.GetAxis("Horizontal");
                attackTimer += Time.deltaTime;

                if (isWolf && !MoonTimer.fullMoon && isGrounded)
                {
                    FullMoonEndFreeze();
                    StartCoroutine(healthPoints.PlayerHpUpdate());
                }

                if (isWolf)
                {
                    jumpForce = 19;
                }
                else
                {
                    jumpForce = 15;
                }

                if (isWolf && Input.GetButton("Run"))
                {
                    moveSpeed = 9;
                }
                else
                {
                    moveSpeed = 6;
                }

                if (inputXDirection == 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(inputXDirection * moveSpeed, rb.velocity.y);
                }

                if (isGrounded)
                {
                    coyoteTimeCounter = coyoteTime;
                }
                else
                {
                    coyoteTimeCounter -= Time.deltaTime;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    jumpBufferCounter = jumpBufferTime;
                }
                else
                {
                    jumpBufferCounter -= Time.deltaTime;
                }

                if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
                {
                    if (isWolf)
                    {
                        SFXController.PlaySound("WolfJump");
                    }
                    else
                    {
                        SFXController.PlaySound("Jump");
                    }

                    jumpBufferCounter = 0;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }

                if (Input.GetButtonUp("Jump") && rb.velocity.y > 0.5f)
                {
                    coyoteTimeCounter = 0;
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                }

                animator.SetFloat("yVelocity", rb.velocity.y);

                if (attackTimer > attackCD)
                {
                    if (isWolf && Input.GetButtonDown("Attack1"))
                    {
                        AttackAnimation();
                        attackTimer = 0;
                    }

                }

                SetAnimation();

                FlipSprite();
            }
            else
            {
                if (rb.velocity.y < 2)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y - 0.05f);
                }
                CheckFreezeStatus();
            }
        }
    }

    private void AttackAnimation()
    {
        animator.SetTrigger("wolfAttackTrigger");
        SFXController.PlaySound("WolfAttack1");
    }

    public void AttackDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            IEnemy enemy = enemyCollider.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void SetAnimation()
    {
        if (!isWolf)
        {
            if (isGrounded)
            {
                animator.SetBool("isJumping", false);

                if (rb.velocity.x == 0)
                {
                    animator.SetBool("isWalking", false);
                }
                else
                {
                    animator.SetBool("isWalking", true);
                }
            }

            if (rb.velocity.y != 0 && !isGrounded)
            {
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            if (isGrounded)
            {
                animator.SetBool("wolfJumping", false);

                if (rb.velocity.x == 0)
                {
                    animator.SetBool("wolfWalking", false);
                    animator.SetBool("wolfRunning", false);
                }
                else
                {
                    if (moveSpeed == 6)
                    {
                        animator.SetBool("wolfWalking", true);
                        animator.SetBool("wolfRunning", false);
                    }
                    else
                    {
                        animator.SetBool("wolfRunning", true);
                        animator.SetBool("wolfWalking", false);
                    }
                }
            }

            if (rb.velocity.y != 0 && !isGrounded)
            {
                animator.SetBool("wolfJumping", true);
            }
            else
            {
                animator.SetBool("wolfJumping", false);
            }
        }

    }

    private void FlipSprite()
    {
        bodyDirection = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (bodyDirection)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Enemy") && !damageHit)
            {
                TakeDamage();
            }
        }
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            if (!isWolf)
            {
                SFXController.PlaySound("Hurt");
            }
            else
            {
                SFXController.PlaySound("WolfHurt");
            }


            if (playerHp == 1)
            {
                Death();
            }
            else
            {
                playerHp--;
                healthPoints.HeartsDown(playerHp);
                StartCoroutine(DamageBlinkSprite(14, 0.05f));
            }
        }
    }

    private IEnumerator DamageBlinkSprite(int totalBlinks, float blinkDuration)
    {
        damageHit = true;

        Color originalColor = spriteRenderer.color;
        Color blinkColor = originalColor;
        blinkColor.a = 0.5f;

        for (int blinkCount = 0; blinkCount < totalBlinks; blinkCount++)
        {
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkDuration);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }

        damageHit = false;
    }

    public void FullMoonStartFreeze()
    {
        moonFreeze = true;
        inputFreeze = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isJumping", false);
        animator.SetTrigger("wolfTrigger");
        isWolf = true;
        SFXController.PlaySound("WolfTransform");
        StartCoroutine(MoonFreezeCounter());
    }

    private void FullMoonEndFreeze()
    {
        moonFreeze = true;
        inputFreeze = true;
        animator.SetBool("wolfWalking", false);
        animator.SetBool("wolfJumping", false);
        animator.SetBool("wolfRunning", false);
        animator.SetTrigger("playerTrigger");
        isWolf = false;
        SFXController.PlaySound("PlayerTransform");
        StartCoroutine(MoonFreezeCounter());
    }

    private IEnumerator MoonFreezeCounter()
    {
        yield return new WaitForSeconds(3.5f);

        moonFreeze = false;
        inputFreeze = false;
    }

    public void CheckFreezeStatus()
    {
        if (moonFreeze || inputFreeze)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("wolfWalking", false);
            animator.SetBool("wolfJumping", false);
            animator.SetBool("wolfRunning", false);
        }
    }

    private void CheckSoundStatus()
    {
        if (SFXController.sfxOn)
        {
            AudioListener.pause = false;
        }
        else
        {
            {
                AudioListener.pause = true;
            }
        }
    }

    private void PauseGame()
    {
        pause = true;
        Time.timeScale = 0;
        pauseText.SetActive(true);

        if (!SFXController.sfxOn)
        {
            return;
        }
        else
        {
            AudioListener.pause = true;
        }
    }

    private void ContinueGame()
    {
        pause = false;
        Time.timeScale = 1;
        pauseText.SetActive(false);

        if (!SFXController.sfxOn)
        {
            return;
        }
        else
        {
            AudioListener.pause = false;
        }
    }

    public void LavaDeath()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        LifeCount.playerLives -= 1;
        SFXController.StopSound("BGMusic");
        SFXController.PlaySound("DeathMusic");

        if (isWolf)
        {
            SFXController.PlaySound("LavaDeathWolf");
        }
        else
        {
            SFXController.PlaySound("LavaDeath");
        }

        Instantiate(lavaSplash, transform.position, Quaternion.identity);

        for (int i = 0; i < healthPoints.HpHeartsOn.Length; i++)
        {
            healthPoints.HpHeartsOn[i].SetActive(false);
        }

        if (LifeCount.playerLives > 0)
        {
            StartCoroutine(darkScreen.DarkenScreenAndReload());
        }
        else
        {
            gameOver = true;
            StartCoroutine(darkScreen.DarkenScreenGameOver());
        }
    }

    private void Death()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        LifeCount.playerLives -= 1;
        healthPoints.HpHeartsOn[0].SetActive(false);
        SFXController.StopSound("BGMusic");
        SFXController.PlaySound("DeathMusic");

        if (!isWolf)
        {
            animator.SetTrigger("deathTrigger");
        }
        else
        {
            animator.SetTrigger("wolfDeathTrigger");
        }

        if (LifeCount.playerLives > 0)
        {
            StartCoroutine(darkScreen.DarkenScreenAndReload());
        }
        else
        {
            gameOver = true;
            StartCoroutine(darkScreen.DarkenScreenGameOver());
        }
    }
}
