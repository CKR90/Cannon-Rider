using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PirateBomb : MonoBehaviour
{
    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public float Speed;
    public ParticleSystem MissileParticle;
    public ParticleSystem ExplosionParticle;
    public AudioSource ExplosionSound;
    public AudioSource RollSound;

    private List<GameObject> inArea = new List<GameObject>();

    void Start()
    {
        GetComponent<Rigidbody>().velocity = Direction.normalized * Speed;

        Invoke("DestroyEvent", 5f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag != "Road")
        {
            SoundController.instance.PlayTalk(TalkList.Voice_Clip_Male_60);
            DestroyEvent();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<I_Destroy>(out I_Destroy component))
        {
            inArea.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<I_Destroy>(out I_Destroy component))
        {
            if(inArea.Find(x => x == other.gameObject) != null)
            {
                inArea.Remove(other.gameObject);
            }
        }
    }
    private void DestroyEvent()
    {
        ExplosionParticle.transform.SetParent(null);
        ExplosionParticle.transform.localEulerAngles = Vector3.zero;
        Destroy(ExplosionParticle.gameObject, 3f);
        ExplosionParticle.Play();   
        ExplosionSound.Play();

        Destroy(transform.GetComponentInChildren<MeshRenderer>());
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(RollSound);
        if(MissileParticle != null) Destroy(MissileParticle.gameObject);
        
        Destroy(gameObject, 1f);

        foreach(var g in inArea)
        {
            if(g != null) g.GetComponent<I_Destroy>().KillWithGun(40, GunType.Pirate);
        }
    }
}
