using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButtonEnableControl : MonoBehaviour
{
    public GameObject LoginButton;

    void FixedUpdate()
    {
        if(!InternetCheck.internet) LoginButton.SetActive(false);
        else if (User.auth == null && !LoginButton.activeSelf) LoginButton.SetActive(true);
        else if(User.auth != null && LoginButton.activeSelf) LoginButton.SetActive(false);
    }
}
