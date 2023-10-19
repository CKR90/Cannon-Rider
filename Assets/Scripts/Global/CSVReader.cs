using System.IO;
using System;
using UnityEngine;

public static class CSVReader
{
    private static string filePath =  Application.dataPath + "/Resources/LevelData.csv";

    public static CSVLine ReadLine(int index)
    {
        StreamReader reader = new StreamReader(filePath);
        string[] line = new string[17];
        while (!reader.EndOfStream)
        {
            string[] values = reader.ReadLine().Split(';');
            if (Int32.Parse(values[0]) == index)
            {
                for (int i = 0; i < 17; i++)
                {
                    line[i] = values[i];
                }
                reader.Close();

                BasketItem itemType = BasketItem.None;
                if (line[1] == "A") itemType = BasketItem.Apple;
                else if (line[1] == "O") itemType = BasketItem.Orange;
                else if (line[1] == "P") itemType = BasketItem.Pear;

                return new CSVLine
                    (
                    index, itemType, 
                    ParseItemNeed(line[2]), Parse(line[2]), Parse(line[3]), 
                    Parse(line[4]), Parse(line[5]), 
                    Parse(line[6]), Bool(line[7]),
                    Bool(line[8]),  Bool(line[9]),
                    Bool(line[10]), Bool(line[11]),
                    Bool(line[12]), Bool(line[13]),
                    Bool(line[14]), line[15],
                    Parse(line[16])
                    );
            }
        }
        reader.Close();
        return null;
    }
    private static int Parse(string mixedString)
    {
        if (String.IsNullOrEmpty(mixedString))
        {
            return 0;
        }

        int sign = 1;
        if(mixedString[0] == '-') sign = -1;

        string numString = "";
        foreach (char c in mixedString)
        {
            if (Char.IsDigit(c))
            {
                numString += c;
            }
        }
        return Int32.Parse(numString) * sign;
    }
    private static ItemType ParseItemNeed(string mixedString)
    {
        if (String.IsNullOrEmpty(mixedString))
        {
            return ItemType.none;
        }
        else
        {
            if (mixedString[0] == 'A') return ItemType.apple;
            if (mixedString[0] == 'O') return ItemType.apple;
            if (mixedString[0] == 'P') return ItemType.orange;
        }
        return ItemType.none;
    }
    private static bool Bool(string s)
    {
        if(s == "1") return true;
        else return false;
    }


    public static int GetLineCount()
    {
        StreamReader reader = new StreamReader(filePath);
        int count = 0;
        while (!reader.EndOfStream)
        {
            reader.ReadLine();
            count++;
        }
        reader.Close();
        return count - 1;
    }
}

public class CSVLine
{
    public int index;
    public BasketItem itemType;
    public ItemType needItem;
    public int itemSize;
    public int coinSize;
    public int roadDistance;
    public int enemyMinSpawn;
    public int enemyMaxSpawn;
    public bool enablePlane;
    public bool enableAward;
    public bool enableTNT;
    public bool enablePirate;
    public bool enableMissile;
    public bool enableCoin;
    public bool enableProtector;
    public bool enableBooster;
    public string Unlock = "";
    public int enemyMaxSpawnTime;

    public CSVLine
        (
        int index, BasketItem itemType, ItemType needItem,
        int itemSize, int coinSize, 
        int roadDistance, 
        int enemyMinSpawn, int enemyMaxSpawn,
        bool enablePlane, bool enableAward,
        bool enableTNT, bool enablePirate,
        bool enableMissile, bool enableCoin,
        bool enableProtector, bool enableBooster,
        string Unlock, int enemyMaxSpawnTime
        )
    {
        this.index = index;
        this.itemType = itemType;
        this.needItem = needItem;
        this.itemSize = itemSize;
        this.coinSize = coinSize;
        this.roadDistance = roadDistance;
        this.enemyMinSpawn = enemyMinSpawn;
        this.enemyMaxSpawn = enemyMaxSpawn;
        this.enablePlane = enablePlane;
        this.enableAward = enableAward;
        this.enableTNT = enableTNT;
        this.enablePirate= enablePirate;
        this.enableMissile = enableMissile;
        this.enableCoin = enableCoin;
        this.enableProtector = enableProtector;
        this.enableBooster = enableBooster;
        this.Unlock = Unlock;
        this.enemyMaxSpawnTime = enemyMaxSpawnTime;
    }
}
