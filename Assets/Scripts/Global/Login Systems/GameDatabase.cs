using Firebase.Auth;
using System;
using System.Collections.Generic;
using UnityEngine;
public class GameDatabase : MonoBehaviour
{
    public static GameDatabase Instance;

    private int lastCompletedLevelIndex = -1;
    private int coin = 0;
    private int basketItem = 0;
    private int killedBanana = 0;
    private int killedEggy = 0;
    private int killedOrange = 0;
    private int killedPear = 0;
    private float finishTime = 0f;
    private int apple = 0;
    private int orange = 0;
    private int pear = 0;
    private int score = 0;

    public void ApplyDataToLocalDatabase()
    {
        int level = LevelDataTransfer.gamePlaySettings.levelIndex;


        if(level > User.info.lastcompletedlevelindex)
        {
            User.info.lastcompletedlevelindex = level;
        }

        if (level >= User.info.times.Count)
        {
            User.info.times.Add(finishTime);
        }
        else if (User.info.times[level] > finishTime)
        {
            User.info.times[level] = finishTime;
        }

        User.info.coin += coin;
        User.info.apple += apple;
        User.info.orange += orange;
        User.info.score += score;
        User.info.pear += pear;

        DailyMissionDatabase.AddDailyData(DailyItem.BasketApple, apple);
        DailyMissionDatabase.AddDailyData(DailyItem.BasketApple, orange);
        DailyMissionDatabase.AddDailyData(DailyItem.BasketApple, pear);

        if (LevelDataTransfer.gamePlaySettings.basketEnable)
        {
            switch(LevelDataTransfer.gamePlaySettings.basketItem) 
            {
                case BasketItem.Apple: User.info.apple += basketItem; break;
                case BasketItem.Pear: User.info.pear += basketItem; break;
                case BasketItem.Orange: User.info.orange += basketItem; break;
            }
        }

        DataManager.SaveEncryptedString(User.info);
    }
    private void Awake()
    {
        Instance = this;
    }
    public void ResetData()
    {
        lastCompletedLevelIndex = -1;
        if (User.info != null)
        {
            lastCompletedLevelIndex = User.info.lastcompletedlevelindex;
        }
        else if (!PlayerPrefs.HasKey("LastCompletedLevelIndex"))
        {
            lastCompletedLevelIndex = PlayerPrefs.GetInt("LastCompletedLevelIndex");
        }
        coin = 0;
        basketItem = 0;
        killedBanana = 0;
        killedEggy = 0;
        killedOrange = 0;
        killedPear = 0;
        finishTime = 0f;
        apple = 0;
        orange = 0;
        pear = 0;
    }
    public int IncreaseCoin(int value)
    {
        value = Mathf.Abs(value);
        coin += value;
        return coin;
    }
    public int DecreaseCoin(int value)
    {
        value = Mathf.Abs(value);
        coin = Mathf.Max(coin - value, 0);
        return coin;
    }
    public int GetCoinSize()
    {
        return coin;
    }

    public int SetBasketItem(int value)
    {
        value = Mathf.Abs(value);
        basketItem = value;
        return basketItem;
    }
    public int GetBasketItem()
    {
        return basketItem;
    }
    public int DecreaseBasketItem(int value)
    {
        value = Mathf.Abs(value);
        basketItem = Mathf.Max(basketItem - value, 0);
        return basketItem;
    }

    public void SetFinishTime(float time)
    {
        finishTime = time;
    }


    public void AddDeadEnemy(string EnemyName)
    {
        switch (EnemyName)
        {
            case "Banana": killedBanana++; break;
            case "Eggy": killedEggy++; break;
            case "Orange": killedOrange++; break;
            case "Pear": killedPear++; break;
        }
    }
    public int GetKilledBanana()
    {
        return killedBanana;
    }
    public int GetKilledEggy()
    {
        return killedEggy;
    }
    public int GetKilledOrange()
    {
        return killedOrange;
    }
    public int GetKilledPear()
    {
        return killedPear;
    }

