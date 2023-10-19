using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomLevelButton : MonoBehaviour
{
    public GameObject PlayRandomButton;
    private void FixedUpdate()
    {
        if(User.info != null && User.info.lastcompletedlevelindex >= LevelDataGenerator.Instance.levels.Count - 1)
        {
            if(!PlayRandomButton.activeSelf) PlayRandomButton.SetActive(true);
        }
        else
        {
            if (PlayRandomButton.activeSelf) PlayRandomButton.SetActive(false);
        }
    }


    public void PlayRandom()
    {
        int level = Random.Range(70, LevelDataGenerator.Instance.levels.Count);

        LevelDataGenerator.Instance.levelBoxes[level].ClickEvent();
    }
}
