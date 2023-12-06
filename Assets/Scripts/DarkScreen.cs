using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DarkScreen : MonoBehaviour
{
    [SerializeField] Image darkScreen;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject mainMenuButton;
    [SerializeField] GameObject levelCompletedTextAnimation;
    public float darkenDuration = 1;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    public void Awake()
    {
        StartCoroutine(FadeInDarkScreen());
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
    }

    public IEnumerator DarkenScreenAndReload()
    {
        yield return new WaitForSeconds(2.3f);

        darkScreen.gameObject.SetActive(true);
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < darkenDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / darkenDuration);
            darkScreen.color = new Color(0, 0, 0, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        darkScreen.color = new Color(0, 0, 0, 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator DarkenScreenLevelCompleted()
    {
        darkScreen.gameObject.SetActive(true);
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeOutDuration);
            darkScreen.color = new Color(0, 0, 0, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        darkScreen.color = new Color(0, 0, 0, 1);

        levelCompletedTextAnimation.SetActive(true);

        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator DarkenScreenGameOver()
    {
        yield return new WaitForSeconds(2.3f);

        darkScreen.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < darkenDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / darkenDuration);
            darkScreen.color = new Color(0, 0, 0, alpha);
            gameOverText.color = new Color(1, 1, 1, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        darkScreen.color = new Color(0, 0, 0, 1);

        playAgainButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }
}
