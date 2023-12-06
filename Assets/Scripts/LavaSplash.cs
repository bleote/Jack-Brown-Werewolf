using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplash : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 3);
    }
}
