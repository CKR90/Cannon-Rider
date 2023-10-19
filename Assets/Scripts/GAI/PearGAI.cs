using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PearGAI : MonoBehaviour, I_Enemy, I_Destroy
{
    public bool isIdiot = false;

    public string EnemyName = "Enemy";
    public SkinnedMeshRenderer sm;
    public List<ParticleSystem> ParticleSystems = new List<ParticleSystem>();

    private Vector3 CenterPos;
    private Transform Vehicle;
    private Canvas canvas;
    private float RoadLength = 12f;
    private bool isDestroyed = false;
    private bool blockUpdate = false;

    private float speed = 2f;
    private int particleIndex = 0;

    private Vector3 defaultScale = Vector3.one;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        EndLife();

        if (blockUpdate) return;

        if(!isIdiot) transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            DestroyMe();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DestroyMe();
        }
    }


    public string GetName()
    {
        return EnemyName;
    }
    public void Initialize()
    {
        transform.forward = CenterPos - transform.position;
        speed = Random.Range(2.5f, 3f);

        Ray r = new Ray(transform.position, -Vector3.up);

        if (Physics.Raycast(r, out RaycastHit hit, 100f))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }
        transform.DOScale(defaultScale, 0.9f);

        sm.SetBlendShapeWeight(1, 50f);
        sm.SetBlendShapeWeight(4, 75);
    }
    public void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Vehicle.GetComponent<AutoPilot>().BlockRollUntilLanding(transform, false);

        ParticleSystems[particleIndex].transform.SetParent(null);
        ParticleSystems[particleIndex].gameObject.SetActive(true);

        transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        SoundController.instance.Play(SFXList.Suction_Plop_2);
        Destroy(ParticleSystems[particleIndex].gameObject, 3f);
        Destroy(gameObject, .1f);
    }
    public bool IsBomb()
    {
        return false;
    }
    public void SetCenterPosition(Vector3 centerPosition)
    {
        CenterPos = centerPosition;
    }
    public void SetPlayer(Transform playerTransform)
    {
        Vehicle = playerTransform;
    }
    public void SetRoadLength(float length)
    {
        RoadLength = length;
    }
    public void EndLife()
    {
        if (transform.position.z < Vehicle.position.z - 30f) Destroy(gameObject);
        else if (Mathf.Abs(transform.position.x) > RoadLength / 2f) DestroyMe();
    }
    public void RunBombView()
    {
        SkinnedMeshRenderer smr = transform.GetComponentInChildren<SkinnedMeshRenderer>();

        Material m = new Material(smr.sharedMaterial);
        smr.sharedMaterial = m;
        smr.sharedMaterial.DOColor(new Color(0.7f, 0f, 0f, 1f), 2f);
        Vector3 scale = transform.localScale;
        transform.DOScale(scale * 1.5f, 2f);
        particleIndex = 1;
    }

    public void SetParentUICanvas(Canvas parent)
    {
        canvas = parent;
    }
    public void SetDefaultScale(Vector3 scale)
    {
        defaultScale = scale;
    }

    public void KillWithGun(int coin, GunType gunType)
    {
        DailyMissionDatabase.AddDailyData(DailyItem.Pear, gunType, 1);
        GameUICoinAdder.Instance.AddCoin(canvas, transform.position + Vector3.up, coin);
        GameDatabase.Instance.AddDeadEnemy(EnemyName);
        DestroyMe();
    }
}
