using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSkeletonTurnBodyDetector : MonoBehaviour
{
    [SerializeField] FighterSkeleton fighterSkeleton;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            fighterSkeleton.TurnBody();
        }
    }
}
