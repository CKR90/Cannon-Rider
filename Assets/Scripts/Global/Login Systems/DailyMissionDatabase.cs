using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

public class DailyMissionDatabase : MonoBehaviour
{
    private static DailyData dailyData = new DailyData();
    private static DailyData dailyCopy;

    private static readonly byte[] Key = Encoding.ASCII.GetBytes("fgg4fku+F-<CBgi/");
    private static readonly byte[] IV = Encoding.ASCII.GetBytes("+AqCKR#^64Omr,/a");
    private static readonly string fileName = "/D4DAD04.txt";

    public static void ApplyDaily()
    {
        SaveEncryptedString();
    }
    public static DailyData LoadDaily()
    {
        LoadEncryptedString();
        return dailyData;
    }
    public static void ResetDaily(string newEndTime)
    {

        dailyData = new DailyData();
        dailyData.endTime = newEndTime;
        SaveEncryptedString();
    }
    public static void UpdateBasketData()
    {
        int size = GameDatabase.Instance.GetBasketItem();

        switch(LevelDataTransfer.gamePlaySettings.basketItem)
        {
            case BasketItem.Apple: AddDailyData(DailyItem.BasketApple, size); break;
            case BasketItem.Pear: AddDailyData(DailyItem.BasketPear, size); break;
            case BasketItem.Orange: AddDailyData(DailyItem.BasketOrange, size); break;
        }
    }
    public static void AddDailyData(DailyItem dailyItem, int size)
    {
        switch (dailyItem)
        {
            case DailyItem.Coin: dailyData.EarnedCoin += size; break;
            case DailyItem.CoveredRoad: dailyData.CoveredRoad += size; break;
            case DailyItem.CompletedLevel: dailyData.CompletedLevel += size; break;
            case DailyItem.BasketApple: dailyData.EarnedApple += size; break;
            case DailyItem.BasketPear: dailyData.EarnedPear += size; break;
            case DailyItem.BasketOrange: dailyData.EarnedOrange += size; break;
        }
    }
    public static void AddDailyData(DailyItem dailyItem, GunType gunType, int size)
    {
        if (dailyItem == DailyItem.Pear)
        {
            switch (gunType)
            {
                case GunType.Pirate: dailyData.ExplodedPearWithPirate += size; break;
                case GunType.TNT: dailyData.ExplodedPearWithTNT += size; break;
                case GunType.Missile: dailyData.ExplodedPearWithMissile += size; break;
            }
            dailyData.ExplodedPearWithAny += size;
        }
        else if (dailyItem == DailyItem.Banana)
        {
            switch (gunType)
            {
                case GunType.Pirate: dailyData.ExplodedBananaWithPirate += size; break;
                case GunType.TNT: dailyData.ExplodedBananaWithTNT += size; break;
                case GunType.Missile: dailyData.ExplodedBananaWithMissile += size; break;
            }
            dailyData.ExplodedBananaWithAny += size;
        }
        else if (dailyItem == DailyItem.Orange)
        {
            switch (gunType)
            {
                case GunType.Pirate: dailyData.ExplodedOrangeWithPirate += size; break;
                case GunType.TNT: dailyData.ExplodedOrangeWithTNT += size; break;
                case GunType.Missile: dailyData.ExplodedOrangeWithMissile += size; break;
            }
            dailyData.ExplodedOrangeWithAny += size;
        }
        else if (dailyItem == DailyItem.Eggy)
        {
            switch (gunType)
            {
                case GunType.Pirate: dailyData.ExplodedEggyWithPirate += size; break;
                case GunType.TNT: dailyData.ExplodedEggyWithTNT += size; break;
                case GunType.Missile: dailyData.ExplodedEggyWithMissile += size; break;
            }
            dailyData.ExplodedEggyWithAny += size;
        }
    }
    public static void GetDailyCopy()
    {
        dailyCopy = new DailyData();
        dailyCopy.endTime = dailyData.endTime;
        dailyCopy.ExplodedBananaWithPirate = dailyData.ExplodedBananaWithPirate;
        dailyCopy.ExplodedBananaWithTNT = dailyData.ExplodedBananaWithTNT;
        dailyCopy.ExplodedBananaWithMissile = dailyData.ExplodedBananaWithMissile;
        dailyCopy.ExplodedBananaWithAny = dailyData.ExplodedBananaWithAny;

        dailyCopy.ExplodedPearWithPirate = dailyData.ExplodedPearWithPirate;
        dailyCopy.ExplodedPearWithTNT = dailyData.ExplodedPearWithTNT;
        dailyCopy.ExplodedPearWithMissile = dailyData.ExplodedPearWithMissile;
        dailyCopy.ExplodedPearWithAny = dailyData.ExplodedPearWithAny;

        dailyCopy.ExplodedOrangeWithPirate = dailyData.ExplodedOrangeWithPirate;
        dailyCopy.ExplodedOrangeWithTNT = dailyData.ExplodedOrangeWithTNT;
        dailyCopy.ExplodedOrangeWithMissile = dailyData.ExplodedOrangeWithMissile;
        dailyCopy.ExplodedOrangeWithAny = dailyData.ExplodedOrangeWithAny;

        dailyCopy.ExplodedEggyWithPirate = dailyData.ExplodedEggyWithPirate;
        dailyCopy.ExplodedEggyWithTNT = dailyData.ExplodedEggyWithTNT;
        dailyCopy.ExplodedEggyWithMissile = dailyData.ExplodedEggyWithMissile;
        dailyCopy.ExplodedEggyWithAny = dailyData.ExplodedEggyWithAny;

        dailyCopy.EarnedCoin = dailyData.EarnedCoin;
        dailyCopy.EarnedApple = dailyData.EarnedApple;
        dailyCopy.EarnedPear = dailyData.EarnedPear;
        dailyCopy.EarnedOrange = dailyData.EarnedOrange;

        dailyCopy.CoveredRoad = dailyData.CoveredRoad;

        dailyCopy.CompletedLevel = dailyData.CompletedLevel;
    }
    public static void ReturnDailyCopy()
    {
        dailyData.endTime = dailyCopy.endTime;
        dailyData.ExplodedBananaWithPirate = dailyCopy.ExplodedBananaWithPirate;
        dailyData.ExplodedBananaWithTNT = dailyCopy.ExplodedBananaWithTNT;
        dailyData.ExplodedBananaWithMissile = dailyCopy.ExplodedBananaWithMissile;
        dailyData.ExplodedBananaWithAny = dailyCopy.ExplodedBananaWithAny;

        dailyData.ExplodedPearWithPirate = dailyCopy.ExplodedPearWithPirate;
        dailyData.ExplodedPearWithTNT = dailyCopy.ExplodedPearWithTNT;
        dailyData.ExplodedPearWithMissile = dailyCopy.ExplodedPearWithMissile;
        dailyData.ExplodedPearWithAny = dailyCopy.ExplodedPearWithAny;

        dailyData.ExplodedOrangeWithPirate = dailyCopy.ExplodedOrangeWithPirate;
        dailyData.ExplodedOrangeWithTNT = dailyCopy.ExplodedOrangeWithTNT;
        dailyData.ExplodedOrangeWithMissile = dailyCopy.ExplodedOrangeWithMissile;
        dailyData.ExplodedOrangeWithAny = dailyCopy.ExplodedOrangeWithAny;

        dailyData.ExplodedEggyWithPirate = dailyCopy.ExplodedEggyWithPirate;
        dailyData.ExplodedEggyWithTNT = dailyCopy.ExplodedEggyWithTNT;
        dailyData.ExplodedEggyWithMissile = dailyCopy.ExplodedEggyWithMissile;
        dailyData.ExplodedEggyWithAny = dailyCopy.ExplodedEggyWithAny;

        dailyData.EarnedCoin = dailyCopy.EarnedCoin;
        dailyData.EarnedApple = dailyCopy.EarnedApple;
        dailyData.EarnedPear = dailyCopy.EarnedPear;
        dailyData.EarnedOrange = dailyCopy.EarnedOrange;

        dailyData.CoveredRoad = dailyCopy.CoveredRoad;

        dailyData.CompletedLevel = dailyCopy.CompletedLevel;
    }
    public static void UpdateDailyMission(int index, DailyBox dailyBox)
    {
        dailyData.dailyMission[index] = dailyBox.Mission;
        dailyData.dailyExplodeWith[index] = dailyBox.ExplodeWith;
        dailyData.CountLimit[index] = dailyBox.CountLimit;
        dailyData.SizeLimit[index] = dailyBox.SizeLimit;
        dailyData.isAwardPaid[index] = dailyBox.isAwardPaid;
    }

