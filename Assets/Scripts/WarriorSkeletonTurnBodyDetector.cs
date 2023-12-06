using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkeletonTurnBodyDetector : MonoBehaviour
{
    [SerializeField] WarriorSkeleton warriorSkeleton;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            warriorSkeleton.TurnBody();
        }
    }
}
