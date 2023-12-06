using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    public GameObject[] HpHeartsOn;

    [SerializeField] PlayerController playerController;

    public void HeartsUp()
    {
        for (int i = 0; i < playerController.playerHp; i++)
        {
            HpHeartsOn[i].SetActive(true);
        }
    }

    public void HeartsDown(int playerHp)
    {
        HpHeartsOn[playerHp].SetActive(false);
    }

    public IEnumerator WolfHpUpdate()
    {
        yield return new WaitForSeconds(2f);

        playerController.playerHp = 5;
        HeartsUp();
    }

    public IEnumerator PlayerHpUpdate()
    {
        yield return new WaitForSeconds(2f);

        if (playerController.playerHp > 3)
        {
            playerController.playerHp = 3;
            HpHeartsOn[3].SetActive(false);
            HpHeartsOn[4].SetActive(false);
        }
    }
}
