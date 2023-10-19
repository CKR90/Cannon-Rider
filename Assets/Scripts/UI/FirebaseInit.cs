using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public LoginMethodData loginMethodData;
    public GPGSManagerData gpgsManagerData;
    public FirebaseGoogleLoginData firebaseGoogleLoginData;
    public EmailLoginData emailLoginData;

    private void Start()
    {
        GameObject g = GameObject.Find("Firebase Instance");
        LoginMethod lm = g.GetComponent<LoginMethod>();
        GPGSManager gp = g.GetComponent<GPGSManager>();
        FirebaseGoogleLogin fg = g.GetComponent<FirebaseGoogleLogin>();
        EmailLogin el = g.GetComponent<EmailLogin>();

        TransferLoginMethod(lm);
        gp.LoginPanel = gpgsManagerData.LoginPanel;
        TransferEmailMethod(el);

    }

    private void TransferLoginMethod(LoginMethod lm)
    {
        lm.LoadingBlocker = loginMethodData.LoadingBlocker;
        lm.LoginPanel = loginMethodData.LoginPanel;
        lm.SettingsPanel= loginMethodData.SettingsPanel;
        lm.EmailPanel = loginMethodData.EmailPanel;
        lm.EmailLoginPanel = loginMethodData.EmailLoginPanel;
        lm.EmailRegisterPanel = loginMethodData.EmailRegisterPanel;
        lm.EmailResetPasswordPanel = loginMethodData.EmailResetPasswordPanel;
        lm.SuccessText = loginMethodData.SuccessText;
        lm.ErrorText = loginMethodData.ErrorText;
    }
    private void TransferEmailMethod(EmailLogin e)
    {
        e.LoginPanel = emailLoginData.LoginPanel;
        e.SettingsPanel = emailLoginData.SettingsPanel;
        e.Login = emailLoginData.Login;
        e.Register = emailLoginData.Register;
        e.ResetPanel= emailLoginData.ResetPanel;
        e.LoginPanelEmailText = emailLoginData.LoginPanelEmailText;
        e.LoginBlocker = emailLoginData.LoginBlocker;
        e.LoginDarker = emailLoginData.LoginDarker;
        e.LoginSuccess = emailLoginData.LoginSuccess;
        e.LoginFail = emailLoginData.LoginFail;
        e.SigninBlocker = emailLoginData.SigninBlocker;
        e.SigninDarker = emailLoginData.SigninDarker;
        e.SigninSuccess = emailLoginData.SigninSuccess;
        e.SigninFail = emailLoginData.SigninFail;
        e.EmailVerificationPanel = emailLoginData.EmailVerificationPanel;
        e.EmailVerificationText = emailLoginData.EmailVerificationText;
    }


    [System.Serializable] 
    public class LoginMethodData
    {
        public GameObject LoadingBlocker;
        public GameObject LoginPanel;
        public GameObject SettingsPanel;
        public GameObject EmailPanel;
        public GameObject EmailLoginPanel;
        public GameObject EmailRegisterPanel;
        public GameObject EmailResetPasswordPanel;
        public TextMeshProUGUI SuccessText;
        public TextMeshProUGUI ErrorText;
    }
    [System.Serializable]
    public class GPGSManagerData
    {
        public GameObject LoginPanel;
    }
    [System.Serializable]
    public class FirebaseGoogleLoginData
    {
        public GameObject LoginPanel;
    }
    [System.Serializable]
    public class EmailLoginData
    {
        public GameObject LoginPanel;
        public GameObject SettingsPanel;
        public GameObject Login;
        public GameObject Register;
        public GameObject ResetPanel;
        public TextMeshProUGUI LoginPanelEmailText;
        public GameObject LoginBlocker;
        public GameObject LoginDarker;
        public TextMeshProUGUI LoginSuccess;
        public TextMeshProUGUI LoginFail;
        public GameObject SigninBlocker;
        public GameObject SigninDarker;
        public TextMeshProUGUI SigninSuccess;
        public TextMeshProUGUI SigninFail;
        public GameObject EmailVerificationPanel;
        public TextMeshProUGUI EmailVerificationText;
    }
}
