using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkeletonWeakSpot : MonoBehaviour
{
    [SerializeField] int stompDamage = 3;
    [SerializeField] ArcherSkeleton archerSkeleton;
    Collider2D weakSpotCollider;

    private void Start()
    {
        weakSpotCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (archerSkeleton.isDead || PlayerController.isDead)
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
                archerSkeleton.TakeDamage(stompDamage);
            }
        }
    }
}
