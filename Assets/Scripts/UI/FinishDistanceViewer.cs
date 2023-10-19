using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishDistanceViewer : MonoBehaviour
{
    public GameObject RetryButton;
    public GameObject LeaveButton;
    public Transform Vehicle;
    public TextMeshProUGUI t;

    private int dist = 0;
    private int to = 0;

    private bool finish = false;
    void Awake()
    {
        dist = LevelDataTransfer.gamePlaySettings.roadDistance;
    }


    void Update()
    {
        if (finish) return;

        int remaining = Mathf.Max(Mathf.FloorToInt(dist - Vehicle.position.z), 0);
        if(to != remaining)
        {
            to = remaining;
            t.text = to + "m";
        }

        if(dist - Vehicle.position.z <= 0f)
        {
            finish = true;
            t.text = "0m";
            PlayTimeCounter.Instance.StopCounter();
            RetryButton.SetActive(false);
            LeaveButton.SetActive(false);

            DailyMissionDatabase.AddDailyData(DailyItem.CoveredRoad, LevelDataTransfer.gamePlaySettings.roadDistance);
            DailyMissionDatabase.AddDailyData(DailyItem.CompletedLevel, 1);
            GameController.Instance.FinishGame();
        }
    }
}
