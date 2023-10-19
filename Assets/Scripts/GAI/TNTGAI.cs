using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTGAI : MonoBehaviour, I_Destroy
{
    public Transform Vehicle;
    public GameObject ExplosionArea;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;


    [HideInInspector] public List<GameObject> inArea = new List<GameObject>();
    private float timer = 4f;
    private int oldTimeSetValue = 0;
    private bool playerDetect = false;
    private GameObject _ExplosionArea;

    [HideInInspector] public bool isTestTNT = false;

    void Update()
    {
        if(!isTestTNT)
        {
            spriteRenderer.transform.forward = Camera.main.transform.forward;
            timer -= Time.deltaTime;

            if (timer < 0f) Explode();
            else if (timer < 1f) TimeSet(0);
            else if (timer < 2f) TimeSet(1);
            else if (timer < 3f) TimeSet(2);
            else if (timer < 4f) TimeSet(3);
        }
        else if(spriteRenderer != null)
        {
            Destroy(spriteRenderer.gameObject);
        }

        
    }

    public void Explode(bool withBullet = false)
    {
        ParticleSystem p = transform.GetComponentInChildren<ParticleSystem>();

        if(p != null)
        {
            p.transform.SetParent(null);
            p.transform.GetComponent<AudioSource>().Play();
            p.Play();
        }


        float rollForce = 30f;
        if (Vehicle.transform.position.x > transform.position.x) rollForce *= -1f;

        Vehicle.GetComponent<AutoPilot>().BlockRollUntilLanding(transform, false);
        Vehicle.GetComponent<AutoPilot>().BombEffect(transform, 1000f, rollForce, false);

        if(Vector3.Distance(transform.position, Vehicle.position) < 3f)
        {
            CanvasExplosionEffect.instance.Play();
        }

        if(withBullet)
        {
            foreach (var enemy in inArea)
            {
                if (enemy != null)
                {
                    if (withBullet) enemy.GetComponent<I_Destroy>().KillWithGun(40, GunType.TNT);
                    else enemy.GetComponent<I_Enemy>().DestroyMe();
                }
            }
        }
        else
        {
            foreach (var enemy in inArea)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<I_Enemy>().DestroyMe();
                }
            }
        }

        

        if(p != null) Destroy(p.gameObject, 3f);
        Destroy(gameObject);
        Destroy(_ExplosionArea);
    }
    private void TimeSet(int t)
    {
        if (oldTimeSetValue == t) return;
        
        oldTimeSetValue = t;

        spriteRenderer.sprite = sprites[t];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!playerDetect && collision.collider.gameObject.tag == "Player")
        {
            playerDetect = true;
            Explode();
        }


        if (collision.collider.gameObject.tag != "Road") return;

        Destroy(GetComponent<Rigidbody>());
        SoundController.instance.Play(SFXList.Window_Hit_2);
        GetComponent<AudioSource>().Play();

        _ExplosionArea = Instantiate(ExplosionArea, transform.position, Quaternion.identity);
        _ExplosionArea.name = "TNT Explosion Area";
        _ExplosionArea.GetComponent<TNTExplosionArea>().tnt = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!playerDetect && other.gameObject.tag == "Player")
        {
            playerDetect = true;
            Explode();
        }
    }
    public void KillWithGun(int coin, GunType gunType)
    {
        Explode(true);
    }
}
