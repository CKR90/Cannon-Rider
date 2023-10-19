using System;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using Google;


public class FirebaseGoogleLogin : MonoBehaviour
{
    public static FirebaseGoogleLogin Instance;
    private string GoogleWebAPI = ""; //Deleted For Security
    private GoogleSignInConfiguration configuration;
    private FirebaseAuth auth;
    private string imageUrl = "";
    private Action<bool, LoginResultReason> result;
    private CurrentScene currentScene;

    private void Start()
    {
        CanvasLog.Instance.Log("GoogleStart");
        Instance = this;
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true
        };

        InitFirebase();
    }
    private void InitFirebase()
    {
        
        auth = FirebaseAuth.DefaultInstance;

       if(auth != null) { CanvasLog.Instance.Log("Auth Found"); }
    }
    public void SignIn(Action<bool, LoginResultReason> result, CurrentScene currentScene)
    {
        CanvasLog.Instance.Log("SignIn");
        this.result = result;
        this.currentScene = currentScene;


        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            result(false, LoginResultReason.TryingGoogleAuthOnComputer);
            return;
        }

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        CanvasLog.Instance.Log("ContinueWith");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }
    private void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        CanvasLog.Instance.Log("OnGoogleAuthenticatedFinished");
        if (task.IsFaulted || task.IsCanceled)
        {
            CanvasLog.Instance.Log("task.IsFaulted || task.IsCanceled");
            if (currentScene == CurrentScene.Starter) result(false, LoginResultReason.FailWhenStarterScene_Google);
            if (currentScene == CurrentScene.Menu) result(false, LoginResultReason.FailWhenMenuScene_Google);
        }
        else
        {
            CanvasLog.Instance.Log("credential");
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    CanvasLog.Instance.Log("credential task.IsCanceled || task.IsFaulted");
                    if (currentScene == CurrentScene.Starter) result(false, LoginResultReason.FailWhenStarterScene_Google);
                    if (currentScene == CurrentScene.Menu) result(false, LoginResultReason.FailWhenMenuScene_Google);
                    return;
                }

                CanvasLog.Instance.Log("AuthData data = new AuthData();");
                AuthData data = new AuthData();
                data.userID = auth.CurrentUser.UserId;
                data.email = auth.CurrentUser.Email;
                data.displayName = auth.CurrentUser.DisplayName;

                User.auth = data;

                if (currentScene == CurrentScene.Starter) result(true, LoginResultReason.SuccessWhenStarterScene_Google);
                if (currentScene == CurrentScene.Menu) result(true, LoginResultReason.SuccessWhenMenuScene_Google);

                CanvasLog.Instance.Log("LoadImage");
                StartCoroutine(LoadImage(CheckImageUrl(auth.CurrentUser.PhotoUrl.ToString())));
                LoginMethod.Instance.SetLoginMethod(LoginWith.Google);
                
            });

        }
    }


    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }

        return imageUrl;
    }
    private IEnumerator LoadImage(string imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;


        User.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
