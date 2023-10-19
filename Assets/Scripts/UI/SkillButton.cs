using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    [HideInInspector] public AwardItem data;
    [HideInInspector] public bool buttonEnabled = false;


    private TextMeshProUGUI tmp;
    private int size = 0;


    void Start()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ButtonEvent()
    {
        if (!buttonEnabled) return;

        if(data.awardType == AwardType.Skill)
        {
            if (size <= 0)
            {
                Destroy(gameObject);
                return;
            }

            size--;
            tmp.SetText(size.ToString());
        }



        if(data.awardType == AwardType.Skill)
        {
            if (data.skillID == SkillIDs.PirateBomb)
            {
                GameObject g = Instantiate(data.prefab);
                g.name = data.name;
                g.transform.position = data.Vehicle.transform.position + data.Vehicle.transform.forward;
                PirateBomb pb = g.GetComponent<PirateBomb>();
                pb.Direction = data.Vehicle.transform.forward;
                pb.Speed = data.speed;
            }
            else if (data.skillID == SkillIDs.Missile)
            {
                GameObject g = Instantiate(data.prefab);
                g.name = data.name;
                g.transform.position = data.Vehicle.transform.position + data.Vehicle.transform.forward;
                Missile pb = g.GetComponent<Missile>();
                g.transform.forward = data.Vehicle.transform.forward;
                pb.Speed = data.speed;
            }
        }
        else if(data.awardType == AwardType.Helper)
        {
            if (data.helperID == HelperIDs.Protector)
            {
                if (AwardMixer.Instance.ProtectorEnabled) return;

                GameObject g = Instantiate(data.prefab);
                g.name = data.name;
                g.transform.position = data.Vehicle.transform.position + data.Vehicle.transform.forward * 0.2f + Vector3.up * 0.1f;
                g.GetComponent<ProtectorShield>().Vehicle = data.Vehicle;
                g.GetComponent<ProtectorShield>().LifeTime = size;
                AwardMixer.Instance.ProtectorEnabled = true;
            }
            if (data.helperID == HelperIDs.Booster)
            {
                if (AwardMixer.Instance.BoosterEnabled) return;


                data.Vehicle.Booster.GetComponent<Booster>().Vehicle = data.Vehicle;
                data.Vehicle.Booster.GetComponent<Booster>().LifeTime = size;
                data.Vehicle.BoosterHelperEnabled = true;
                AwardMixer.Instance.BoosterEnabled = true;
            }
        }

        if (data.awardType == AwardType.Skill)
        {
            if (size <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        else if(data.awardType == AwardType.Helper)
        {
            Destroy(gameObject);
        }

    }

    public void AddSize(int size, bool isSec)
    {
        this.size += size;

        if(isSec) tmp.SetText(this.size.ToString() + " sec");
        else tmp.SetText(this.size.ToString());

    }
}