    private static void SaveEncryptedString()
    {
        string json = JsonConvert.SerializeObject(dailyData);

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

                File.WriteAllBytes(Application.persistentDataPath + fileName, ms.ToArray());
            }
        }
    }
    private static void LoadEncryptedString()
    {
        if (!File.Exists(Application.persistentDataPath + fileName))
        {
            ResetDaily(DateTime.Now.AddDays(1).ToString());
            return;
        }

        byte[] encryptedData = File.ReadAllBytes(Application.persistentDataPath + fileName);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            try
            {
                using (MemoryStream ms = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            string json = sr.ReadToEnd();
                            dailyData = JsonConvert.DeserializeObject<DailyData>(json);
                        }
                    }
                }
            }
            catch
            {
                ResetDaily(DateTime.Now.AddDays(1).ToString());
            }
        }
    }
    private static void SaveDailyBoxToJson(DailyData dailyData)
    {
        string json = JsonUtility.ToJson(dailyData);
        string filePath = Application.persistentDataPath + "/DailyJson.json";
        File.WriteAllText(filePath, json);
    }
    private static void LoadDailyBoxFromJson()
    {
        string filePath = Application.persistentDataPath + "/DailyJson.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            try
            {
                dailyData = JsonUtility.FromJson<DailyData>(json);
            }
            catch
            {
                ResetDaily(DateTime.Now.AddDays(1).ToString());
            }
        }
        else
        {
            ResetDaily(DateTime.Now.AddDays(1).ToString());
        }
    }

}

