using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject TapToPlayScreen;
    public GameObject FruitPointerDarker;
    public List<Tutorial> Tutorials;

    private Tutorial t = null;

    void Start()
    {
        int levelIndex = LevelDataTransfer.gamePlaySettings.levelIndex;

        if (levelIndex < User.info.lastcompletedlevelindex)
        {
            TapToPlayScreen.SetActive(true);
            Destroy(gameObject);
            return;
        }

        t = Tutorials.Find(x => x.showAtThisIndex == levelIndex);

        if (t != null)
        {
            if(!PlayerPrefs.HasKey(t.tutorialType.ToString()))
            {
                StartTutorial();
            }
            else
            {
                TapToPlayScreen.SetActive(true);
                Destroy(FruitPointerDarker);
                Destroy(gameObject);
            }
        }
        else
        {
            TapToPlayScreen.SetActive(true);
            Destroy(FruitPointerDarker);
            Destroy(gameObject);
        }
    }

    private void StartTutorial()
    {
        switch (t.tutorialType)
        {
            case TutorialType.LearnCannonBall: GetComponent<Tutorial_LearnCannonBall>().enabled = true; break;
            case TutorialType.LearnAwardBox:   GetComponent<TutorialAwardBox>().enabled = true; break;
            case TutorialType.LearnTNT: GetComponent<TutorialTNT>().enabled = true; break;
            case TutorialType.FruitPointer: GetComponent<TutorialBasketHelper>().enabled = true; break;
        }
    }
}
