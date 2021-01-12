using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpawnBombs : MonoBehaviour
{
    public Transform CameraRig;
    public GameObject[] balls;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBombsOverTime());
    }

    IEnumerator SpawnBombsOverTime() 
    {
        while(true)
        {
            GameObject ball = Instantiate(balls[UnityEngine.Random.Range(0, balls.Length)]);
            float angle = UnityEngine.Random.Range(0f, 360f);
            float radius = UnityEngine.Random.Range(0.75f, 1.0f);
            ball.transform.position = CameraRig.position + new Vector3(
                radius * Mathf.Sin(angle),
                UnityEngine.Random.Range(1.25f, 1.75f), 
                radius * Mathf.Cos(angle));
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
        }
    }
}
