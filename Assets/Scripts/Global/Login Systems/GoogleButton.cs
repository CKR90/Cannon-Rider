using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleButton : MonoBehaviour
{
    public void Click()
    {
        if (LoginMethod.Instance.ErrorText != null) LoginMethod.Instance.ErrorText.SetText("");
        if (LoginMethod.Instance.SuccessText != null) LoginMethod.Instance.SuccessText.SetText("");
        if (LoginMethod.Instance.LoadingBlocker != null) LoginMethod.Instance.LoadingBlocker.SetActive(true);

        FirebaseGoogleLogin.Instance.SignIn(LoginMethod.Instance.LoginResult, CurrentScene.Menu);
    }
}
