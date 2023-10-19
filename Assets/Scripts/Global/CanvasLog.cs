using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasLog : MonoBehaviour
{
    public static CanvasLog Instance;

    public TextMeshProUGUI t;

    private string log = "";
    private int row = 0;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Log(string msg)
    {
        return;
        if (row >= 30)
        {
            row = 0;
            log = "";
        }

        row++;
        log += msg + "\n";
        t.SetText(log);
    }
}
