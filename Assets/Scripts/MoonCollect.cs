using System.Collections;
using UnityEngine;

public class MoonCollect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    MoonTimer moonTimer;
    Score score;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        moonTimer = FindObjectOfType<MoonTimer>();
        score = FindObjectOfType<Score>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            circleCollider.enabled = false;
            spriteRenderer.color = new Color(255, 255, 255, 0.2f);
            moonTimer.moonPickUps++;
            Score.scorePoints += 200;
            score.UpdateScoreText();
            SFXController.PlaySound("MoonCollect");

            if (moonTimer.moonPickUps <= 3)
            {
                StartCoroutine(SoundDelay("MoonHowl"));
            }

            moonTimer.UpdateMoonPhase();
        }
    }

    private IEnumerator SoundDelay(string sfx)
    {
        yield return new WaitForSeconds(0.2f);

        SFXController.PlaySound(sfx);
    }
}
