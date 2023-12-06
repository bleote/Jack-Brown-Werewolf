using System.Collections;
using UnityEngine;

public class WarriorSkeleton : MonoBehaviour, IEnemy
{
    [Header("Patrol Movement")]
    float moveSpeed;
    [SerializeField] float moveSpeedNormal;
    [SerializeField] float moveSpeedFast;
    public bool goingLeft = true;
    private bool isAttacking = false;
    private bool loadingAttack = false;

    [Header("Health")]
    [SerializeField] int maxHp = 13;
    [SerializeField] int currentHp;
    private bool damageHit = false;
    public bool isDead = false;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask playerLayer;


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
                if (!isAttacking && !loadingAttack)
                {
                    moveSpeed = moveSpeedNormal;
                    animator.SetBool("isWalking", true);
                }

                if (isAttacking)
                {
                    moveSpeed = moveSpeedFast;
                }

                if (loadingAttack)
                {
                    moveSpeed = 0;
                }

                if (goingLeft)
                {
                    rb.velocity = new Vector2(-moveSpeed, 0);
                }
                else
                {
                    rb.velocity = new Vector2(moveSpeed, 0);
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

    public void SkeletonPunchSound()
    {
        SFXController.PlaySound("SkeletonPunch");
    }

    public void AttackDamage()
    {
        if (!PlayerController.isDead)
        {
            if (PlayerController.damageHit)
            {
                return;
            }
            else
            {
                Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

                foreach (Collider2D player in hitPlayers)
                {
                    player.GetComponent<PlayerController>().TakeDamage();
                }
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

    public void LoadingAttackTrue()
    {
        loadingAttack = true;
    }

    public void LoadingAttackFalse()
    {
        loadingAttack = false;
    }

    public void IsAttackingTrue()
    {
        loadingAttack = false;
        isAttacking = true;
    }

    public void IsAttackingFalse()
    {
        isAttacking = false;
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

            Score.scorePoints += 300;
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

    public IEnumerator SkeletonHurtSound()
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
