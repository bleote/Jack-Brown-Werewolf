using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSkeletonWeakSpot : MonoBehaviour
{
    [SerializeField] int stompDamage = 3;
    [SerializeField] FighterSkeleton fighterSkeleton;
    Collider2D weakSpotCollider;

    private void Start()
    {
        weakSpotCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (fighterSkeleton.isDead || PlayerController.isDead)
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
                fighterSkeleton.TakeDamage(stompDamage);
            }
        }
    }
}
