using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static bool newGame = true;
    public static int scorePoints;
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + Score.scorePoints.ToString("D6");
    }

    public void UpdateScoreText()
    {
        StartCoroutine(FlashText());
        scoreText.text = "Score: " + Score.scorePoints.ToString("D6");
    }

    private IEnumerator FlashText()
    {
        Color whiteColor = new Color(1, 1, 1);
        Color flashColor = new Color(0.588f, 1, 1);
        int blinkCount = 2;
        float flashDuration = 0.1f;

        for (int i = 0; i < blinkCount; i++)
        {
            scoreText.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            scoreText.color = whiteColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
