using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PackParent;
    public ScrollLevelBoxes_Initializer scrollLevelBoxes_Initializer;
    public void Open()
    {
        scrollLevelBoxes_Initializer.AwakeInit = true;

        StartCoroutine(LateOpen());
    }

    private IEnumerator LateOpen()
    {
        yield return new WaitForEndOfFrame();

        PackParent.SetActive(true);
        MainMenu.SetActive(false);
    }
}
