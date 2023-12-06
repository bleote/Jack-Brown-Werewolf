using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeCount : MonoBehaviour
{
    public static int playerLives = 3;
    TextMeshProUGUI lifeCountText;

    private void Awake()
    {
        lifeCountText = GameObject.FindGameObjectWithTag("LifeCountText").GetComponent<TextMeshProUGUI>();
        lifeCountText.text = playerLives.ToString();
    }
}
