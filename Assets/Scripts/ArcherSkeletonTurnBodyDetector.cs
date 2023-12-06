using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkeletonTurnBodyDetector : MonoBehaviour
{
    [SerializeField] ArcherSkeleton archerSkeleton;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            archerSkeleton.TurnBody();
        }
    }
}
