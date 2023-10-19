using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

public static class DataManager
{
    public static string dataPath = "";
    private static byte[] Key = Encoding.ASCII.GetBytes("fgg4Gfg+F-<CBgi/");
    private static byte[] IV = Encoding.ASCII.GetBytes("+AqCKR#^64mlP,/a");
    private static string fileName = "/CRENC00.txt";

    public static void SaveEncryptedString(UserInfo userinfo)
    {
        string json = JsonConvert.SerializeObject(userinfo);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(json);
                    }
                }

                File.WriteAllBytes(dataPath + fileName, ms.ToArray());
            }
        }
    }

    public static UserInfo LoadEncryptedString()
    {
        string path = dataPath;

        if (!File.Exists(path + fileName))
        {
            return null;
        }

        byte[] encryptedData = File.ReadAllBytes(dataPath + fileName);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        string json = sr.ReadToEnd();
                        UserInfo userinfo = JsonConvert.DeserializeObject<UserInfo>(json);
                        return userinfo;
                    }
                }
            }
        }
    }
}
