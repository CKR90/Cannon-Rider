using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance;
    private DatabaseReference reference;

    private void Start()
    {
        Instance = this;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(InitializeFirebaseDatabase);
    }


    private void InitializeFirebaseDatabase(Task<DependencyStatus> task)
    {
        CanvasLog.Instance.Log("FirebaseApp.InitializeFirebaseDatabase");
        var dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            reference = FirebaseDatabase.DefaultInstance.GetReference("users");
            CanvasLog.Instance.Log("reference");
        }
    }
    public void GetUserInfo(Action<UserInfo> callback)
    {
        if (reference != null)
        {
            reference.Child(User.auth.userID).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    callback(null);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null && snapshot.Value != null)
                    {
                        UserInfo info = JsonUtility.FromJson<UserInfo>(snapshot.GetRawJsonValue());
                        User.auth.email = FirebaseAuth.DefaultInstance.CurrentUser.Email;
                        User.auth.displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
                        User.auth.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                        callback(info);
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            });
        }
        else
        {
            callback(null);
        }
    }
    public void UpdateUserInfo(UserInfo userInfo)
    {
        if (reference == null || User.auth == null) return;

        userInfo.userID = User.auth.userID;
        userInfo.email = User.auth.email;
        userInfo.displayname = User.auth.displayName;

        DataManager.SaveEncryptedString(userInfo);
        User.info = userInfo;

        string json = JsonUtility.ToJson(userInfo);
        reference.Child(User.auth.userID).SetRawJsonValueAsync(json);
    }
    public void UpdateItem(string itemName, int value)
    {
        if (reference == null || User.auth == null) return;

        reference.Child(User.auth.userID).Child(itemName).SetValueAsync(value);
    }
    public void UpdateItems(Dictionary<string, object> values)
    {
        if (reference == null || User.auth == null) return;

        reference.Child(User.auth.userID).UpdateChildrenAsync(values);
    }
}
