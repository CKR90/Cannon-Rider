using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class User
{
    public static AuthData auth;
    public static UserInfo info;
    public static Sprite sprite;
}

public class AuthData
{
    public string userID;
    public string displayName;
    public string email;
}
