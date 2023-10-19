using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Firebase.Auth;
//using TMPro;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

public class Auth : MonoBehaviour
{
    //public TextMeshProUGUI resultLine;

    //private FirebaseAuth auth;
    //private Credential credential;
    //[HideInInspector] public FirebaseUser user;

    //void Start()
    //{
    //    PlayGamesPlatform.Instance.Authenticate((status) =>
    //    {
    //        if (status == SignInStatus.Success)
    //        {
    //            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
    //            {
    //                auth = FirebaseAuth.DefaultInstance;

    //                credential = PlayGamesAuthProvider.GetCredential(code);

    //                StartCoroutine(AuthGet());

    //            });
    //        }
    //        else
    //        {
    //            resultLine.SetText("Sign Failed");
    //        }
    //    });
    //}

    //private IEnumerator AuthGet()
    //{
    //    System.Threading.Tasks.Task<FirebaseUser> task = auth.SignInWithCredentialAsync(credential);

    //    while (!task.IsCompleted) yield return null;

    //    if (task.IsCanceled)
    //    {
    //        resultLine.SetText("Auth Canceled");
    //    }
    //    else if (task.IsFaulted)
    //    {
    //        resultLine.SetText("Auth Faulted");
    //    }
    //    else
    //    {
    //        user = task.Result;
    //        resultLine.SetText("Hello: " + user.Email);
    //    }

    //}
}
