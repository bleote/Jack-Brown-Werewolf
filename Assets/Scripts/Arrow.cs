using UnityEngine;

public class Arrow : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        if (gameObject != null)
        {
            Destroy(gameObject, 8);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerController.isDead)
        {
            if (collision.gameObject.CompareTag("Player") && !PlayerController.damageHit)
            {
                playerController.TakeDamage();
                DestroyArrow();
            }
            else if (collision.gameObject.CompareTag("Player") && PlayerController.damageHit)
            {
                DestroyArrow();
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Invoke("DestroyArrow", 0.05f);
        }
    }

    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
