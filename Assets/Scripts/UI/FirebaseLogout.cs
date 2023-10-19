using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseLogout : MonoBehaviour
{
    public void Logout()
    {
        LoginMethod.Instance.Logout();
    }
}
