using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChiliGAI : MonoBehaviour
{
    [HideInInspector] public AudioClip ExplosionSFX;
    [HideInInspector] public Vector3 CenterPos;
    [HideInInspector] public Transform Vehicle;
    [HideInInspector] public float RoadLength = 12f;
    private Animator animator;
    private float timer = 0f;
    private bool isDestroyed = false;
    private bool blockUpdate = false;

    void Start()
    {
        transform.right = CenterPos - transform.position;
        animator = GetComponent<Animator>();

        Ray r = new Ray(transform.position, -Vector3.up);

        if(Physics.Raycast(r, out RaycastHit hit, 100f))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }
        transform.DOScale(1f, 1f);
    }

    void Update()
    {
        Check_Destroy();

        if (blockUpdate) return;

        timer += Time.deltaTime;
        if(timer >= 1.2f)
        {
            timer = 0f;
            animator.SetTrigger("walk");
            transform.DOShakeRotation(0.4f, 10, 5, 90).OnComplete(() =>
            {
                transform.DOMove(transform.position + transform.right * 1f, 0.4f);
                GetComponent<AudioSource>().Play();
            });
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            Invoke("Invoke_Destroy", .1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke("Invoke_Destroy", .1f);
        }
    }

    private void Invoke_Destroy()
    {
        if(isDestroyed) return;
        isDestroyed = true;

        ParticleSystem ps = transform.GetComponentInChildren<ParticleSystem>();
        ps.transform.SetParent(null);
        ps.Play();
        Destroy(ps.gameObject, 3f);
        SoundController.instance.Play(SFXList.Suction_Plop_2, 1f);
        Destroy(gameObject);
    }
    private void Check_Destroy()
    {
        if(transform.position.z < Vehicle.position.z - 10f) Destroy(gameObject);
        else if(Mathf.Abs(transform.position.x) > RoadLength / 2f ) Invoke_Destroy();
    }
}
