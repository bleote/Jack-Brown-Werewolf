using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject controlsPannel;
    [SerializeField] private GameObject sfxButton;

    float textSpeed = 20;
    [SerializeField] RectTransform playButtonRectTransform;
    [SerializeField] RectTransform controlsButtonRectTransform;
    [SerializeField] RectTransform sfxButtonRectTransform;

    [SerializeField] private TextMeshProUGUI soundButtonText;

    bool controlsPannelOn;
    bool playedMusicOnStart;

    private void Awake()
    {
        SetNewGame();
        UpdateSoundButtonText();
        playButtonRectTransform.position = new Vector3(-12, playButtonRectTransform.position.y, playButtonRectTransform.position.z);
        controlsButtonRectTransform.position = new Vector3(12, controlsButtonRectTransform.position.y, controlsButtonRectTransform.position.z);
        sfxButtonRectTransform.position = new Vector3(-12, sfxButtonRectTransform.position.y, sfxButtonRectTransform.position.z);
        controlsPannel.SetActive(false);
        controlsPannelOn = false;
    }

    private void Start()
    {
        StartCoroutine(MoveButton(playButtonRectTransform));
        StartCoroutine(MoveButton(controlsButtonRectTransform));
        StartCoroutine(MoveButton(sfxButtonRectTransform));

        if (SFXController.sfxOn)
        {
            Invoke("PlayBgMusic", 1);
            playedMusicOnStart = true;
        }
        else
        {
            playedMusicOnStart = false;
        }
    }

    private void SetNewGame()
    {
        Score.newGame = true;
    }

    public void CheckControlsPannelDisplay()
    {
        if (!controlsPannelOn)
        {
            controlsPannel.SetActive(true);
            controlsPannelOn = true;
        }
        else
        {
            controlsPannel.SetActive(false);
            controlsPannelOn = false;
        }
    }

    public void PlayBgMusic()
    {
        SFXController.PlaySound("BGMusic");
    }

    public void MouseClickSound()
    {
        SFXController.PlaySound("MouseClick");
    }

    public void UpdateSoundButtonText()
    {
        if (SFXController.sfxOn)
        {
            soundButtonText.text = "Sound On";
            soundButtonText.colorGradient = new VertexGradient(new Color(0, 1, 0, 1), new Color(0, 1, 0, 1), new Color(0, 0, 0, 1), new Color(0, 0, 0, 1));
        }
        else
        {
            soundButtonText.text = "Sound Off";
            soundButtonText.colorGradient = new VertexGradient(new Color(1, 0, 0, 1), new Color(1, 0, 0, 1), new Color(0, 0, 0, 1), new Color(0, 0, 0, 1));
        }
    }

    public void CheckIfPlayedMusicOnStart()
    {
        if (playedMusicOnStart)
        {
            return;
        }
        else
        {
            playedMusicOnStart = true;
            SFXController.PlaySound("BGMusic");
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameDelay());
    }

    private IEnumerator LoadGameDelay()
    {
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator MoveButton(RectTransform buttonRectTransform)
    {
        yield return new WaitForSeconds(4);

        float targetX = 0;

        while (Vector3.Distance(buttonRectTransform.position, new Vector3(targetX, buttonRectTransform.position.y, buttonRectTransform.position.z)) > 0.01f)
        {
            float newX = Mathf.MoveTowards(buttonRectTransform.position.x, targetX, Time.deltaTime * textSpeed);

            buttonRectTransform.position = new Vector3(newX, buttonRectTransform.position.y, buttonRectTransform.position.z);

            yield return null;
        }

        buttonRectTransform.position = new Vector3(targetX, buttonRectTransform.position.y, buttonRectTransform.position.z);
    }
}
