using UnityEngine;

public static class LevelDataTransfer
{
    public static GamePlaySettings gamePlaySettings;
    public static LevelUI levelUI;
    public static MapSettings mapSettings;
    public static PlaneEvent planeEvent;

    public static int EarnedCoin;
    public static int SavedBasketItem;

    public static void SetLevelData(LevelData d)
    {
        gamePlaySettings = d.gamePlaySettings;
        levelUI = d.levelUI;
        mapSettings = d.mapSettings;
        planeEvent= d.planeEvent;

    }
}
