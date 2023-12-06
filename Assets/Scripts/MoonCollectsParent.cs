using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonCollectsParent : MonoBehaviour
{
    [SerializeField] List<GameObject> moonCollectsList;
    [SerializeField] PlayerController playerController;

    private void LateUpdate()
    {
        if (!MoonTimer.fullMoon && !MoonTimer.moonCollect && playerController.isGrounded)
        {
            ResetMoonCollects();
            MoonTimer.moonCollect = true;
        } 
    }

    private void ResetMoonCollects()
    {
        for (int i = 0; i < moonCollectsList.Count; ++i)
        {
            moonCollectsList[i].GetComponent<CircleCollider2D>().enabled = true;
            moonCollectsList[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        }
    }
}
