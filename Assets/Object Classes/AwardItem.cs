using UnityEngine;

[System.Serializable]
public class AwardItem
{
    public string name;
    public bool Enable;
    public AwardType awardType;
    public AwardList awardList;
    public SkillIDs skillID;
    public HelperIDs helperID;
    public SFXList sound;
    public Sprite sprite;
    public GameObject prefab;
    public int minAwardSize;
    public int maxAwardSize;
    public float speed;
    public float positionForwardOffset;
    [HideInInspector] public AutoPilot Vehicle;
}
