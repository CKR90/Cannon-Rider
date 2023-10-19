using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyManager : MonoBehaviour
{
    public static DailyManager Instance;

    public RectTransform BG;
    [SerializeField] private Sprites sprites;

    public TextMeshProUGUI RemainingTimeText;
    public RectTransform DescriptionPanel;
    public List<DailyBox> boxes = new List<DailyBox>();
    public DescriptionObjects descriptionObjects;
    private int MissionIndexLimit = 0;
    private int ExplodeWithIndexLimit = 0;

    private DailyData dailyData;

    private float timer = 0f;

    private bool DescriptionAnimPause = false;
    private Animator bombAnim;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        bombAnim = descriptionObjects.withBomb.GetComponent<Animator>();

        dailyData = DailyMissionDatabase.LoadDaily();

        CheckDailyTime();

        if (dailyData.dailyMission[0] == DailyMission.None)
        {
            GenerateNewMissions();
        }
        else
        {
            GetLoadedMissions();
            CheckCompleted();
            WriteRemainingTime();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GenerateNewMissions();
        }

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            CheckDailyTime();

            timer -= 1f;
            WriteRemainingTime();
        }
        if(bombAnim.GetBool("Pause") != DescriptionAnimPause) bombAnim.SetBool("Pause", DescriptionAnimPause);


    }

    private void CheckDailyTime()
    {
        int seconds = ClockManager.GetTimeDifferenceInSeconds(dailyData.endTime);

        if (seconds <= 0)
        {
            while (seconds <= 0)
            {
                seconds += 86400;
            }

            DailyMissionDatabase.ResetDaily(DateTime.Now.AddSeconds(seconds).ToString());
            dailyData = DailyMissionDatabase.LoadDaily();
            GenerateNewMissions();
        }
    }
    private void WriteRemainingTime()
    {
        RemainingTimeText.SetText(ClockManager.GetRemainingTime(dailyData.endTime));
    }
    public void GenerateNewMissions()
    {
        int seconds = ClockManager.GetTimeDifferenceInSeconds(dailyData.endTime);
        ClearBoxDatas();
        CalculateLimits();

        for (int i = 0; i < boxes.Count; i++)
        {
            DailyBox box = boxes[i];
            box.Completed.SetActive(false);
            box.Mission = GetRandomMission();
            box.ExplodeWith = GetRandomExplosion(box.Mission);
            ApplySprite(box.MissionImage, box.Mission);
            ApplySprite(box.ExplodeWithImage, box.ExplodeWith);
            SetSizeAndDescription(box);

            DailyMissionDatabase.UpdateDailyMission(i, box);
        }
        DailyMissionDatabase.ApplyDaily();
        dailyData = DailyMissionDatabase.LoadDaily();
    }
    private void ClearBoxDatas()
    {
        foreach (var b in boxes)
        {
            b.isCompleted = false;
            b.isAwardPaid = false;
            b.Mission = DailyMission.None;
            b.ExplodeWith = DailyExplodeWith.None;
            b.CurrentCount = 0;
            b.CountLimit = 0;
            b.CurrentSize = 0;
            b.SizeLimit = 0;
        }
    }
    private void GetLoadedMissions()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            DailyBox box = boxes[i];
            box.Completed.SetActive(false);
            box.Mission = dailyData.dailyMission[i];
            box.ExplodeWith = dailyData.dailyExplodeWith[i];
            box.CountLimit = dailyData.CountLimit[i];
            box.SizeLimit = dailyData.SizeLimit[i];
            box.isAwardPaid = dailyData.isAwardPaid[i];

            if (dailyData.dailyMission[i] == DailyMission.ExplodeBanana)
            {
                switch(dailyData.dailyExplodeWith[i])
                {
                    case DailyExplodeWith.None: box.CurrentCount = dailyData.ExplodedBananaWithAny; break;
                    case DailyExplodeWith.PirateBomb: box.CurrentCount = dailyData.ExplodedBananaWithPirate; break;
                    case DailyExplodeWith.TNT: box.CurrentCount = dailyData.ExplodedBananaWithTNT; break;
                    case DailyExplodeWith.HomingMissile: box.CurrentCount = dailyData.ExplodedBananaWithMissile; break;
                }
            }
            else if (dailyData.dailyMission[i] == DailyMission.ExplodePear)
            {
                switch (dailyData.dailyExplodeWith[i])
                {
                    case DailyExplodeWith.None: box.CurrentCount = dailyData.ExplodedPearWithAny; break;
                    case DailyExplodeWith.PirateBomb: box.CurrentCount = dailyData.ExplodedPearWithPirate; break;
                    case DailyExplodeWith.TNT: box.CurrentCount = dailyData.ExplodedPearWithTNT; break;
                    case DailyExplodeWith.HomingMissile: box.CurrentCount = dailyData.ExplodedPearWithMissile; break;
                }
            }
            else if (dailyData.dailyMission[i] == DailyMission.ExplodeOrange)
            {
                switch (dailyData.dailyExplodeWith[i])
                {
                    case DailyExplodeWith.None: box.CurrentCount = dailyData.ExplodedOrangeWithAny; break;
                    case DailyExplodeWith.PirateBomb: box.CurrentCount = dailyData.ExplodedOrangeWithPirate; break;
                    case DailyExplodeWith.TNT: box.CurrentCount = dailyData.ExplodedOrangeWithTNT; break;
                    case DailyExplodeWith.HomingMissile: box.CurrentCount = dailyData.ExplodedOrangeWithMissile; break;
                }
            }
            else if (dailyData.dailyMission[i] == DailyMission.ExplodeEggy)
            {
                switch (dailyData.dailyExplodeWith[i])
                {
                    case DailyExplodeWith.None: box.CurrentCount = dailyData.ExplodedEggyWithAny; break;
                    case DailyExplodeWith.PirateBomb: box.CurrentCount = dailyData.ExplodedEggyWithPirate; break;
                    case DailyExplodeWith.TNT: box.CurrentCount = dailyData.ExplodedEggyWithTNT; break;
                    case DailyExplodeWith.HomingMissile: box.CurrentCount = dailyData.ExplodedEggyWithMissile; break;
                }
            }
            else if (dailyData.dailyMission[i] == DailyMission.GoAnyMeters)
            {
                box.CurrentSize = dailyData.CoveredRoad;
            }
            else if (dailyData.dailyMission[i] == DailyMission.CompleteAnyLevels)
            {
                box.CurrentSize = dailyData.CompletedLevel;
            }
            else if (dailyData.dailyMission[i] == DailyMission.EarnAnyCoin)
            {
                box.CurrentSize = dailyData.EarnedCoin;
            }
            else if (dailyData.dailyMission[i] == DailyMission.EarnBasketApple)
            {
                box.CurrentSize = dailyData.EarnedApple;
            }
            else if (dailyData.dailyMission[i] == DailyMission.EarnBasketPear)
            {
                box.CurrentSize = dailyData.EarnedPear;
            }
            else if (dailyData.dailyMission[i] == DailyMission.EarnBasketOrange)
            {
                box.CurrentSize = dailyData.EarnedOrange;
            }


            ApplySprite(box.MissionImage, box.Mission);
            ApplySprite(box.ExplodeWithImage, box.ExplodeWith);
            ApplyDailyDataToScreen(i, box);
        }
    }
    private void CheckCompleted()
    {
        foreach(var box in boxes)
        {
            if(box.isAwardPaid)
            {
                box.Paid.SetActive(true);
                box.Completed.SetActive(false);
                box.Description.enabled = false;
                box.RemainingCount.enabled = false;
                box.RemainingSize.enabled = false;
            }
            else if((box.CountLimit > 0 && box.CurrentCount >= box.CountLimit) || (box.SizeLimit > 0 && box.CurrentSize >= box.SizeLimit))
            {
                box.isCompleted = true;
                box.Completed.SetActive(true);
                box.Paid.SetActive(false);
                box.Description.enabled = true;
                box.RemainingCount.enabled = true;
                box.RemainingSize.enabled = true;
            }
            else
            {
                box.Paid.SetActive(false);
                box.Completed.SetActive(false);
                box.Description.enabled = true;
                box.RemainingCount.enabled = true;
                box.RemainingSize.enabled = true;
            }
        }
    }
    private DailyMission GetRandomMission()
    {
        DailyMission mission = (DailyMission)UnityEngine.Random.Range(0, MissionIndexLimit);

        for (int t = 0; t < 5; t++)
        {
            if (!MissionExisting(mission))
            {
                return mission;
            }
            mission = (DailyMission)(((int)mission + 1) % MissionIndexLimit);
        }
        return mission;
    }
    private DailyExplodeWith GetRandomExplosion(DailyMission mission)
    {
        if ((int)mission < 4)
        {
            return (DailyExplodeWith)UnityEngine.Random.Range(0, ExplodeWithIndexLimit);
        }
        else
        {
            return DailyExplodeWith.None;
        }
    }
    private void CalculateLimits()
    {
        int missileIndex = LevelDataGenerator.Instance.levels.Find(x => x.levelAwards.HomingMissile).gamePlaySettings.levelIndex;
        if (User.info.lastcompletedlevelindex > missileIndex)
        {
            ExplodeWithIndexLimit = (int)DailyExplodeWith.HomingMissile + 1;
        }
        else
        {
            int tntIndex = LevelDataGenerator.Instance.levels.Find(x => x.levelAwards.TNT).gamePlaySettings.levelIndex;
            if (User.info.lastcompletedlevelindex > tntIndex)
            {
                ExplodeWithIndexLimit = (int)DailyExplodeWith.TNT + 1;
            }
            else
            {
                int pirateIndex = LevelDataGenerator.Instance.levels.Find(x => x.levelAwards.PirateBomb).gamePlaySettings.levelIndex;
                if (User.info.lastcompletedlevelindex > pirateIndex)
                {
                    ExplodeWithIndexLimit = (int)DailyExplodeWith.PirateBomb + 1;
                }
                else
                {
                    ExplodeWithIndexLimit = (int)DailyExplodeWith.None;
                }
            }
        }


        int basketIndex = LevelDataGenerator.Instance.levels.Find(x => x.levelAwards.Basket).gamePlaySettings.levelIndex;
        if (User.info.lastcompletedlevelindex > basketIndex)
        {
            MissionIndexLimit = (int)DailyMission.EarnBasketOrange + 1;
        }
        else
        {
            MissionIndexLimit = (int)DailyMission.EarnAnyCoin + 1;
        }

    }
    private bool MissionExisting(DailyMission mission)
    {
        foreach (DailyBox box in boxes)
        {
            if (box.Mission == mission) return true;
        }
        return false;
    }

    private void ApplySprite(Image image, DailyMission dailyMission)
    {
        switch(dailyMission)
        {
            case DailyMission.ExplodeBanana: image.sprite = sprites.Banana; break;
            case DailyMission.ExplodePear: image.sprite = sprites.Pear; break;
            case DailyMission.ExplodeEggy: image.sprite = sprites.Eggy; break;
            case DailyMission.ExplodeOrange: image.sprite = sprites.Orange; break;
            case DailyMission.GoAnyMeters: image.sprite = sprites.Road; break;
            case DailyMission.CompleteAnyLevels: image.sprite = sprites.Levels; break;
            case DailyMission.EarnAnyCoin: image.sprite = sprites.Coin; break;
            case DailyMission.EarnBasketApple: image.sprite = sprites.BasketApple; break;
            case DailyMission.EarnBasketPear: image.sprite = sprites.BasketPear; break;
            case DailyMission.EarnBasketOrange: image.sprite = sprites.BasketOrange; break;
        }
    }
    private void ApplySprite(Image image, DailyExplodeWith dailyExplodeWith)
    {
        image.color = new Color(1f, 1f, 1f, 1f);
        switch (dailyExplodeWith)
        {
            case DailyExplodeWith.PirateBomb: image.sprite = sprites.Pirate; break;
            case DailyExplodeWith.TNT: image.sprite = sprites.TNT; break;
            case DailyExplodeWith.HomingMissile: image.sprite = sprites.Missile; break;
            default: image.color = new Color(1f, 1f, 1f, 0f); break;
        }
    }
    private void SetSizeAndDescription(DailyBox box)
    {
        box.CurrentSize = 0;
        box.CurrentCount = 0;
        box.CountLimit = 0;
        box.SizeLimit = 0;

        box.RemainingCount.SetText("");
        box.RemainingSize.SetText("");

        if((int)box.Mission < 4)
        {
            if(box.ExplodeWith == DailyExplodeWith.None)
            {
                box.CountLimit = UnityEngine.Random.Range(4, 15) * 5;
                box.Description.SetText("Explode");
            }
            else
            {
                box.CountLimit = UnityEngine.Random.Range(2, 6);

                switch(box.ExplodeWith)
                {
                    case DailyExplodeWith.PirateBomb: box.Description.SetText("Explode with Pirate Bomb"); break;
                    case DailyExplodeWith.TNT: box.Description.SetText("Explode with TNT"); break;
                    case DailyExplodeWith.HomingMissile: box.Description.SetText("Explode with Homing Missile"); break;
                    default: box.Description.SetText("Explode"); break;
                }
            }
            box.RemainingCount.SetText("0/" + box.CountLimit);
        }
        else if(box.Mission == DailyMission.GoAnyMeters)
        {
            box.SizeLimit = UnityEngine.Random.Range(80, 161) * 50;
            box.RemainingSize.SetText("0/" + box.SizeLimit + "m");
            box.Description.SetText("Cover " + box.SizeLimit + " meters");
        }
        else if (box.Mission == DailyMission.CompleteAnyLevels)
        {
            box.SizeLimit = UnityEngine.Random.Range(4, 9);
            box.RemainingSize.SetText("0/" + box.SizeLimit);
            box.Description.SetText("Complete " + box.SizeLimit + " levels");
        }
        else if (box.Mission == DailyMission.EarnAnyCoin)
        {
            box.SizeLimit = UnityEngine.Random.Range(20, 61) * 100;
            box.RemainingSize.SetText("0/" + box.SizeLimit);
            box.Description.SetText("Earn " + box.SizeLimit + " coins");
        }
        else
        {
            box.SizeLimit = UnityEngine.Random.Range(15, 31) * 10;
            box.RemainingSize.SetText("0/" + box.SizeLimit);

            switch(box.Mission)
            {
                case DailyMission.EarnBasketApple: box.Description.SetText("Earn " + box.SizeLimit + " Apples"); break;
                case DailyMission.EarnBasketOrange: box.Description.SetText("Earn " + box.SizeLimit + " Oranges"); break;
                case DailyMission.EarnBasketPear: box.Description.SetText("Earn " + box.SizeLimit + " Pears"); break;
            }
        }
    }
    private void ApplyDailyDataToScreen(int index, DailyBox box)
    {
        box.RemainingCount.SetText("");
        box.RemainingSize.SetText("");

        if ((int)box.Mission < 4)
        {
            if (box.ExplodeWith == DailyExplodeWith.None)
            {
                box.Description.SetText("Explode");
            }
            else
            {
                switch (box.ExplodeWith)
                {
                    case DailyExplodeWith.PirateBomb: box.Description.SetText("Explode with Pirate Bomb"); break;
                    case DailyExplodeWith.TNT: box.Description.SetText("Explode with TNT"); break;
                    case DailyExplodeWith.HomingMissile: box.Description.SetText("Explode with Homing Missile"); break;
                    default: box.Description.SetText("Explode"); break;
                }
            }
            box.RemainingCount.SetText(box.CurrentCount + "/" + box.CountLimit);
        }
        else if (box.Mission == DailyMission.GoAnyMeters)
        {
            box.RemainingSize.SetText(box.CurrentSize + "/" + box.SizeLimit + "m");
            box.Description.SetText("Cover " + box.SizeLimit + " meters");
        }
        else if (box.Mission == DailyMission.CompleteAnyLevels)
        {
            box.RemainingSize.SetText(box.CurrentSize + "/" + box.SizeLimit);
            box.Description.SetText("Complete " + box.SizeLimit + " levels");
        }
        else if (box.Mission == DailyMission.EarnAnyCoin)
        {
            box.RemainingSize.SetText(box.CurrentSize + "/" + box.SizeLimit);
            box.Description.SetText("Earn " + box.SizeLimit + " coins");
        }
        else
        {
            box.RemainingSize.SetText(box.CurrentSize + "/" + box.SizeLimit);

            switch (box.Mission)
            {
                case DailyMission.EarnBasketApple: box.Description.SetText("Earn " + box.SizeLimit + " Apples"); break;
                case DailyMission.EarnBasketOrange: box.Description.SetText("Earn " + box.SizeLimit + " Oranges"); break;
                case DailyMission.EarnBasketPear: box.Description.SetText("Earn " + box.SizeLimit + " Pears"); break;
            }
        }
    }

    public void ClickEvent(int index)
    {
        if (boxes[index].isAwardPaid)
        {
            SoundController.instance.Play(SFXList.ButtonClick);
            return;
        }

        if (!boxes[index].isCompleted) OpenDescriptionPanel(boxes[index]);
        else AwardPanelEvent(boxes[index], index);
    }
    private void AwardPanelEvent(DailyBox box, int index)
    {
        box.isAwardPaid = true;
        DailyMissionDatabase.UpdateDailyMission(index, box);
        DailyMissionDatabase.ApplyDaily();
        DailyAwardApply.Instance.PayDailyAwardAndUpdateDatabases();
        CheckCompleted();
    }
    private void OpenDescriptionPanel(DailyBox box)
    {
        DOTween.KillAll();
        ApplyDescriptionData(box);
        SoundController.instance.Play(SFXList.ButtonClick);
        DescriptionPanel.localScale = Vector3.zero;
        DescriptionPanel.gameObject.SetActive(true);
        DescriptionPanel.DOScale(1f, .3f);
    }
    public void CloseDescriptionPanel()
    {
        DOTween.KillAll();
        SoundController.instance.Play(SFXList.ButtonClick);
        DescriptionPanel.localScale = Vector3.one;
        DescriptionPanel.DOScale(0f, .3f).OnComplete(() => DescriptionPanel.gameObject.SetActive(false));
    }

    private void ApplyDescriptionData(DailyBox box)
    {
        #region Count_Size

        Vector2 anchor = descriptionObjects.CompleteBar.sizeDelta;

        if (box.CountLimit > 0)
        {
            descriptionObjects.Size_Count.SetText(box.CurrentCount + "/" + box.CountLimit);
            anchor.x = 350f * box.CurrentCount / box.CountLimit;
        }
        else
        {
            descriptionObjects.Size_Count.SetText(box.CurrentSize + "/" + box.SizeLimit);
            anchor.x = 350f * box.CurrentSize / box.SizeLimit;
        }

        descriptionObjects.CompleteBar.sizeDelta = anchor;

        if (box.Mission == DailyMission.GoAnyMeters) descriptionObjects.Size_Count.SetText(descriptionObjects.Size_Count.text + "m");
        #endregion
        #region Description Text
        string desc = "";
        switch (box.Mission)
        {
            case DailyMission.ExplodeBanana:     desc = "Explode "  + box.CountLimit + " bananas"; break;
            case DailyMission.ExplodePear:       desc = "Explode "  + box.CountLimit + " pears";   break;
            case DailyMission.ExplodeEggy:       desc = "Explode "  + box.CountLimit + " eggies";  break;
            case DailyMission.ExplodeOrange:     desc = "Explode "  + box.CountLimit + " oranges"; break;
            case DailyMission.GoAnyMeters:       desc = "Cover "    + box.SizeLimit  + " meters";  break;
            case DailyMission.CompleteAnyLevels: desc = "Complete " + box.SizeLimit  + " levels";  break;
            case DailyMission.EarnAnyCoin:       desc = "Earn "     + box.SizeLimit  + " coins";   break;
            case DailyMission.EarnBasketApple:   desc = "Earn "     + box.SizeLimit  + " apples";  break;
            case DailyMission.EarnBasketPear:    desc = "Earn "     + box.SizeLimit  + " pears";   break;
            case DailyMission.EarnBasketOrange:  desc = "Earn "     + box.SizeLimit  + " oranges"; break;
        }

        

        if ((int)box.Mission < 4 && box.ExplodeWith != DailyExplodeWith.None)
        {
            switch(box.ExplodeWith)
            {
                case DailyExplodeWith.PirateBomb:    desc += " with pirate bomb.";    break;
                case DailyExplodeWith.TNT:           desc += " with TNT.";            break;
                case DailyExplodeWith.HomingMissile: desc += " with homing missile."; break;
            }
        }

        descriptionObjects.Description.SetText(desc);
        #endregion
        #region Bomb Image
        if ((int)box.Mission < 4 && box.ExplodeWith != DailyExplodeWith.None)
        {
            switch(box.ExplodeWith)
            {
                case DailyExplodeWith.PirateBomb:    
                    descriptionObjects.BombImage.sprite = sprites.Pirate;
                    descriptionObjects.TNTWave.SetActive(false);
                    DescriptionAnimPause = false;
                    break;
                case DailyExplodeWith.TNT:           
                    descriptionObjects.BombImage.sprite = sprites.TNT;
                    descriptionObjects.TNTWave.SetActive(true);
                    DescriptionAnimPause = true;
                    break;
                case DailyExplodeWith.HomingMissile: 
                    descriptionObjects.BombImage.sprite = sprites.Missile_Horizontal;
                    descriptionObjects.TNTWave.SetActive(false);
                    DescriptionAnimPause = false;
                    break;
            }

            descriptionObjects.withBomb.SetActive(true);
        }
        else
        {
            descriptionObjects.withBomb.SetActive(false);
            descriptionObjects.TNTWave.SetActive(false);
        }
        #endregion
        #region Event_Enemy Image
        if ((int)box.Mission < 4)
        {
            switch (box.Mission)
            {
                case DailyMission.ExplodeBanana: descriptionObjects.EnemyImage.sprite = sprites.Banana; break;
                case DailyMission.ExplodePear:   descriptionObjects.EnemyImage.sprite = sprites.Pear;   break;
                case DailyMission.ExplodeEggy:   descriptionObjects.EnemyImage.sprite = sprites.Eggy;   break;
                case DailyMission.ExplodeOrange: descriptionObjects.EnemyImage.sprite = sprites.Orange; break;
            }

            descriptionObjects.EventImage.gameObject.SetActive(false);
            descriptionObjects.EnemyImage.gameObject.SetActive(true);
        }
        else
        {
            switch(box.Mission)
            {
                case DailyMission.GoAnyMeters:       descriptionObjects.EventImage.sprite = sprites.Road;         break;
                case DailyMission.CompleteAnyLevels: descriptionObjects.EventImage.sprite = sprites.Levels;       break;
                case DailyMission.EarnAnyCoin:       descriptionObjects.EventImage.sprite = sprites.Coin;         break;
                case DailyMission.EarnBasketApple:   descriptionObjects.EventImage.sprite = sprites.BasketApple;  break;
                case DailyMission.EarnBasketPear:    descriptionObjects.EventImage.sprite = sprites.BasketPear;   break;
                case DailyMission.EarnBasketOrange:  descriptionObjects.EventImage.sprite = sprites.BasketOrange; break;
            }
            descriptionObjects.EventImage.gameObject.SetActive(true);
            descriptionObjects.EnemyImage.gameObject.SetActive(false);
        }
        #endregion
    }


    [System.Serializable] public class Sprites
    {
        public Sprite ExplodeBG;
        public Sprite Banana;
        public Sprite Pear;
        public Sprite Orange;
        public Sprite Eggy;
        public Sprite Cannon;
        public Sprite TNT;
        public Sprite Pirate;
        public Sprite Missile;
        public Sprite Missile_Horizontal;
        public Sprite Road;
        public Sprite Levels;
        public Sprite Coin;
        public Sprite BasketApple;
        public Sprite BasketPear;
        public Sprite BasketOrange;
    }
    [System.Serializable] public class DescriptionObjects
    {
        public GameObject withBomb;
        public GameObject TNTWave;
        public RectTransform CompleteBar;
        public Image BombImage;
        public Image EnemyImage;
        public Image EventImage;
        public TextMeshProUGUI Size_Count;
        public TextMeshProUGUI Description;
    }
}

[System.Serializable]
public class DailyBox
{
    public Image MissionImage;
    public Image ExplodeWithImage;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI RemainingCount;
    public TextMeshProUGUI RemainingSize;
    public GameObject Completed;
    public GameObject Paid;
    [HideInInspector] public bool isCompleted = false;
    [HideInInspector] public bool isAwardPaid = false;
    [HideInInspector] public DailyMission Mission;
    [HideInInspector] public DailyExplodeWith ExplodeWith;
    [HideInInspector] public int CurrentCount = 0;
    [HideInInspector] public int CountLimit = 0;
    [HideInInspector] public int CurrentSize = 0;
    [HideInInspector] public int SizeLimit = 0;
}

public enum DailyMission
{
    ExplodeBanana,
    ExplodePear,
    ExplodeEggy,
    ExplodeOrange,
    GoAnyMeters,
    CompleteAnyLevels,
    EarnAnyCoin,
    EarnBasketApple,
    EarnBasketPear,
    EarnBasketOrange,
    None
}
public enum DailyExplodeWith
{
    None,
    PirateBomb,
    TNT,
    HomingMissile,
    
}
