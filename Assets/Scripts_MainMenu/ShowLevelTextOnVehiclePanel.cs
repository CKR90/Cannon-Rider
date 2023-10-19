using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowLevelTextOnVehiclePanel : MonoBehaviour
{
    public TextMeshProUGUI t;
    void OnEnable()
    {
        StartCoroutine(SetData());
    }

    private IEnumerator<WaitUntil> SetData()
    {
        yield return new WaitUntil(() => LevelDataTransfer.gamePlaySettings != null);

        t.SetText("Level " + (LevelDataTransfer.gamePlaySettings.levelIndex + 1).ToString());
    }
}