    public void CreateLocalDatabaseIfNotExist()
    {
        UserInfo u = DataManager.LoadEncryptedString();

        if (u == null)
        {
            u = new UserInfo();
            DataManager.SaveEncryptedString(u);
        }
        User.info = u;
    }
    public void CreateUserInfoFromLocalDB()
    {
        User.info = DataManager.LoadEncryptedString();
    }
    public void MergeLocalAndFirebaseDB(UserInfo info)
    {
        UserInfo local = DataManager.LoadEncryptedString();

        if(local == null)
        {
            local = new UserInfo();
        }

        if (local.userID == null || local.userID == info.userID) ApplyMergedData(local, info);
        else
        {
            DataManager.SaveEncryptedString(info);
            DBManager.Instance.UpdateUserInfo(info);
        }
    }
    private void ApplyMergedData(UserInfo local, UserInfo info)
    {
        local.userID = info.userID;
        local.email = info.email;
        local.displayname = info.displayname;

        if(local.lastcompletedlevelindex < info.lastcompletedlevelindex)
        {
            local.lastcompletedlevelindex = info.lastcompletedlevelindex;
            local.coin = info.coin;
            local.apple = info.apple;
            local.orange = info.orange;
            local.pear = info.pear;
            local.times = info.times;
            local.cannonlevel = info.cannonlevel;
            local.velocitylevel = info.velocitylevel;
            local.tirelevel = info.tirelevel;
            local.suspensionlevel = info.suspensionlevel;
            local.lastpaidlevelindex = info.lastpaidlevelindex;
        }
        DataManager.SaveEncryptedString(local);
        DBManager.Instance.UpdateUserInfo(local);

    }

    public void PayLevelNeeds(LevelNeeds levelNeeds, int paidLevelIndex)
    {
        User.info.coin = Mathf.Max(0, User.info.coin - levelNeeds.CoinSize);
        User.info.lastpaidlevelindex = paidLevelIndex;
       
        switch (levelNeeds.itemType)
        {
            case ItemType.apple:  User.info.apple  = Mathf.Max(0, User.info.apple  - levelNeeds.ItemSize); break;
            case ItemType.pear:   User.info.pear   = Mathf.Max(0, User.info.pear   - levelNeeds.ItemSize); break;
            case ItemType.orange: User.info.orange = Mathf.Max(0, User.info.orange - levelNeeds.ItemSize); break;
        }

        DataManager.SaveEncryptedString(User.info);

        Dictionary<string, object> datas = new Dictionary<string, object>();
        datas["lastpaidlevelindex"] = paidLevelIndex;
        datas["coin"] = User.info.coin;
        datas["apple"] = User.info.apple;
        datas["pear"] = User.info.pear;
        datas["orange"] = User.info.orange;
        DBManager.Instance.UpdateItems(datas);
    }
    public void PaySpeedUpgrade(int price, int paidlevel)
    {
        User.info.coin = Mathf.Max(User.info.coin - price, 0);
        User.info.velocitylevel = paidlevel;
        DataManager.SaveEncryptedString(User.info);

        Dictionary<string, object> datas = new Dictionary<string, object>();
        datas["velocitylevel"] = paidlevel;
        datas["coin"] = User.info.coin;
        DBManager.Instance.UpdateItems(datas);
    }
    public void PayDampingUpgrade(int price, int paidlevel)
    {
        User.info.coin = Mathf.Max(User.info.coin - price, 0);
        User.info.suspensionlevel = paidlevel;
        DataManager.SaveEncryptedString(User.info);

        Dictionary<string, object> datas = new Dictionary<string, object>();
        datas["suspensionlevel"] = paidlevel;
        datas["coin"] = User.info.coin;
        DBManager.Instance.UpdateItems(datas);
    }
    public void PayTireUpgrade(int price, int paidlevel)
    {
        User.info.coin = Mathf.Max(User.info.coin - price, 0);
        User.info.tirelevel = paidlevel;
        DataManager.SaveEncryptedString(User.info);

        Dictionary<string, object> datas = new Dictionary<string, object>();
        datas["tirelevel"] = paidlevel;
        datas["coin"] = User.info.coin;
        DBManager.Instance.UpdateItems(datas);
    }
    public void PayMaxrangeUpgrade(int price, int paidlevel)
    {
        User.info.coin = Mathf.Max(User.info.coin - price, 0);
        User.info.cannonlevel = paidlevel;
        DataManager.SaveEncryptedString(User.info);

        Dictionary<string, object> datas = new Dictionary<string, object>();
        datas["cannonlevel"] = paidlevel;
        datas["coin"] = User.info.coin;
        DBManager.Instance.UpdateItems(datas);
    }
    public void SaveLocalData()
    {
        DataManager.SaveEncryptedString(User.info);
    }

}
