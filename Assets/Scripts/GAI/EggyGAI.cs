using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EggyGAI : MonoBehaviour, I_Enemy, I_Destroy
{
    public string EnemyName = "Enemy";
    public List<ParticleSystem> ParticleSystems = new List<ParticleSystem>();

    private Vector3 CenterPos;
    private Transform Vehicle;
    private Canvas canvas;
    private float RoadLength = 12f;
    private bool isDestroyed = false;
    private bool blockUpdate = false;

    private float speed = 2f;
    private bool isBomb = false;

    private bool ColliderDetect = false;

    private Vector3 defaultScale = Vector3.one;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        EndLife();

        if (blockUpdate) return;
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            ColliderDetect = true;
            DestroyMe();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ColliderDetect = true;
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
        speed = Random.Range(1f, 4f);

        Ray r = new Ray(transform.position, -Vector3.up);

        if (Physics.Raycast(r, out RaycastHit hit, 100f))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }
        transform.DOScale(defaultScale, 0.9f);

        if (Random.Range(0, 4) == 0)
        {
            isBomb = true;
            float time = Random.Range(2f, 5f);
            Invoke("DestroyMe", time);
            Invoke("RunBombView", 1f);
        }
    }

    public bool IsBomb()
    {
        return isBomb;
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
    public void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        ParticleSystems[0].transform.SetParent(null);
        ParticleSystems[0].gameObject.SetActive(true);

        if (isBomb)
        {
            for (int i = 1; i < ParticleSystems.Count; i++)
            {
                ParticleSystems[i].transform.SetParent(null);
                ParticleSystems[i].gameObject.SetActive(true);
                Destroy(ParticleSystems[i].gameObject, 3f);
                Vehicle.gameObject.GetComponent<AutoPilot>().BombEffect(transform);
            }
            if(!ColliderDetect) Vehicle.GetComponent<AutoPilot>().BlockRollUntilLanding(transform);
            SoundController.instance.Play(SFXList.Explosion_Body_Grenade_2);
        }
        else if(ColliderDetect)
        {
            SoundController.instance.Play(SFXList.Window_Hit_2);
            
        }
        else
        {
            SoundController.instance.Play(SFXList.Head_Explode);
        }
        Destroy(ParticleSystems[0].gameObject, 3f);
        Destroy(gameObject);
        
    }
    public void RunBombView()
    {
        SkinnedMeshRenderer smr = transform.GetComponentInChildren<SkinnedMeshRenderer>();

        Material m = new Material(smr.sharedMaterial);
        smr.sharedMaterial = m;
        smr.sharedMaterial.DOColor(new Color(0.7f, 0f, 0f, 1f), 2f);
        Vector3 scale = transform.localScale;
        transform.DOScale(scale * 1.5f, 2f);
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
        DailyMissionDatabase.AddDailyData(DailyItem.Eggy, gunType, 1);
        GameUICoinAdder.Instance.AddCoin(canvas, transform.position + Vector3.up, coin);
        GameDatabase.Instance.AddDeadEnemy(EnemyName);
        DestroyMe();
    }
}
