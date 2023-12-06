using System.Collections;
using UnityEngine;

public class ArcherSkeletonAttackArea : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ArcherSkeleton archerSkeleton;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerController.isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!PlayerController.moonFreeze)
                {
                    archerSkeleton.playerInsideAttackRange = true;
                    animator.SetBool("isAttacking", true);
                }
                else
                {
                    animator.SetBool("isAttacking", false);
                    archerSkeleton.playerInsideAttackRange = false;
                }
            }
            else
            {
                archerSkeleton.playerInsideAttackRange = false;
            }
        }
        else
        {
            archerSkeleton.playerInsideAttackRange = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            archerSkeleton.playerInsideAttackRange = false;
        }
    }
}
