using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{

    void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs Deleted!!!");
    }


}
