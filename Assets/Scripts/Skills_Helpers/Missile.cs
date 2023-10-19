using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject targetSpriteRenderer;
    [HideInInspector] public float Speed;


    private Transform target;
    private bool destroyDetected = false;

    private float beepTime = .15f;
    private float beepTimer = 0f;

    private float timer = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("DestroyEvent", 3f);
        FindTarget();
        InitializeCrosshair();
    }

    void Update()
    {
        SetDirectionAndVelocity();
        PlayBeepSound();
        SetCrosshair();
    }
    private void FindTarget()
    {
        List<GameObject> enemies = ObstacleController.instance.enemies;
        enemies.RemoveAll(x => x == null);

        GameObject temp = enemies.Where(x => Vector3.Distance(x.transform.position, transform.position) >= 10f && x.transform.position.z > transform.position.z).FirstOrDefault();
        if (temp != null) target = temp.transform;
        else if(ObstacleController.instance.enemies.Count > 0)
        {
            temp = ObstacleController.instance.enemies[Random.Range(0, ObstacleController.instance.enemies.Count)];
            target = temp.transform;
        }
    }
    private void PlayBeepSound()
    {
        if (target == null) return;

        beepTimer += Time.deltaTime;
        if(beepTimer >= beepTime)
        {
            beepTimer = 0f;
            beepTime = Mathf.Max(0.1f, beepTime - 0.01f);
            SoundController.instance.Play(SFXList.Beep);
        }
    }
    private void InitializeCrosshair()
    {
        if (target == null)
        {
            Destroy(targetSpriteRenderer);
            return;
        }

        targetSpriteRenderer.transform.localScale = Vector3.one * target.GetComponent<CapsuleCollider>().height / 2f;
    }
    private void SetCrosshair()
    {
        if(target == null)
        {
            if(targetSpriteRenderer != null) Destroy(targetSpriteRenderer);
            return;
        }


        targetSpriteRenderer.transform.forward = Camera.main.transform.position - target.transform.position;

        Vector3 pos = target.transform.position + targetSpriteRenderer.transform.forward;
        pos.y += target.GetComponent<CapsuleCollider>().height / 2f;
        targetSpriteRenderer.transform.position = pos;
    }
    private void SetDirectionAndVelocity()
    {
        if (target == null)
        {
            rb.velocity = transform.forward * Speed;
        }
        else
        {
            timer += Time.deltaTime;

            if(timer < 0.5f)
            {
                transform.position += transform.forward * Speed * Time.deltaTime;
            }
            else
            {
                Vector3 targetPos = target.position;
                targetPos.y = target.transform.position.y + target.GetComponent<CapsuleCollider>().height / 2f;

                Vector3 dir = (targetPos - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 30f);

                float dist = (targetPos - transform.position).magnitude;

                float dist2 = Speed * Time.deltaTime;

                transform.position += transform.forward * Mathf.Min(dist, dist2);
            }

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Road") DestroyEvent();

        if (other.gameObject.TryGetComponent<I_Destroy>(out I_Destroy comp))
        {
            comp.KillWithGun(40, GunType.Missile);
            DestroyEvent();
        }
    }
    private void DestroyEvent()
    {
        if (destroyDetected) return;
        destroyDetected = true;

        explosion.transform.SetParent(null);
        explosion.Play();
        SoundController.instance.Play(SFXList.etfx_explosion_nuke);
        Destroy(explosion.gameObject, 3f);
        Destroy(gameObject);
    }
}
