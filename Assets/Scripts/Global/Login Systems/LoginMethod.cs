using Firebase.Auth;
using Google;
using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMethod : MonoBehaviour
{
    public static LoginMethod Instance;

    public GameObject LoadingBlocker;
    public GameObject InitScreen;
    public GameObject LoginPanel;
    public GameObject SettingsPanel;
    public GameObject EmailPanel;
    public GameObject EmailLoginPanel;
    public GameObject EmailRegisterPanel;
    public GameObject EmailResetPasswordPanel;
    public GPGSManager playGames;
    public FirebaseGoogleLogin google;
    public EmailLogin mail;

    public TextMeshProUGUI SuccessText;
    public TextMeshProUGUI ErrorText;

    [HideInInspector] public LoginWith loginWith;


    private bool loadmenuasync = false;
    private bool blockClearError = false;
    private bool blockClearSuccess = false;


    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Firebase Instance";
        InitScreen.SetActive(true);


        Init();
    }



    private void Init()
    {
        loginWith = GetLoginMethod();

        if (loginWith == LoginWith.PlayGames)
        {
            PlayGames();
        }
        else if (loginWith == LoginWith.Google)
        {
            Google();
        }
        else if (loginWith == LoginWith.Email)
        {
            Mail();
        }
    }

    public void Logout(bool SetLoginMethodAsNull = true)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        User.auth = null;
        User.sprite = null;
    }
    public void SetLoginMethod(LoginWith loginMethod)
    {
        PlayerPrefs.SetInt("LoginWith", (int)loginMethod);
        loginWith = loginMethod;
    }

    private void PlayGames()
    {
        playGames.SignIn(LoginResult, CurrentScene.Starter);
    }
    public void Google()
    {
        google.SignIn(LoginResult, CurrentScene.Starter);

    }
    private void Mail()
    {
        if (PlayerPrefs.HasKey("Email") && PlayerPrefs.HasKey("Password"))
        {
            string email = PlayerPrefs.GetString("Email");
            string pass = PlayerPrefs.GetString("Password");

            EmailLoginData data = new EmailLoginData();
            data.Email = email;
            data.Password = pass;
            mail.LoginWithData(data, LoginResult, CurrentScene.Starter);
        }
        else
        {
            LoginResult(false, LoginResultReason.FailWhenStarterScene_Email);
        }

    }
    private LoginWith GetLoginMethod()
    {
        if (!PlayerPrefs.HasKey("LoginWith"))
        {
            PlayerPrefs.SetInt("LoginWith", 0);
        }
        return (LoginWith)PlayerPrefs.GetInt("LoginWith");
    }


    public void LoginResult(bool result, LoginResultReason reason)
    {
        if (result) LoginSuccess(reason);
        else LoginFail(reason);
    }

    private void LoginSuccess(LoginResultReason reason)
    {
        CanvasLog.Instance.Log("LoginSuccess");
        switch (reason)
        {
            case LoginResultReason.SuccessWhenStarterScene_Email: StarterScene_MailSuccess(); break;
            case LoginResultReason.SuccessWhenMenuScene_Email: MenuScene_MailSuccess(); break;

            case LoginResultReason.SuccessWhenStarterScene_Google: StarterScene_GoogleSuccess(); break;
            case LoginResultReason.SuccessWhenMenuScene_Google: MenuScene_GoogleSucces(); break;

            case LoginResultReason.SuccessWhenStarterScene_PlayGames: StarterScene_PlayGamesSuccess(); break;
            case LoginResultReason.SuccessWhenMenuScene_PlayGames: MenuScene_PlayGamesSuccess(); break;
        }
    }
    private void LoginFail(LoginResultReason reason)
    {
        switch (reason)
        {
            case LoginResultReason.FailWhenStarterScene_Email: StarterScene_MailFail(); break;
            case LoginResultReason.FailWhenMenuScene_Email: MenuScene_MailFail(); break;

            case LoginResultReason.FailWhenStarterScene_Google: StarterScene_GoogleFail(); break;
            case LoginResultReason.FailWhenMenuScene_Google: MenuScene_GoogleFail(); break;

            case LoginResultReason.FailWhenStarterScene_PlayGames: StarterScene_PlayGamesFail(); break;
            case LoginResultReason.FailWhenMenuScene_PlayGames: MenuScene_PlayGamesFail(); break;

            case LoginResultReason.TryingGoogleAuthOnComputer: TryingGoogleOnComputer(); break;
            case LoginResultReason.TryingPlayGamesAuthOnComputer: TryingPlayGamesOnComputer(); break;

            case LoginResultReason.LoginMethodIsNone: LoginWithNone(); break;
            default: LoginWithNone(); break;
        }
    }

    private void StarterScene_MailSuccess()
    {
        DBManager.Instance.GetUserInfo(GetUserInfoCallbackAndLoadMenuScene);
    }
    private void StarterScene_MailFail()
    {
        StartCoroutine(OpenLoginPanel());
    }
    private void StarterScene_GoogleSuccess()
    {
        CanvasLog.Instance.Log("StarterScene_GoogleSuccess");
        DBManager.Instance.GetUserInfo(GetUserInfoCallbackAndLoadMenuScene);
    }
    private void StarterScene_GoogleFail()
    {
        StartCoroutine(OpenLoginPanel());
    }
    private void StarterScene_PlayGamesSuccess()
    {
        DBManager.Instance.GetUserInfo(GetUserInfoCallbackAndLoadMenuScene);
    }
    private void StarterScene_PlayGamesFail()
    {
        StartCoroutine(OpenLoginPanel());
    }
    private void MenuScene_MailSuccess()
    {
        DBManager.Instance.GetUserInfo(GetUserInfoCallback);
    }
    private void MenuScene_MailFail()
    {
        if (LoadingBlocker != null) LoadingBlocker.SetActive(false);
    }
    private void MenuScene_GoogleSucces()
    {
        CanvasLog.Instance.Log("MenuScene_GoogleSucces");
        DBManager.Instance.GetUserInfo(GetUserInfoCallback);
    }
    private void MenuScene_GoogleFail()
    {
        CanvasLog.Instance.Log("MenuScene_GoogleFail");
        if (ErrorText != null) ErrorText.SetText("Login Failed,\nPlease Try Again Later");
        if (LoadingBlocker != null) LoadingBlocker.SetActive(false);
    }
    private void MenuScene_PlayGamesSuccess()
    {
        DBManager.Instance.GetUserInfo(GetUserInfoCallback);
    }        
    private void MenuScene_PlayGamesFail()
    {
        if (ErrorText != null) ErrorText.SetText("Please restart game and try again,\nor continue with another login methods.");
        if (LoadingBlocker != null) LoadingBlocker.SetActive(false);
    }

    private void LoginWithNone()
    {
        SceneLoaderAsync.Instance.LoadSceneAsync("Menu");
    }
    private void TryingPlayGamesOnComputer()
    {
        if (SceneManager.GetActiveScene().name == "Starter")
        {
            StartCoroutine(OpenLoginPanel());
        }
        else
        {
            if (ErrorText != null) ErrorText.SetText("Login Failed");
            if (LoadingBlocker != null) LoadingBlocker.SetActive(false);
        }

    }
    private void TryingGoogleOnComputer()
    {
        CanvasLog.Instance.Log("TryingGoogleOnComputer");
        if (SceneManager.GetActiveScene().name == "Starter")
        {
            StartCoroutine(OpenLoginPanel());
        }
        else
        {
            if (ErrorText != null) ErrorText.SetText("Login Failed");
            if (LoadingBlocker != null) LoadingBlocker.SetActive(false);
        }
    }


    private IEnumerator<WaitUntil> OpenLoginPanel()
    {
        loadmenuasync = true;
        yield return new WaitUntil(predicate: () => SceneManager.GetActiveScene().name == "Menu");


        if (InternetCheck.internet)
        {
            if (!PlayerPrefs.HasKey("LoginComplete")) LoginPanel.SetActive(true);
        }
    }
    private void GetUserInfoCallback(UserInfo info)
    {
        if (info == null)
        {
            DBManager.Instance.UpdateUserInfo(User.info);
        }
        else
        {
            GameDatabase.Instance.MergeLocalAndFirebaseDB(info);
        }
        if (!PlayerPrefs.HasKey("LoginComplete")) PlayerPrefs.SetInt("LoginComplete", 1);
        if (LoginPanel != null) LoginPanel.SetActive(false);
        if (LoadingBlocker != null) LoadingBlocker.SetActive(false);

        LevelDataGenerator.Instance.ReGenerateLevelBoxes();
    }
    private void GetUserInfoCallbackAndLoadMenuScene(UserInfo info)
    {

        if (info == null)
        {
            DBManager.Instance.UpdateUserInfo(User.info);
        }
        else
        {
            GameDatabase.Instance.MergeLocalAndFirebaseDB(info);
        }
        if (!PlayerPrefs.HasKey("LoginComplete")) PlayerPrefs.SetInt("LoginComplete", 1);
        loadmenuasync = true;
    }

    private void Update()
    {
        if (loadmenuasync)
        {
            loadmenuasync = false;
            SceneLoaderAsync.Instance.LoadSceneAsync("Menu");
        }

        if (ErrorText != null && ErrorText.text != "" && !blockClearError)
        {
            blockClearError = true;
            Invoke("ClearErrorText", 3f);
        }
        if (SuccessText != null && SuccessText.text != "" && !blockClearSuccess)
        {
            blockClearSuccess = true;
            Invoke("ClearSuccessText", 3f);
        }
    }


    private void ClearErrorText()
    {
        blockClearError = false;
        ErrorText.SetText("");
    }
    private void ClearSuccessText()
    {
        SuccessText.SetText("");
    }
}