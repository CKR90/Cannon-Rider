using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayTimeCounter : MonoBehaviour
{
    public static PlayTimeCounter Instance;

    public TextMeshProUGUI t;

    private bool start = false;
    [HideInInspector]public float TimeToFinish = 0f;
    void Awake()
    {
        Instance = this;
        t.SetText("00:00.000");
    }

    
    void Update()
    {
        if(start)
        {
            TimeToFinish += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(TimeToFinish);
            t.SetText(time.ToString(@"mm\:ss\.fff"));
        }
    }

    public void StartCounter()
    {
        start = true;
    }
    public void StopCounter()
    {
        start = false;

        if (User.info.times.Count <= LevelDataTransfer.gamePlaySettings.levelIndex)
        {
            SoundController.instance.PlayTalk(TalkList.Best_Time_Female);
        }
        else if (TimeToFinish < User.info.times[LevelDataTransfer.gamePlaySettings.levelIndex])
        {
            SoundController.instance.PlayTalk(TalkList.Best_Time_Female);
        }
    }
    public void ResetCounter()
    {
        TimeToFinish = 0f;
        t.SetText("00:00.000");
        start = false;
    }
}
