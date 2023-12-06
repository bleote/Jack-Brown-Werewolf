using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static bool sfxOn = true;
    public static int scene = 1;

    [SerializeField] private GameObject soundPannel;
    [SerializeField] private GameObject objectSoundPlayer;

    public static void PlaySound(string sfx)
    {
        if (sfxOn)
        {
            GameObject soundObject = GameObject.FindGameObjectWithTag(sfx);
            AudioSource source = soundObject.GetComponent<AudioSource>();
            source.Play();
        }
    }

    public static void StopSound(string sfx)
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag(sfx);
        AudioSource source = soundObject.GetComponent<AudioSource>();
        source.Stop();
    }

    public void SfxSwitch()
    {
        if (sfxOn)
        {
            sfxOn = false;
            AudioListener.pause = true;
        }
        else
        {
            sfxOn = true;
            AudioListener.pause = false;
        }
    }
}
