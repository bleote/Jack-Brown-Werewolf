using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonTimer : MonoBehaviour
{
    public int moonPickUps = 0;
    public static bool fullMoon = false;
    public static bool moonCollect = true;
    bool moonPickUpsReset = false;

    [SerializeField] List<GameObject> moonPhaseImages;
    [SerializeField] Image fullMoonTimerImage;
    [SerializeField] Image fullMoonBG;
    [SerializeField] float TimeToFinishFullMoon = 30;
    float fullMoonTimer;
    public float moonFillFraction;

    PlayerController playerController;
    HealthPoints healthPoints;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        healthPoints = FindObjectOfType<HealthPoints>();
    }

    void Update()
    {

        if (moonPickUps == 4 && !moonPickUpsReset)
        {
            moonPickUpsReset = true;
            fullMoon = true;
            moonPhaseImages[4].SetActive(true);
            playerController.FullMoonStartFreeze();
            StartCoroutine(healthPoints.WolfHpUpdate());
        }

        if (fullMoon)
        {
            fullMoonTimerImage.fillAmount = moonFillFraction;

            if (!PlayerController.moonFreeze)
            {
                moonPhaseImages[4].SetActive(false);
                fullMoonTimerImage.gameObject.SetActive(true);
                fullMoonBG.gameObject.SetActive(true);
                moonCollect = false;
                UpdateTimer();
            }
        }
        else
        {
            fullMoonTimer = TimeToFinishFullMoon;
        }
    }

    void UpdateTimer()
    {
        fullMoonTimer -= Time.deltaTime;

        if (fullMoonTimer > 0)
        {
            moonFillFraction = fullMoonTimer / TimeToFinishFullMoon;
        }
        else
        {
            moonPhaseImages[0].SetActive(true);
            fullMoonTimerImage.gameObject.SetActive(false);
            fullMoonBG.gameObject.SetActive(false);
            fullMoon = false;
            moonPickUpsReset = false;
            moonPickUps = 0;
        }
    }

    public void UpdateMoonPhase()
    {
        for (int i = 0; i < moonPhaseImages.Count; i++)
        {
            moonPhaseImages[i].SetActive(i == moonPickUps);
        }
    }
}
