using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    PlayerController playerController;
    DarkScreen darkScreen;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        darkScreen = FindObjectOfType<DarkScreen>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.inputFreeze = true;
            StartCoroutine(darkScreen.DarkenScreenLevelCompleted());
        }
    }
}
