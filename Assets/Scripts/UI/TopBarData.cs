using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBarData : MonoBehaviour
{
    public static TopBarData Instance;

    public TextMeshProUGUI userName;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI apple;
    public TextMeshProUGUI pear;
    public TextMeshProUGUI orange;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InvokeRepeating("SetData", 0f, .5f);
    }

    public void SetData()
    {
        if (User.info == null)
        {
            userName.SetText("");
            coin.SetText("0");
            apple.SetText("0");
            pear.SetText("0");
            orange.SetText("0");
        }
        else
        {
            userName.SetText(User.info.displayname);
            coin.SetText(User.info.coin.ToString());
            apple.SetText(User.info.apple.ToString());
            pear.SetText(User.info.pear.ToString());
            orange.SetText(User.info.orange.ToString());
        }
    }
}
