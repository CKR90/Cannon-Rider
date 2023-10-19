using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDataGenerator : MonoBehaviour
{
    public static LevelDataGenerator Instance;

    public GameObject Blocker;
    public GameObject BackToMapsButton;
    public GameObject BackToMainMenuButton;
    public GameObject VehiclePanel;
    public Transform PackParent;
    public GameObject levelPackPrefab;
    public GameObject levelBoxPrefab;
    public GameObject PlayRandomPack;
    public Sprites sprites;
    public GameObject NeedsPopup;
    public TextMeshProUGUI NeedsPopupText;

    public List<Sprite> levelPackBackgrounds;
    public List<RoadMaterial> roadMaterials;
    public List<LevelData> levels = new List<LevelData>();



    [HideInInspector] public List<GameObject> packs = new List<GameObject>();
    [HideInInspector] public List<LevelBox> levelBoxes = new List<LevelBox>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //CreateLevels(); //Mevcut levellarý siler. Resources klasöründeki csv dosyasýna göre yeni level'lar üretir.
        GenerateLevelBoxes();
        GetDeviceInfo();

    }

    private void GetDeviceInfo()
    {

        if (User.info != null && User.info.device == null)
        {
            try
            {
                if (Application.isMobilePlatform)
                {
                    string deviceModel = SystemInfo.deviceModel;
                    string deviceName = SystemInfo.deviceName;
                    string deviceType = SystemInfo.deviceType.ToString();

                    User.info.device = deviceName + " " + deviceModel + " - " + deviceType;
                }
                else
                {
                    User.info.device = "Computer";
                }
            }
            catch
            {
                User.info.device = "Unknown";
            }
        }

    }
    private void GenerateLevelBoxes()
    {
        ClearLevelDataTransferObject();

        SoundController.instance.PlayMenuMusic();
        int packSize = (levels.Count - levels.Count % 20) / 20;

        packs.Clear();
        levelBoxes.Clear();
        for (int p = 0; p < packSize; p++)
        {
            GameObject pack = Instantiate(levelPackPrefab, PackParent);
            Vector3 packPos = Vector3.zero;
            packPos.y = -200f - p * 1720f;
            pack.GetComponent<RectTransform>().anchoredPosition3D = packPos;

            pack.name = "Chapter " + (p + 1).ToString();
            pack.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = "Chapter " + (p + 1).ToString();

            if (levelPackBackgrounds.Count > 0)
            {
                pack.GetComponent<Image>().sprite = levelPackBackgrounds[p % levelPackBackgrounds.Count];
            }

            for (int i = 0; i < 20; i++)
            {
                int index = (p * 20) + i;
                string levelNumber = (index + 1).ToString();
                GameObject box = Instantiate(levelBoxPrefab, pack.transform);
                box.name = "Level " + levelNumber;
                RectTransform rt = box.GetComponent<RectTransform>();

                Vector3 pos = Vector3.zero;
                pos.x = 65f + (i % 4) * 250f;
                pos.y = -200f - ((i - i % 4) / 4) * 300f;

                rt.anchoredPosition3D = pos;

                LevelBox b = box.GetComponent<LevelBox>();

                b.levelData = levels[index];
                b.levelData.gamePlaySettings.levelIndex = index;
                b.BackToMapsButton = BackToMapsButton;
                b.BackToMainMenuButton = BackToMainMenuButton;
                b.VehiclePanel = VehiclePanel;
                b.PackParent = PackParent.gameObject;
                b.objects.Popup = NeedsPopup;
                b.objects.PopupText = NeedsPopupText;

                LevelDataTransfer.SetLevelData(levels[index]);
                box.GetComponent<Button>().onClick.AddListener(() => box.GetComponent<LevelBox>().ClickEvent());

                levelBoxes.Add(b);
            }
            packs.Add(pack);
        }
        #region Add Random Level Pack As Last Position
        GameObject playRandom = Instantiate(PlayRandomPack, PackParent);
        Vector3 randPos = Vector3.zero;
        randPos.y = -200f - packSize * 1720f;
        playRandom.GetComponent<RectTransform>().anchoredPosition3D = randPos;
        playRandom.name = "Chapter Random";


        packs.Add(playRandom);
        #endregion

    }
    public void ReGenerateLevelBoxes()
    {
        foreach (var b in levelBoxes)
        {
            Destroy(b.gameObject);
        }
        foreach (var p in packs)
        {
            Destroy(p);
        }

        packs.Clear();
        levelBoxes.Clear();

        GenerateLevelBoxes();
    }
    public void LoadMap()
    {
        Blocker.SetActive(true);
        SoundController.instance.StopMenuMusic();
        SceneLoaderAsync.Instance.LoadSceneAsync("Map");
    }
    public void ClearLevelDataTransferObject()
    {
        LevelDataTransfer.gamePlaySettings = null;
        LevelDataTransfer.levelUI = null;
        LevelDataTransfer.mapSettings = null;
        LevelDataTransfer.planeEvent = null;
    }

    private void CreateLevels()
    {
        levels.Clear();
        int count = CSVReader.GetLineCount();

        for (int i = 0; i < count; i++)
        {
            CSVLine line = CSVReader.ReadLine(i);

            LevelData x = new LevelData();
            x.gamePlaySettings.levelIndex = i;
            x.gamePlaySettings.roadDistance = line.roadDistance;
            x.gamePlaySettings.EnemiesMaxSpawnTime = line.enemyMaxSpawnTime;
            x.gamePlaySettings.EnemiesMinSpawnDistance = line.enemyMinSpawn;
            x.gamePlaySettings.EnemiesMaxSpawnDistance = line.enemyMaxSpawn;

            if (line.itemType != BasketItem.None)
            {
                x.gamePlaySettings.basketEnable = true;
                x.gamePlaySettings.basketItem = line.itemType;
            }

            x.levelNeeds.itemType = line.needItem;
            x.levelNeeds.ItemSize = line.itemSize;
            x.levelNeeds.CoinSize = line.coinSize;

            x.planeEvent.EnablePlane = line.enablePlane;
            x.planeEvent.EnableAwardBox = line.enableAward;
            x.planeEvent.EnableTNT = line.enableTNT;
            x.planeEvent.EnableAwardPirateBomb = line.enablePirate;
            x.planeEvent.EnableAwardHomingMissile = line.enableMissile;
            x.planeEvent.EnableAwardCoin = line.enableCoin;
            x.planeEvent.EnableAwardProtector = line.enableProtector;
            x.planeEvent.EnableAwardBooster = line.enableBooster;

            switch (line.Unlock)
            {
                case "Pirate": x.levelAwards.PirateBomb = true; break;
                case "TNT": x.levelAwards.TNT = true; break;
                case "Basket": x.levelAwards.Basket = true; break;
                case "Coin": x.levelAwards.Coin = true; break;
                case "Missile": x.levelAwards.HomingMissile = true; break;
                case "Protector": x.levelAwards.Protector = true; break;
                case "Booster": x.levelAwards.Booster = true; break;
            }

            x.levelUI.showFPS = false;
            x.levelUI.levelTexture = sprites.levelUIList[UnityEngine.Random.Range(0, sprites.levelUIList.Count)];

            x.mapSettings.EnableRoadStrip = true;
            x.mapSettings.EnableSideStrip = true;

            RoadMaterial rm = roadMaterials[UnityEngine.Random.Range(0, roadMaterials.Count)];
            x.mapSettings.roadMaterial = rm.Road;
            x.mapSettings.sideAreaMaterial = rm.Side;

            if (rm.RoadType == RoadType.Desert)
            {
                x.mapSettings.foliageType = Foliage.Cactus;
                x.mapSettings.rockType = Rock.CactusArea;
                x.mapSettings.buildingType = Building.CactusArea;
                x.mapSettings.decalType = Decal.CactusArea;
            }
            else if (rm.RoadType == RoadType.Soil)
            {
                x.mapSettings.foliageType = Foliage.Pine;
                x.mapSettings.rockType = Rock.Sharp;
                x.mapSettings.buildingType = Building.CactusArea;
                x.mapSettings.decalType = Decal.CactusArea;
            }
            else if (rm.RoadType == RoadType.Grass)
            {
                x.mapSettings.foliageType = Foliage.Pine;
                x.mapSettings.rockType = Rock.Mossy;
                x.mapSettings.buildingType = Building.CactusArea;
                x.mapSettings.decalType = Decal.CactusArea;
            }



            levels.Add(x);
        }
    }

    [System.Serializable]
    public class Sprites
    {
        public Sprite BasketApple;
        public Sprite BasketOrange;
        public Sprite BasketPear;
        public Sprite Booster;
        public Sprite Missile;
        public Sprite PirateBomb;
        public Sprite Protector;
        public Sprite TNT;
        public Sprite Coin;
        public Sprite Basket;
        public List<Sprite> levelUIList = new List<Sprite>();
    }
    [System.Serializable]
    public class RoadMaterial
    {
        public RoadType RoadType;
        public Material Road;
        public Material Side;
    }
    public enum RoadType
    {
        Desert,
        Soil,
        Grass
    }
}



