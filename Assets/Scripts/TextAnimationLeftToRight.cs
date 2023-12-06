using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimationLeftToRight : MonoBehaviour
{
    public float animationDuration = 0.5f;

    [SerializeField] private TextMeshProUGUI textMesh;
    private float screenWidth;

    void Start()
    {
        screenWidth = Screen.width;

        StartCoroutine(AnimateTextLeftToRight());
    }

    public IEnumerator AnimateTextLeftToRight()
    {
        float elapsedTime = 0f;
        float initialX = -screenWidth;
        float targetX = 0f;

        while (elapsedTime < animationDuration)
        {
            float newX = Mathf.Lerp(initialX, targetX, elapsedTime / (animationDuration));
            textMesh.rectTransform.anchoredPosition = new Vector2(newX, textMesh.rectTransform.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        elapsedTime = 0f;
        initialX = 0f;
        targetX = screenWidth;

        while (elapsedTime < animationDuration)
        {
            float newX = Mathf.Lerp(initialX, targetX, elapsedTime / (animationDuration));
            textMesh.rectTransform.anchoredPosition = new Vector2(newX, textMesh.rectTransform.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
