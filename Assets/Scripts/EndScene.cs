using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] Image darkScreen;
    [SerializeField] GameObject darkScreenGO;
    float fadeInDuration = 1;

    TextMeshProUGUI scoreText;

    public void Awake()
    {
        StartCoroutine(FadeInDarkScreen());
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Final Score: " + Score.scorePoints.ToString("D6");

    }

    public void PlayAgain()
    {
        Score.newGame = true;
        SFXController.PlaySound("MouseClick");
        Invoke("LoadLevel1", 0.2f);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        SFXController.PlaySound("MouseClick");
        Invoke("LoadMainMenu", 0.2f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public IEnumerator FadeInDarkScreen()
    {
        darkScreen.gameObject.SetActive(true);
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeInDuration);
            darkScreen.color = new Color(0, 0, 0, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        darkScreen.color = new Color(0, 0, 0, 0);
        darkScreenGO.SetActive(false);
    }
}
