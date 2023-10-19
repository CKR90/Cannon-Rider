using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardEditor : MonoBehaviour
{
    public bool LoginTrue_SigninFalse = false;


    
    [SerializeField] private int CharacterLimit = 50;
    [SerializeField] private RectTransform Cursor;
    [SerializeField] private List<Image> AllTextBox;

    [SerializeField] private GameObject LoginKeyboardScaler;
    [SerializeField] private GameObject SigninKeyboardScaler;

    [SerializeField] private TextMeshProUGUI SuccessText;
    [SerializeField] private TextMeshProUGUI ErrorText;
    [SerializeField] private TextMeshProUGUI UserName;
    [SerializeField] private TextMeshProUGUI Email;
    [SerializeField] private TextMeshProUGUI ReEmail;
    [SerializeField] private TextMeshProUGUI Password;
    [SerializeField] private TextMeshProUGUI RePassword;

    [SerializeField] private TextMeshProUGUI UserName_Hint;
    [SerializeField] private TextMeshProUGUI Email_Hint;
    [SerializeField] private TextMeshProUGUI ReEmail_Hint;
    [SerializeField] private TextMeshProUGUI Password_Hint;
    [SerializeField] private TextMeshProUGUI RePassword_Hint;

    [SerializeField] private string UserName_HintText = "";
    [SerializeField] private string Email_HintText = "";
    [SerializeField] private string ReEmail_HintText = "";
    [SerializeField] private string Password_HintText = "";
    [SerializeField] private string RePassword_HintText = "";

    [SerializeField] private Color SelectedTextboxColor;
    [SerializeField] private Color UnselectedTextboxColor;

    [SerializeField] private Image HideShowLogin;
    [SerializeField] private Image HideShowSignin;
    [SerializeField] private Sprite ShowPassword;
    [SerializeField] private Sprite HidePassword;


    [SerializeField] private List<TextMeshProUGUI> buttons;


    private static bool ResetPasswordPanelEnabled = false;
    private bool showPassword = false;
    private string pass = "";
    private string rePass = "";



    private List<string[]> keys = new List<string[]>();
    private bool Capital = false;
    private bool Symbol = false;

    private int cursorIndex = 0;

    private TextMeshProUGUI SelectedText;

    private CursorBlink cursorBlink;

    private void Awake()
    {
        cursorBlink = Cursor.transform.GetComponent<CursorBlink>();
        GenerateKeys();
    }
    private void OnEnable()
    {
        Capital = false;
        Symbol = false;
        SetKey();
        SetSize();
        GenerateFields();
        ErrorText.SetText("");
        SuccessText.SetText("");

        LoginKeyboardScaler.SetActive(false);
        SigninKeyboardScaler.SetActive(false);

        foreach (var i in AllTextBox)
        {
            i.color = UnselectedTextboxColor;
        }
        SelectedText = null;

        showPassword = false;
        pass = "";
        rePass = "";
        CheckMemory();
    }
    void Start()
    {

    }
    void Update()
    {
        CheckHints();
        SetCursor();
        CheckKeyboard();
    }


    private void CheckMemory()
    {
        if (transform.parent.name == "LoginPanel" || transform.parent.name == "ResetPasswordPanel")
        {
            if (PlayerPrefs.HasKey("Email"))
            {
                string email = PlayerPrefs.GetString("Email");
                Email.SetText(email);
            }
            if (PlayerPrefs.HasKey("Password"))
            {
                string password = PlayerPrefs.GetString("Password");
                pass = password;

                Password.SetText("");
                if (showPassword)
                {
                    Password.SetText(pass);
                }
                else
                {
                    string p = "";
                    foreach (var v in pass)
                    {
                        p += "*";
                    }
                    Password.SetText(p);
                }
            }
        }
    }
    private void GenerateFields()
    {
        UserName.SetText("");
        Email.SetText("");
        ReEmail.SetText("");
        Password.SetText("");
        RePassword.SetText("");

        CheckHints();
    }
    private void GenerateKeys()
    {
        keys.Add(new string[] { "q", "Q", "(" });
        keys.Add(new string[] { "w", "W", ")" });
        keys.Add(new string[] { "e", "E", "[" });
        keys.Add(new string[] { "r", "R", "]" });
        keys.Add(new string[] { "t", "T", "{" });
        keys.Add(new string[] { "y", "Y", "}" });
        keys.Add(new string[] { "u", "U", "<" });
        keys.Add(new string[] { "i", "I", ">" });
        keys.Add(new string[] { "o", "O", "|" });
        keys.Add(new string[] { "p", "P", "?" });
        keys.Add(new string[] { "a", "A", ":" });
        keys.Add(new string[] { "s", "S", ";" });
        keys.Add(new string[] { "d", "D", "+" });
        keys.Add(new string[] { "f", "F", "-" });
        keys.Add(new string[] { "g", "G", "*" });
        keys.Add(new string[] { "h", "H", "/" });
        keys.Add(new string[] { "j", "J", "%" });
        keys.Add(new string[] { "k", "K", "#" });
        keys.Add(new string[] { "l", "L", "!" });
        keys.Add(new string[] { "z", "Z", "@" });
        keys.Add(new string[] { "x", "X", "£" });
        keys.Add(new string[] { "c", "C", "$" });
        keys.Add(new string[] { "v", "V", "&" });
        keys.Add(new string[] { "b", "B", "'" });
        keys.Add(new string[] { "n", "N", "\"" });
        keys.Add(new string[] { "m", "M", "~" });
        keys.Add(new string[] { "@", "@", "@" });
        keys.Add(new string[] { ".", ".", "." });
        keys.Add(new string[] { " ", " ", " " });
    }
    private void SetKey()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].text = keys[i][GetMode()].ToString();
        }
    }
    private void SetSize()
    {
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio < 1.7f)
        {
            transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * Screen.width / 1000f;
        } 
    }
    private void SetCursor()
    {
        if (SelectedText == null)
        {
            if (Cursor.gameObject.activeSelf)
            {
                Cursor.gameObject.SetActive(false);
            }
            return;
        }


        if (!Cursor.gameObject.activeSelf) Cursor.gameObject.SetActive(true);
        
        if (Cursor.transform.parent != SelectedText.transform)
        {
            Cursor.transform.SetParent(SelectedText.transform);
        }

        Vector3 pos;

        if (SelectedText.textInfo.characterCount <= 0)
        {
            pos = Vector3.zero;
        }
        else
        {
            pos = SelectedText.textInfo.characterInfo[SelectedText.textInfo.characterCount - 1].bottomRight;

            if (SelectedText.textInfo.characterInfo[SelectedText.textInfo.characterCount - 1].character == ' ')
            {
                pos.x += 15f;
            }
        }

        pos.y = 0f;

        

        Cursor.anchoredPosition3D = pos;
    }

    private void CheckHints()
    {

        if (UserName.text == "" && UserName_Hint.text == "") UserName_Hint.SetText(UserName_HintText);
        if (UserName.text != "" && UserName_Hint.text != "") UserName_Hint.SetText("");

        if (Email.text == "" && Email_Hint.text == "") Email_Hint.SetText(Email_HintText);
        if (Email.text != "" && Email_Hint.text != "") Email_Hint.SetText("");

        if (ReEmail.text == "" && ReEmail_Hint.text == "") ReEmail_Hint.SetText(ReEmail_HintText);
        if (ReEmail.text != "" && ReEmail_Hint.text != "") ReEmail_Hint.SetText("");

        if (Password.text == "" && Password_Hint.text == "") Password_Hint.SetText(Password_HintText);
        if (Password.text != "" && Password_Hint.text != "") Password_Hint.SetText("");

        if (RePassword.text == "" && RePassword_Hint.text == "") RePassword_Hint.SetText(RePassword_HintText);
        if (RePassword.text != "" && RePassword_Hint.text != "") RePassword_Hint.SetText("");
    }
    private void CheckKeyboard()
    {
        if(SelectedText == null)
        {
            if(LoginKeyboardScaler.activeSelf) LoginKeyboardScaler.SetActive(false);
            if(SigninKeyboardScaler.activeSelf) SigninKeyboardScaler.SetActive(false);
        }
        else
        {
            if (!LoginKeyboardScaler.activeSelf) LoginKeyboardScaler.SetActive(true);
            if (!SigninKeyboardScaler.activeSelf) SigninKeyboardScaler.SetActive(true);
        }
    }
    public void SelectedTextBox(int index)
    {
        switch (index)
        {
            case 0: SelectedText = UserName; break;
            case 1: SelectedText = Email; break;
            case 2: SelectedText = ReEmail; break;
            case 3: SelectedText = Password; break;
            case 4: SelectedText = RePassword; break;
        }


        foreach (var i in AllTextBox)
        {
            i.color = UnselectedTextboxColor;
        }

        if (AllTextBox.Count == 2) AllTextBox[Mathf.Min(index - 1, 1)].color = SelectedTextboxColor;
        else AllTextBox[index].color = SelectedTextboxColor;

    }

    public void Button_Capital()
    {
        Capital = !Capital;
        SetKey();
    }
    public void Button_Symbol()
    {
        Symbol = !Symbol;
        Capital = false;
        SetKey();
    }
    public void Button_Clear()
    {
        if (SelectedText == null) return;
        SelectedText.SetText("");
        cursorBlink.ResetCursorBlink();
        SetCursor();

        if (SelectedText == Password)
        {
            pass = "";
        }
        if (SelectedText == RePassword)
        {
            rePass = "";
        }
    }
    public void Button_Done()
    {
        foreach (var i in AllTextBox)
        {
            i.color = UnselectedTextboxColor;
        }
        SelectedText = null;
    }
    public void Input_Char(int index)
    {
        if (SelectedText == null) return;
        if (SelectedText.text.Length >= CharacterLimit) return;

        cursorBlink.ResetCursorBlink();

        if (SelectedText == Password)
        {
            pass += keys[index][GetMode()];
            if (showPassword) SelectedText.text = pass;
            else
            {
                SelectedText.text = "";
                foreach (char c in pass) SelectedText.text += "*";
            }
        }
        else if (SelectedText == RePassword)
        {
            rePass += keys[index][GetMode()];
            if (showPassword) SelectedText.text = rePass;
            else
            {
                SelectedText.text = "";
                foreach (char c in rePass) SelectedText.text += "*";
            }
        }
        else
        {
            SelectedText.text += keys[index][GetMode()];
        }

        SetCursor();
    }
    public void Input_Number(int number)
    {
        if (SelectedText == null) return;
        if (SelectedText.text.Length >= CharacterLimit) return;

        cursorBlink.ResetCursorBlink();

        if (SelectedText == Password)
        {
            pass += number.ToString();
            if (showPassword) SelectedText.text = pass;
            else
            {
                SelectedText.text = "";
                foreach (char c in pass) SelectedText.text += "*";
            }
        }
        else if (SelectedText == RePassword)
        {
            rePass += number.ToString();
            if (showPassword) SelectedText.text = rePass;
            else
            {
                SelectedText.text = "";
                foreach (char c in rePass) SelectedText.text += "*";
            }
        }
        else
        {
            SelectedText.text += number.ToString();
        }
        SetCursor();
    }
    public void ShowHidePassword()
    {
        showPassword = !showPassword;
        CheckPasswordVisiblity();

    }
    private void CheckPasswordVisiblity()
    {
        if (ResetPasswordPanelEnabled) return;

        if (showPassword)
        {
            Password.text = pass;
            RePassword.text = rePass;

            HideShowLogin.sprite = ShowPassword;
            HideShowSignin.sprite = ShowPassword;
        }
        else
        {
            Password.text = "";
            RePassword.text = "";

            HideShowLogin.sprite = HidePassword;
            HideShowSignin.sprite = HidePassword;

            foreach (char c in pass)
            {
                Password.text += "*";
            }
            foreach (char c in rePass)
            {
                RePassword.text += "*";
            }
        }
    }
    public void BackSpace()
    {
        if (SelectedText == null) return;

        cursorBlink.ResetCursorBlink();
        string temp = SelectedText.text;
        SelectedText.text = "";
        for (int i = 0; i < temp.Length - 1; i++)
        {
            SelectedText.text += temp[i];
        }

        if (SelectedText == Password)
        {
            temp = pass;
            pass = "";
            for (int i = 0; i < temp.Length - 1; i++)
            {
                pass += temp[i];
            }
        }

        if (SelectedText == RePassword)
        {
            temp = rePass;
            rePass = "";
            for (int i = 0; i < temp.Length - 1; i++)
            {
                rePass += temp[i];
            }
        }

        SetCursor();
    }

    private int GetMode()
    {
        int mode = 0;
        if (Capital) mode += 1;
        if (Symbol) mode += 2;
        mode = Mathf.Clamp(mode, 0, 2);
        return mode;
    }
    public void ApplyInputData()
    {
        EmailLoginData data= new EmailLoginData();

        if(ResetPasswordPanelEnabled)
        {
            data.Email = Email.text;
            SuccessText.SetText("");
            ErrorText.SetText("");

            if (!ValidateEmailResetPasswordData(data, ErrorText)) return;

            EmailLogin.Instance.ResetPassword(data.Email, SuccessText, ErrorText, SetResetPasswordPanelStateInfo);
        }
        else if(LoginTrue_SigninFalse)
        {
            data.Email = Email.text;
            data.Password = pass;

            if (!ValidateEmailLoginData(data, ErrorText)) return;

            EmailLogin.Instance.LoginWithData(data, LoginMethod.Instance.LoginResult, CurrentScene.Menu);
        }
        else
        {
            data.UserName = UserName.text;
            data.Email = Email.text;
            data.ConfirmEmail = ReEmail.text;
            data.Password = pass;
            data.ConfirmPassword = rePass;

            if (!ValidateEmailSigninData(data, ErrorText)) return;


            EmailLogin.Instance.SigninWithData(data);
        }
       
    }
    public bool ValidateEmailResetPasswordData(EmailLoginData data, TextMeshProUGUI errorText)
    {
        errorText.SetText("");

        if (string.IsNullOrEmpty(data.Email))
        {
            errorText.SetText("Email cannot be empty");
            return false;
        }
        else if (!Regex.IsMatch(data.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorText.SetText("Invalid email address");
            return false;
        }
        return true;
    }
    public bool ValidateEmailLoginData(EmailLoginData data, TextMeshProUGUI errorText)
    {
        errorText.SetText("");

        if (string.IsNullOrEmpty(data.Email))
        {
            errorText.SetText("Email cannot be empty");
            return false;
        }
        else if (!Regex.IsMatch(data.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorText.SetText("Invalid email address");
            return false;
        }
        else if (string.IsNullOrEmpty(data.Password))
        {
            errorText.SetText("Password cannot be empty");
            return false;
        }
        else if (data.Password.Length < 6)
        {
            errorText.SetText("Password must be at least 6 characters long");
            return false;
        }


        return true;
    }
    public bool ValidateEmailSigninData(EmailLoginData data, TextMeshProUGUI errorText)
    {
        errorText.SetText("");

        if (string.IsNullOrEmpty(data.UserName))
        {
            errorText.SetText("Username cannot be empty");
            return false;
        }
        else if (string.IsNullOrEmpty(data.Email))
        {
            errorText.SetText("Email cannot be empty");
            return false;
        }
        else if (string.IsNullOrEmpty(data.ConfirmEmail))
        {
            errorText.SetText("Confirm email cannot be empty");
            return false;
        }
        else if (data.Email != data.ConfirmEmail)
        {
            errorText.SetText("Email and confirm email do not match");
            return false;
        }
        else if (string.IsNullOrEmpty(data.Password))
        {
            errorText.SetText("Password cannot be empty");
            return false;
        }
        else if (string.IsNullOrEmpty(data.ConfirmPassword))
        {
            errorText.SetText("Confirm password cannot be empty");
            return false;
        }
        else if (data.Password != data.ConfirmPassword)
        {
            errorText.SetText("Password and confirm password do not match");
            return false;
        }
        else if (!Regex.IsMatch(data.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorText.SetText("Invalid email address");
            return false;
        }
        else if (data.Password.Length < 6)
        {
            errorText.SetText("Password must be at least 6 characters long");
            return false;
        }
        else if (data.Password.Distinct().Count() < 3)
        {
            errorText.SetText("Password must contain at least 3 different characters");
            return false;
        }
        else if (data.UserName.Length < 3)
        {
            errorText.SetText("Username must be at least 3 characters long");
            return false;
        }


        return true;
    }


    public void SetResetPasswordPanelStateInfo(bool value)
    {
        ResetPasswordPanelEnabled = value;
    }
}
