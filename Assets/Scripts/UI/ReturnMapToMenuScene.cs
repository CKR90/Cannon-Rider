using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReturnMapToMenuScene : MonoBehaviour
{
    public Image darker;

    public void Return()
    {

        darker.DOFade(1f, 1f);
        SoundController.instance.StopEndGameMusic();
        SoundController.instance.PlayMenuMusic();
        SceneLoaderAsync.Instance.LoadSceneAsync("Menu");
    }
}
