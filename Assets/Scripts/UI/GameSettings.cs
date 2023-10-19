using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public GameObject LoginButton;
    public GameObject LogoutButton;
    void OnEnable()
    {
        if(User.auth == null)
        {
            LoginButton.SetActive(true); 
            LogoutButton.SetActive(false);
        }
        else
        {
            LoginButton.SetActive(false);
            LogoutButton.SetActive(true);
        }
    }

    void Update()
    {
        
    }
}
