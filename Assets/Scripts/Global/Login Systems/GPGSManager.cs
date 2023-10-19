using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class GPGSManager : MonoBehaviour
{
    public GameObject LoginPanel;

    private string authCode = "";
    private Firebase.Auth.FirebaseAuth auth;
    public static GPGSManager instance;

    private void Start()
    {
        instance = this;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }


    public void SignIn(Action<bool, LoginResultReason> result, CurrentScene currentScene)
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            result(false, LoginResultReason.TryingPlayGamesAuthOnComputer);
            return;
        }

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                AuthFirebase(result, currentScene);
            }
            else
            {
                if (currentScene == CurrentScene.Starter) result(false, LoginResultReason.FailWhenStarterScene_PlayGames);
                else if (currentScene == CurrentScene.Menu) result(false, LoginResultReason.FailWhenMenuScene_PlayGames);
            }
        });
    }
    private void AuthFirebase(Action<bool, LoginResultReason> result, CurrentScene currentScene)
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.Auth.Credential credential =
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                if (currentScene == CurrentScene.Starter) result(false, LoginResultReason.FailWhenStarterScene_PlayGames);
                else if (currentScene == CurrentScene.Menu) result(false, LoginResultReason.FailWhenMenuScene_PlayGames);
                return;
            }
            if (task.IsFaulted)
            {
                if (currentScene == CurrentScene.Starter) result(false, LoginResultReason.FailWhenStarterScene_PlayGames);
                else if (currentScene == CurrentScene.Menu) result(false, LoginResultReason.FailWhenMenuScene_PlayGames);
                return;
            }
            AuthData data = new AuthData();
            data.userID = task.Result.UserId;
            data.email = task.Result.Email;
            data.displayName= task.Result.DisplayName;
            User.auth = data;

            LoginMethod.Instance.SetLoginMethod(LoginWith.PlayGames);
            if (currentScene == CurrentScene.Starter) result(true, LoginResultReason.SuccessWhenStarterScene_PlayGames);
            else if (currentScene == CurrentScene.Menu) result(true, LoginResultReason.SuccessWhenMenuScene_PlayGames);
        });
    }
}
