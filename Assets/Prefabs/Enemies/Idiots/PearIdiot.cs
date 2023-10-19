using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearIdiot : MonoBehaviour, I_Destroy, I_Enemy
{
    public ParticleSystem ps;


    [HideInInspector] public Canvas canvas;
    [HideInInspector] public GameObject arrow;

    private bool isDestroyed = false;

    public void EndLife()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public bool IsBomb()
    {
        throw new System.NotImplementedException();
    }

    public void KillWithGun(int coin, GunType gunType)
    {
        DailyMissionDatabase.AddDailyData(DailyItem.Pear, gunType, 1);
        GameUICoinAdder.Instance.AddCoin(canvas, transform.position + Vector3.up, coin);
        GameDatabase.Instance.AddDeadEnemy("Pear");
        DestroyMe();
    }

    public void RunBombView()
    {
        throw new System.NotImplementedException();
    }

    public void SetCenterPosition(Vector3 centerPosition)
    {
        throw new System.NotImplementedException();
    }

    public void SetDefaultScale(Vector3 scale)
    {
        throw new System.NotImplementedException();
    }

    public void SetParentUICanvas(Canvas parent)
    {
        throw new System.NotImplementedException();
    }

    public void SetPlayer(Transform playerTransform)
    {
        throw new System.NotImplementedException();
    }

    public void SetRoadLength(float length)
    {
        throw new System.NotImplementedException();
    }

    private void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Destroy(arrow);

        ps.transform.SetParent(null);
        ps.gameObject.SetActive(true);

        transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        SoundController.instance.Play(SFXList.Suction_Plop_2);
        Destroy(ps.gameObject, 3f);
        Destroy(gameObject, .1f);
    }

    void I_Enemy.DestroyMe()
    {
        DestroyMe();
    }
}
