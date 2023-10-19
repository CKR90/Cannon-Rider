using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InternetCheck : MonoBehaviour
{
    public static bool internet = true;

    float timer = 0f;
    private void Start()
    {
        StartCoroutine(CheckInternetConnection());
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 10f)
        {
            timer = 0f;
            StartCoroutine(CheckInternetConnection());
        }
    }



    private IEnumerator CheckInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest("http://www.google.com");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            internet = false; // true;
        }
        else
        {
            internet = false;
        }
    }
}
