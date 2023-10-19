using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardBox : MonoBehaviour, I_Destroy
{
    public ParticleSystem CrackParticle;
    [HideInInspector] public AwardMixer awardMixer;
    [HideInInspector] public AutoPilot Vehicle;

    private bool playerDetect = false;

    public void Explode()
    {
        awardMixer.RunMixer();
        CrackParticle.transform.SetParent(null);
        CrackParticle.Play();
        Destroy(CrackParticle.gameObject, 3f);

        SoundController.instance.Play(SFXList.Crash_BoxEffect2);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!playerDetect && collision.collider.gameObject.tag == "Player")
        {
            playerDetect = true;
            Vehicle.BombEffect(transform, 300f, 0f, false);
            Explode();
        }

        if (collision.collider.gameObject.tag != "Road") return;

        Destroy(GetComponent<Rigidbody>());
        SoundController.instance.Play(SFXList.Window_Hit_2);

        


    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerDetect && other.gameObject.tag == "Player")
        {
            playerDetect = true;
            Vehicle.BombEffect(transform, 300f, 0f, false);
            Explode();
        }
    }
    public void KillWithGun(int coin, GunType gunType)
    {
        Explode();
    }
}
