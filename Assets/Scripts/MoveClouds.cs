using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClouds : MonoBehaviour
{
    [SerializeField] float cloud1TargetX = 4;
    [SerializeField] float cloud2TargetX = 4;
    [SerializeField] float cloudSpeed = 3;
    private bool moveInCloud1;
    private bool moveInCloud2;

    private RectTransform cloud1Transform;
    private RectTransform cloud2Transform;

    private void Awake()
    {
        cloud1Transform = GameObject.FindGameObjectWithTag("Cloud1").GetComponent<RectTransform>();
        cloud2Transform = GameObject.FindGameObjectWithTag("Cloud2").GetComponent<RectTransform>();
        moveInCloud1 = false;
        moveInCloud2 = false;
    }

    private void Update()
    {
        if (MoonTimer.fullMoon && (!moveInCloud1 || !moveInCloud2))
        {
            moveInCloud1 = true;
            StartCoroutine(MoveOutCloud1());
            moveInCloud2 = true;
            StartCoroutine(MoveOutCloud2());
        }

        if (!MoonTimer.fullMoon && (moveInCloud1 || moveInCloud2))
        {
            moveInCloud1 = false;
            StartCoroutine(MoveInCloud1());
            moveInCloud2 = false;
            StartCoroutine(MoveInCloud2());
        }
    }

    private IEnumerator MoveOutCloud1()
    {
        yield return new WaitForSeconds(1.0f);

        float targetX = cloud1Transform.position.x - cloud1TargetX;

        while (Vector3.Distance(cloud1Transform.position, new Vector3(targetX, cloud1Transform.position.y, cloud1Transform.position.z)) > 0.01f)
            {
            float newX = Mathf.MoveTowards(cloud1Transform.position.x, targetX, Time.deltaTime * cloudSpeed);

            cloud1Transform.position = new Vector3(newX, cloud1Transform.position.y, cloud1Transform.position.z);

            yield return null;
        }

        cloud1Transform.position = new Vector3(targetX, cloud1Transform.position.y, cloud1Transform.position.z);
    }

    private IEnumerator MoveOutCloud2()
    {
        yield return new WaitForSeconds(1.0f);

        float targetX = cloud2Transform.position.x + cloud2TargetX;

        while (Vector3.Distance(cloud2Transform.position, new Vector3(targetX, cloud2Transform.position.y, cloud2Transform.position.z)) > 0.01f)
        {
            float newX = Mathf.MoveTowards(cloud2Transform.position.x, targetX, Time.deltaTime * cloudSpeed);

            cloud2Transform.position = new Vector3(newX, cloud2Transform.position.y, cloud2Transform.position.z);

            yield return null;
        }

        cloud2Transform.position = new Vector3(targetX, cloud2Transform.position.y, cloud2Transform.position.z);
    }

    private IEnumerator MoveInCloud1()
    {
        yield return new WaitForSeconds(2);

        float targetX = cloud1Transform.position.x + cloud1TargetX;

        while (Vector3.Distance(cloud1Transform.position, new Vector3(targetX, cloud1Transform.position.y, cloud1Transform.position.z)) > 0.01f)
        {
            float newX = Mathf.MoveTowards(cloud1Transform.position.x, targetX, Time.deltaTime * cloudSpeed);

            cloud1Transform.position = new Vector3(newX, cloud1Transform.position.y, cloud1Transform.position.z);

            yield return null;
        }
    }

    private IEnumerator MoveInCloud2()
    {
        yield return new WaitForSeconds(2);

        float targetX = cloud2Transform.position.x - cloud2TargetX;

        while (Vector3.Distance(cloud2Transform.position, new Vector3(targetX, cloud2Transform.position.y, cloud2Transform.position.z)) > 0.01f)
        {
            float newX = Mathf.MoveTowards(cloud2Transform.position.x, targetX, Time.deltaTime * cloudSpeed);

            cloud2Transform.position = new Vector3(newX, cloud2Transform.position.y, cloud2Transform.position.z);

            yield return null;
        }
    }
}
