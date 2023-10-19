using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public LoginMethod loginMethod;
    public GPGSManager gpgsManager;
    public FirebaseGoogleLogin googleManager;
    public EmailLogin emailLogin;
    public DBManager dBManager;

    public float time1, time2, time3, time4, time5, destroyScriptTime;

    private float timer = 0f;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 9999;


        DataManager.dataPath = Application.persistentDataPath;
        GameDatabase.Instance.CreateLocalDatabaseIfNotExist();
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > time1 && !loginMethod.enabled) loginMethod.enabled = true;
        if (timer > time2 && !gpgsManager.enabled) gpgsManager.enabled = true;
        if (timer > time3 && !googleManager.enabled) googleManager.enabled = true;
        if (timer > time4 && !emailLogin.enabled) emailLogin.enabled = true;
        if (timer > time5 && !dBManager.enabled) dBManager.enabled = true;





        if (timer >= destroyScriptTime) Destroy(this);
    }


}
