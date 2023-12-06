using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkeletonWeakSpot : MonoBehaviour
{
    [SerializeField] WarriorSkeleton warriorSkeleton;
    [SerializeField] Animator animator;
    Collider2D weakSpotCollider;
    private bool bounceSound = false;

    private void Start()
    {
        weakSpotCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (warriorSkeleton.isDead || PlayerController.isDead)
        {
            weakSpotCollider.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!PlayerController.isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger("bounceTrigger");

                if (!bounceSound)
                {
                    bounceSound = true;
                    StartCoroutine(SkeletonBounceSound());
                }
            }
        }
    }

    public IEnumerator SkeletonBounceSound()
    {
        SFXController.PlaySound("SkeletonBounce");

        yield return new WaitForSeconds(0.28f);

        bounceSound = false;
    }
}
