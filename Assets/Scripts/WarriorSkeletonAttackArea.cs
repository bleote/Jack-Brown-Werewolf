using UnityEngine;

public class WarriorSkeletonAttackArea : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerController.isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                animator.SetBool("isAttacking", true);
            }
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        if (PlayerController.moonFreeze)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
