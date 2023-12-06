using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroArt : MonoBehaviour
{
    [SerializeField] Image bgImage;
    [SerializeField] Image bgOpacity;

    float textSpeed = 5;
    [SerializeField] RectTransform titleTransform;
    [SerializeField] RectTransform devTransform;

    public float fadeInDuration = 1;

    void Awake()
    {
        titleTransform.position = new Vector3(titleTransform.position.x, 7, titleTransform.position.z);
        devTransform.position = new Vector3(devTransform.position.x, -1, titleTransform.position.z);
        bgImage.color = new Color(0, 0, 0, 1);
    }

    private void Start()
    {
        StartCoroutine(FadeInImage());
        StartCoroutine(MoveTitle());
        StartCoroutine(MoveDev());
        StartCoroutine(ImageOpacityOn());
    }

    private IEnumerator FadeInImage()
    {
        yield return new WaitForSeconds(1);

        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeInDuration)
        {
            float rgb = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            bgImage.color = new Color(rgb, rgb, rgb, 1);

            yield return null;

            elapsedTime = Time.time - startTime;
        }
    }

    private IEnumerator ImageOpacityOn()
    {
        yield return new WaitForSeconds(3);

        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0, 0.05f, elapsedTime / fadeInDuration);
            bgOpacity.color = new Color(1, 1, 1, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }
    }

    private IEnumerator MoveTitle()
    {
        yield return new WaitForSeconds(1.5f);

        float targetY = 5;

        while (Vector3.Distance(titleTransform.position, new Vector3(titleTransform.position.x, targetY, titleTransform.position.z)) > 0.01f)
        {
            float newY = Mathf.MoveTowards(titleTransform.position.y, targetY, Time.deltaTime * textSpeed);

            titleTransform.position = new Vector3(titleTransform.position.x, newY, titleTransform.position.z);

            yield return null;
        }

        titleTransform.position = new Vector3(titleTransform.position.x, targetY, titleTransform.position.z);
    }

    private IEnumerator MoveDev()
    {
        yield return new WaitForSeconds(5);

        float targetY = 0;

        while (Vector3.Distance(devTransform.position, new Vector3(devTransform.position.x, targetY, devTransform.position.z)) > 0.01f)
        {
            float newY = Mathf.MoveTowards(devTransform.position.y, targetY, Time.deltaTime * textSpeed);

            devTransform.position = new Vector3(devTransform.position.x, newY, devTransform.position.z);

            yield return null;
        }

        devTransform.position = new Vector3(devTransform.position.x, targetY, devTransform.position.z);
    }

}
