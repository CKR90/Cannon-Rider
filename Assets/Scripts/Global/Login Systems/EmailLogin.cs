using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System.Collections;
using TMPro;
using System;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class EmailLogin : MonoBehaviour
{
    public static EmailLogin Instance;

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

    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;

    private static bool init = false;

    Action<bool, LoginResultReason> result;
    CurrentScene currentScene;

    private void Start()
    {
        if(!init)
        {
            init = true;
            Instance = this;
            Result("Could not resolve all firebase dependincies: " + dependencyStatus, false, Enum_EmailLoginResult.CouldNotResolveAllFirebaseDependincies);
            //StartCoroutine(CheckAndFixDependenciesAsync());
        }
        
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();

            yield return new WaitForEndOfFrame();
        }
        else
        {
            Result("Could not resolve all firebase dependincies: " + dependencyStatus, false, Enum_EmailLoginResult.CouldNotResolveAllFirebaseDependincies);
        }
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
    public void LoginWithData(EmailLoginData data, Action<bool, LoginResultReason> result, CurrentScene currentScene)
    {
        this.result = result;
        this.currentScene = currentScene;


        if(LoginBlocker != null) LoginBlocker.SetActive(true);
        if(LoginDarker != null) LoginDarker.SetActive(true);
        StartCoroutine(LoginAsync(data));
    }
    public void SigninWithData(EmailLoginData data)
    {
        SigninBlocker.SetActive(true);
        SigninDarker.SetActive(true);
        StartCoroutine(SignInAsync(data));
    }
    private IEnumerator LoginAsync(EmailLoginData data)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(data.Email, data.Password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed!\n";
            Enum_EmailLoginResult Fresult = Enum_EmailLoginResult.Null;

            switch (authError)
            {
                case AuthError.InvalidEmail: failedMessage += "Email is Invalid"; Fresult = Enum_EmailLoginResult.EmailIsInvalid; break;
                case AuthError.WrongPassword: failedMessage += "Email or Password is Wrong"; Fresult = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                case AuthError.MissingEmail: failedMessage += "Email or Password is Wrong"; Fresult = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                case AuthError.MissingPassword: failedMessage += "Email or Password is Wrong"; Fresult = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
            }
            Result(failedMessage, false, Fresult);
            if (currentScene == CurrentScene.Starter) result(false, LoginResultReason.FailWhenStarterScene_Email);
            else if (currentScene == CurrentScene.Menu) result(false, LoginResultReason.FailWhenMenuScene_Email);
        }
        else
        {
            FirebaseUser user = loginTask.Result;

            if (!user.IsEmailVerified)
            {
                SendEmailforVerification();
            }
            else
            {
                AuthData _data = new AuthData();
                _data.userID = loginTask.Result.UserId;
                _data.email = loginTask.Result.Email;
                _data.displayName = loginTask.Result.DisplayName;
                User.auth = _data;

                LoginMethod.Instance.SetLoginMethod(LoginWith.Email);
                Result("Login Success", true, Enum_EmailLoginResult.LoginSuccess);
                if (currentScene == CurrentScene.Starter) result(true, LoginResultReason.SuccessWhenStarterScene_Email);
                else if (currentScene == CurrentScene.Menu) result(true, LoginResultReason.SuccessWhenMenuScene_Email);

                PlayerPrefs.SetString("Email", data.Email);
                PlayerPrefs.SetString("Password", data.Password);
            }
        }
    }
    private IEnumerator SignInAsync(EmailLoginData data)
    {
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(data.Email, data.Password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)(firebaseException.ErrorCode);

            string failedMessage = "Registration Failed!\n";
            Enum_EmailLoginResult result = Enum_EmailLoginResult.Null;

            switch (authError)
            {
                case AuthError.EmailAlreadyInUse: failedMessage += "Email Already Use"; result = Enum_EmailLoginResult.EmailAlreadyUse; break;
                case AuthError.InvalidEmail: failedMessage += "Email is Invalid"; result = Enum_EmailLoginResult.EmailIsInvalid; break;
                case AuthError.WrongPassword: failedMessage += "Email or Password is Wrong"; result = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                case AuthError.MissingEmail: failedMessage += "Email or Password is Wrong"; result = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                case AuthError.MissingPassword: failedMessage += "Email or Password is Wrong"; result = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
            }
            Result(failedMessage, false, result);
        }
        else
        {
            user = registerTask.Result;

            UserProfile userProfile = new UserProfile { DisplayName = data.UserName };

            var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

            if (updateProfileTask.Exception != null)
            {
                user.DeleteAsync();

                FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)(firebaseException.ErrorCode);

                string failedMessage = "Profile Update Failed!\n";
                Enum_EmailLoginResult result = Enum_EmailLoginResult.Null;

                switch (authError)
                {
                    case AuthError.InvalidEmail: failedMessage += "Email is Invalid"; result = Enum_EmailLoginResult.EmailIsInvalid; break;
                    case AuthError.WrongPassword: failedMessage += "Email or Password is Wrong"; result = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                    case AuthError.MissingEmail: failedMessage += "Email or Password is Wrong"; result = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                    case AuthError.MissingPassword: failedMessage += "Email or Password is Wrong"; result = Enum_EmailLoginResult.EmailOrPasswordIsWrong; break;
                }
                Result(failedMessage, false, result);
            }
            else
            {
                SendEmailforVerification();
            }
        }
    }
    private void SendEmailforVerification()
    {
        StartCoroutine(SendEmailforVerificationAsync());
    }
    private IEnumerator SendEmailforVerificationAsync()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            var sendEmailTask = FirebaseAuth.DefaultInstance.CurrentUser.SendEmailVerificationAsync();
            yield return new WaitUntil(() => sendEmailTask.IsCompleted);

            if (sendEmailTask.Exception != null)
            {
                FirebaseException firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Unknown Error, Please Try Again Later.";
                Enum_EmailLoginResult result = Enum_EmailLoginResult.UnknownError;

                switch (error)
                {
                    case AuthError.UnverifiedEmail: failedMessage = "Please Verify Your Email"; result = Enum_EmailLoginResult.VerificationCancelled; break;
                    case AuthError.Cancelled: failedMessage = "Email verification was cancelled"; result = Enum_EmailLoginResult.VerificationCancelled; break;
                    case AuthError.TooManyRequests: result = Enum_EmailLoginResult.TooManyRequest; break;
                    case AuthError.InvalidEmail: failedMessage += "Email is Invalid"; result = Enum_EmailLoginResult.EmailIsInvalid; break;
                    default: failedMessage = error.ToString(); result = Enum_EmailLoginResult.VerificationCancelled; break;
                }
                Result(failedMessage, false, result);
            }
            else
            {
                VerificationEmailPanel();
            }
        }
    }
    private void VerificationEmailPanel()
    {
        string message = "Please verify your email address before login. \n Verification email has been sent to\n";
        message += FirebaseAuth.DefaultInstance.CurrentUser.Email;

        EmailVerificationText.SetText(message);
        EmailVerificationPanel.SetActive(true);
        Result("", true, Enum_EmailLoginResult.VerifyEmail);
    }
    public void ResetPassword(string email, TextMeshProUGUI SuccessText, TextMeshProUGUI ErrorText, Action<bool> SetResetPanelState)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if(task.Exception != null)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Unknown Error, Please Try Again Later.";

                switch (error)
                {
                    case AuthError.InvalidEmail: failedMessage = "Email is Invalid"; break;
                    case AuthError.UserNotFound: failedMessage = "User not found"; break;
                    default: failedMessage = error.ToString(); break;
                }

                ErrorText.SetText(failedMessage);
                SetResetPanelState(true);
            }
            else
            {
                
                ResetPanel.SetActive(false);
                Login.SetActive(true);
                SetResetPanelState(false);
                StartCoroutine(UpdateLoginPanelAsync(email, SuccessText, "Successfully Send Email For Reset Password."));
            }
        });

        
    }
    private IEnumerator UpdateLoginPanelAsync(string email, TextMeshProUGUI result, string message)
    {
        yield return new WaitForEndOfFrame();

        LoginPanelEmailText.SetText(email);
        result.SetText(message);
    }


    private IEnumerator ResultAsync(string message, bool Success, Enum_EmailLoginResult result)
    {

        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Menu");

        LoginDarker.SetActive(false);
        SigninDarker.SetActive(false);

        if (result == Enum_EmailLoginResult.LoginSuccess)
        {
            LoginSuccess.SetText(message);
            Invoke("DisableBlockers", 1f);
            Invoke("DisableLoginPanel", 1f);
            Invoke("DisableSettingsPanel", 1f);
        }
        else if (!Success)
        {
            LoginFail.SetText(message);
            DisableBlockers();
        }

        if (result == Enum_EmailLoginResult.RegisterSuccess)
        {
            SigninSuccess.SetText(message);
            Invoke("DisableBlockers", 1.5f);
            Invoke("DisableLoginPanel", 1.5f);

        }
        else if (!Success)
        {
            SigninFail.SetText(message);
            DisableBlockers();
        }

        if (result == Enum_EmailLoginResult.VerifyEmail)
        {
            DisableBlockers();
            Login.SetActive(true);
            Register.SetActive(false);
        }
    }
    private void Result(string message, bool Success, Enum_EmailLoginResult result)
    {
        if(SceneManager.GetActiveScene().name == "Starter")
        {
            ResultAsync(message, Success, result);
            return;
        }

        if(LoginDarker != null) LoginDarker.SetActive(false);
        if (SigninDarker != null) SigninDarker.SetActive(false);

        if (result == Enum_EmailLoginResult.LoginSuccess)
        {
            if(LoginSuccess != null) LoginSuccess.SetText(message);
            Invoke("DisableBlockers", 1f);
            Invoke("DisableLoginPanel", 1f);
            Invoke("DisableSettingsPanel", 1f);
        }
        else if (!Success)
        {
            if(LoginFail != null) LoginFail.SetText(message);
            DisableBlockers();
        }

        if (result == Enum_EmailLoginResult.RegisterSuccess)
        {
            if(SigninSuccess != null) SigninSuccess.SetText(message);
            Invoke("DisableBlockers", 1.5f);
            Invoke("DisableLoginPanel", 1.5f);

        }
        else if (!Success)
        {
            if (SigninFail != null) SigninFail.SetText(message);
            DisableBlockers();
        }

        if (result == Enum_EmailLoginResult.VerifyEmail)
        {
            DisableBlockers();
            if (Login != null) Login.SetActive(true);
            if (Register != null) Register.SetActive(false);
        }
    }



    private void DisableBlockers()
    {
        if (LoginBlocker != null)
            LoginBlocker.SetActive(false);
        if (SigninBlocker != null)
            SigninBlocker.SetActive(false);
        if (LoginSuccess != null)
            LoginSuccess.SetText("");
        if (SigninSuccess != null)
            SigninSuccess.SetText("");
    }
    private void DisableLoginPanel()
    {
        if (LoginPanel != null)
            LoginPanel.SetActive(false);
    }
    private void DisableSettingsPanel()
    {
        if (SettingsPanel != null)
            SettingsPanel.SetActive(false);
    }
}