public class DailyData
{
    public string endTime = "";

    public int ExplodedBananaWithPirate = 0;
    public int ExplodedBananaWithTNT = 0;
    public int ExplodedBananaWithMissile = 0;
    public int ExplodedBananaWithAny = 0;

    public int ExplodedPearWithPirate = 0;
    public int ExplodedPearWithTNT = 0;
    public int ExplodedPearWithMissile = 0;
    public int ExplodedPearWithAny = 0;

    public int ExplodedOrangeWithPirate = 0;
    public int ExplodedOrangeWithTNT = 0;
    public int ExplodedOrangeWithMissile = 0;
    public int ExplodedOrangeWithAny = 0;

    public int ExplodedEggyWithPirate = 0;
    public int ExplodedEggyWithTNT = 0;
    public int ExplodedEggyWithMissile = 0;
    public int ExplodedEggyWithAny = 0;

    public int EarnedCoin = 0;
    public int EarnedApple = 0;
    public int EarnedPear = 0;
    public int EarnedOrange = 0;

    public int CoveredRoad = 0;

    public int CompletedLevel = 0;

    public DailyMission[] dailyMission = new DailyMission[] { DailyMission.None, DailyMission.None, DailyMission.None, DailyMission.None };
    public DailyExplodeWith[] dailyExplodeWith = new DailyExplodeWith[] { DailyExplodeWith.None, DailyExplodeWith.None, DailyExplodeWith.None, DailyExplodeWith.None };
    public int[] CountLimit = new int[] { 0, 0, 0, 0 };
    public int[] SizeLimit = new int[] { 0, 0, 0, 0 };
    public bool[] isAwardPaid = new bool[] { false, false, false, false };
}
public enum DailyItem
{
    Coin,
    CoveredRoad,
    CompletedLevel,
    Pear,
    Banana,
    Orange,
    Eggy,
    BasketApple,
    BasketOrange,
    BasketPear
}