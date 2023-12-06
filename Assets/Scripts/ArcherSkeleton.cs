using System.Collections;
using UnityEngine;

public class ArcherSkeleton : MonoBehaviour, IEnemy
{
    [Header("Patrol Movement")]
    [SerializeField] float moveSpeed = 1;
    public bool goingLeft = true;

    [Header("Health")]
    [SerializeField] int maxHp = 7;
    [SerializeField] int currentHp;
    private bool damageHit = false;
    public bool isDead = false;

    [Header("Attack")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform arrowSpawn;
    public bool isAttacking = false;
    public bool playerInsideAttackRange = false;
    [SerializeField] float arrowSpeed = 10;
    [SerializeField] float arrowScale = 1;

    private Rigidbody2D rb;
    private Collider2D skeletonCollider;
    private Animator animator;
    private bool hurtSound = false;
    Score score;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        score = FindObjectOfType<Score>();
    }

    void Update()
    {
        if (!isDead)
        {
            if (PlayerController.isDead)
            {
                skeletonCollider.enabled = false;
            }

            if (!PlayerController.moonFreeze)
            {
                if (isAttacking)
                {
                    animator.SetBool("isAttacking", true);
                    rb.velocity = new Vector2(0, 0);
                }
                else
                {
                    animator.SetBool("isWalking", true);

                    if (goingLeft)
                    {
                        rb.velocity = new Vector2(-moveSpeed, 0);
                    }
                    else
                    {
                        rb.velocity = new Vector2(moveSpeed, 0);
                    }
                }
            }
            else
            {
                FreezeSkeleton();
            }

            if (!damageHit)
            {
                return;
            }
            else
            {
                FreezeSkeleton();
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void TurnBody()
    {
        if (goingLeft)
        {
            goingLeft = false;
        }
        else
        {
            goingLeft = true;
        }

        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public IEnumerator checkIfPlayerIsInsideAttackRange()
    {
        yield return new WaitForSeconds(0.2f);

        if (!playerInsideAttackRange)
        {
            StopAttack();
        }
    }

    public void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    public void ArchLoading()
    {
        SFXController.PlaySound("ArchLoading");
    }

    public void ShootArrow()
    {
        if (goingLeft)
        {
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawn.position, transform.rotation);
            Rigidbody2D arrowRb = newArrow.GetComponent<Rigidbody2D>();
            arrowRb.velocity = new Vector2(-arrowSpeed, 0);
        }
        else
        {
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawn.position, transform.rotation);
            newArrow.transform.localScale = new Vector2(-arrowScale, arrowScale);
            Rigidbody2D arrowRb = newArrow.GetComponent<Rigidbody2D>();
            arrowRb.velocity = new Vector2(arrowSpeed, 0);
        }

        SFXController.PlaySound("ArrowSound");
    }

    public void FreezeSkeleton()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        rb.velocity = new Vector2(0, 0);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            Damage(damage);
            Score.scorePoints += 100;
            score.UpdateScoreText();

            if (currentHp <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator DamageHold()
    {
        damageHit = true;

        yield return new WaitForSeconds(0.3f);

        damageHit = false;
    }

    void Damage(int damage)
    {
        currentHp -= damage;
        StartCoroutine(DamageHold());
        animator.SetTrigger("damageTrigger");

        if (!hurtSound)
        {
            hurtSound = true;
            StartCoroutine(SkeletonHurtSound());
        }
    }

    private IEnumerator SkeletonHurtSound()
    {
        SFXController.PlaySound("SkeletonHurt");

        yield return new WaitForSeconds(0.28f);

        hurtSound = false;
    }

    void Die()
    {
        SFXController.StopSound("SkeletonHurt");
        isDead = true;
        animator.SetBool("isDead", true);
        SFXController.PlaySound("SkeletonDeath");
        GetComponent<Collider2D>().enabled = false;
    }
}
