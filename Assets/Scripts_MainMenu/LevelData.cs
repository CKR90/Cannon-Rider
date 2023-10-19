using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable] public class LevelData
{
    public GamePlaySettings gamePlaySettings = new GamePlaySettings();
    public LevelUI levelUI = new LevelUI();
    public MapSettings mapSettings = new MapSettings();
    public PlaneEvent planeEvent = new PlaneEvent();
    public LevelNeeds levelNeeds = new LevelNeeds();
    public LevelAwards levelAwards = new LevelAwards();
}