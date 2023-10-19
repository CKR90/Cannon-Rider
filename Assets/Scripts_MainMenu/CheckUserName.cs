using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckUserName : MonoBehaviour
{
    private float timer = 0f;
    private TextMeshProUGUI tm;
    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
    }

    
    void FixedUpdate()
    {
        if(User.info != null && !string.IsNullOrEmpty(User.info.displayname))
        {
            if(tm.text != User.info.displayname) tm.SetText(User.info.displayname);
        }
    }
}
