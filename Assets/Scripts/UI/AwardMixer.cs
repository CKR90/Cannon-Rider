using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class AwardMixer : MonoBehaviour
{
    public static AwardMixer Instance;

    public AutoPilot Vehicle;
    public RectTransform SkillButtonsList_R;
    public RectTransform SkillButtonsList_L;
    public RectTransform Coin;
    public TextMeshProUGUI CoinText;
    public GameObject SkillButtonPrefab;
    
    [SerializeField]public List<AwardItem> Awards;

    private Image img;
    private bool runEnabled = false;

    private float shuffleTotalTime = 0f;
    private float shuffleTimer = 0f;
    private int shuffleIndex = 0;
    private int lastIndex = -1;

    [HideInInspector] public bool ProtectorEnabled = false;
    [HideInInspector] public bool BoosterEnabled = false;

    void Awake()
    {
        Instance = this;

        SetTransferData();

        img = GetComponent<Image>();
        img.enabled = false;

        foreach(var item in Awards)
        {
            item.Vehicle = Vehicle;
        }
    }
    void Update()
    {
        if(runEnabled) RunMixer();
    }
    private void SetTransferData()
    {
        Awards.Find(x => x.skillID == SkillIDs.PirateBomb && x.awardType == AwardType.Skill).Enable = LevelDataTransfer.planeEvent.EnableAwardPirateBomb;
        Awards.Find(x => x.skillID == SkillIDs.Missile && x.awardType == AwardType.Skill).Enable = LevelDataTransfer.planeEvent.EnableAwardHomingMissile;
        Awards.Find(x => x.helperID == HelperIDs.Protector && x.awardType == AwardType.Helper).Enable = LevelDataTransfer.planeEvent.EnableAwardProtector;
        Awards.Find(x => x.helperID == HelperIDs.Booster && x.awardType == AwardType.Helper).Enable = LevelDataTransfer.planeEvent.EnableAwardBooster;
        Awards.Find(x => x.awardType == AwardType.Coin).Enable = LevelDataTransfer.planeEvent.EnableAwardCoin;
    }
    private void MixerInitialize()
    {

        shuffleTimer = 0f;
        runEnabled = true;
        SoundController.instance.Play(SFXList.Box_Shuffle);
        img.sprite = Awards[0].sprite;
        shuffleIndex = 0;
        shuffleTotalTime = 0f;
        img.enabled = true;
    }
    public void RunMixer()
    {
        if (!runEnabled) MixerInitialize();

        shuffleTimer += Time.deltaTime;
        if(shuffleTimer >= .05f)
        {
            shuffleTimer = 0f;
            shuffleIndex = (shuffleIndex + 1) % Awards.Count;
            img.sprite = Awards[shuffleIndex].sprite;
        }

        shuffleTotalTime += Time.deltaTime;
        if(shuffleTotalTime >= 1.6f)
        {
            runEnabled = false;
            SelectAward();
        }
    }   
    private void SelectAward()
    {
        List<AwardItem> items = Awards.FindAll(x => x.Enable);
        if (items.Count == 0) return; 

        int index = Random.Range(0, items.Count);
        if (index == lastIndex) index = (index + 1) % items.Count;
        lastIndex = index;

        AwardItem s = items[index];
        SoundController.instance.Play(s.sound);
        img.enabled = false;
        InitializeAwardButton(s);
    }
    private void InitializeAwardButton(AwardItem s)
    {
        SkillButtonsList_R.GetComponent<SkillButtonListController>().BlockMovement = true;

        GameObject g = Instantiate(SkillButtonPrefab, transform);
        g.name = s.name;
        int awardSize = Random.Range(s.minAwardSize - 1, s.maxAwardSize) + 1;
        if (s.awardType == AwardType.Coin) awardSize *= 10;
        g.GetComponent<SkillButton>().data = s;

        g.GetComponent<Image>().sprite = s.sprite;

        if(s.awardType != AwardType.Helper) g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(awardSize.ToString());
        else g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(awardSize.ToString() + " sec");

        RectTransform rts = g.GetComponent<RectTransform>();
        rts.anchoredPosition3D = Vector3.zero;

        rts.DOScale(Vector3.one * 1.5f, 1f).SetEase(Ease.OutBounce).OnComplete(() => 
        {
            if (s.awardType == AwardType.Skill || s.awardType == AwardType.Helper) MoveSkillOrHelperToSkillList(rts, s, awardSize);
            else if (s.awardType == AwardType.Coin) MoveCoin(rts, awardSize);
        });

    }
    private void MoveSkillOrHelperToSkillList(RectTransform rts, AwardItem s, int awardSize)
    {
        rts.anchoredPosition3D = Vector3.zero;
        rts.anchorMax = new Vector2(1f, 0f);
        rts.anchorMin = new Vector2(1f, 0f);
        rts.pivot = new Vector2(1f, 0f);
        rts.anchoredPosition3D = new Vector3(50f, -50f, 0f);

        float size = SkillButtonsList_R.sizeDelta.x - 10f;
        if(s.awardList == AwardList.RightSide) rts.transform.SetParent(SkillButtonsList_R.transform);
        else rts.transform.SetParent(SkillButtonsList_L.transform);
        rts.transform.SetAsFirstSibling();
        
        rts.DOSizeDelta(new Vector2(size, size), .5f);

        int IDIndex = 0;
        if (s.awardType == AwardType.Skill) IDIndex = (int)s.skillID;
        else if (s.awardType == AwardType.Helper) IDIndex = (int)s.helperID;

        if (s.awardList == AwardList.RightSide) rts.DOAnchorPos3D(new Vector3(-5f, (5f + 10f * IDIndex) + (size * IDIndex), 0f), .5f);
        else rts.DOAnchorPos3D(new Vector3(5f, (5f + 10f * IDIndex) + (size * IDIndex), 0f), .5f);
        rts.DOScale(1f, .5f).OnComplete(() => AddOrBreakButton(rts, s, awardSize));
    }
    private void AddOrBreakButton(RectTransform rts, AwardItem s, int awardSize)
    {
        List<SkillButton> buttons = new List<SkillButton>();

        if(s.awardType == AwardType.Skill) buttons = SkillButtonsList_R.transform.GetComponentsInChildren<SkillButton>().ToList();
        else if (s.awardType == AwardType.Helper) buttons = SkillButtonsList_L.transform.GetComponentsInChildren<SkillButton>().ToList();
        buttons.Remove(rts.transform.GetComponent<SkillButton>());

        bool isHelper = false;
        if(s.awardType == AwardType.Helper) isHelper = true;


        if(buttons == null || buttons.Count == 0)
        {
            rts.transform.GetComponent<SkillButton>().AddSize(awardSize, isHelper);
            rts.transform.GetComponent<SkillButton>().buttonEnabled = true;
            return;
        }
        else
        {
            SkillButton b; 

            if(!isHelper) b = buttons.Find(x => x.data.skillID == s.skillID);
            else b = buttons.Find(x => x.data.helperID == s.helperID);

            if (b == null)
            {
                rts.transform.GetComponent<SkillButton>().AddSize(awardSize, isHelper);
                rts.transform.GetComponent<SkillButton>().buttonEnabled = true;
                return;
            }
            else
            {
                b.AddSize(awardSize, isHelper);
                Destroy(rts.gameObject);
            }
        }
    }
    private void MoveCoin(RectTransform rts, int awardSize)
    {
        rts.anchoredPosition3D = Vector3.zero;
        rts.anchorMax = new Vector2(0f, 1f);
        rts.anchorMin = new Vector2(0f, 1f);
        rts.pivot = new Vector2(0f, 1f);
        rts.anchoredPosition3D = new Vector3(-50f, 50f, 0f);

        float size = 80f;
        rts.transform.SetParent(Coin.transform);
        rts.transform.SetAsFirstSibling();

        rts.DOSizeDelta(new Vector2(size, size), .5f);
        rts.DOAnchorPos3D(Vector3.zero, .5f);
        rts.DOScale(1f, .5f).OnComplete(() => 
        {
            int coin = GameDatabase.Instance.IncreaseCoin(awardSize);
            CoinText.SetText(coin.ToString());
            Destroy(rts.gameObject);
        });
    }

    public void ResetAwardList()
    {

        List<SkillButton> buttons = SkillButtonsList_R.transform.GetComponentsInChildren<SkillButton>().ToList();

        foreach (SkillButton button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        buttons = SkillButtonsList_L.transform.GetComponentsInChildren<SkillButton>().ToList();
        foreach (SkillButton button in buttons)
        {
            Destroy(button.gameObject);
        }

    }

}







