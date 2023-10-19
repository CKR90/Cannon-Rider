using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{

    [HideInInspector] public AutoPilot Vehicle;
    [HideInInspector] public float LifeTime;

    [SerializeField] private ParticleSystem Flame;
    [SerializeField] private ParticleSystem Explosion;
    private AudioSource aud;
    private float Timer = 0f;
    private int switcher = 0;

    private bool finishGame = false;
    private void OnEnable()
    {
        aud = GetComponent<AudioSource>();
        aud.volume = 0.2f;

        switcher = 0;
        Timer = 0f;

        //Invoke("Invoke_Disable", LifeTime);

        Vehicle.Gas = 1f;
        Vehicle.BoostBlocker = 0f;
        Vehicle.BoostBlockerEnable = false;
        Vehicle.BoosterHelperEnabled = true;
        Vehicle.BlockRollUntilLanding(transform);

        SoundController.instance.Play(SFXList.Flame_Burst);

        Explosion.Play();
    }
    private void OnDisable()
    {
        Vehicle.BoosterHelperEnabled = false;
        AwardMixer.Instance.BoosterEnabled = false;

        Explosion.Play();
    }

    private void Update()
    {
        if(Vehicle.finishGame && !finishGame)
        {
            finishGame = true;
            if (switcher < 18) switcher = 18;
            Timer = LifeTime - 0.3f;


        }


        Timer += Time.deltaTime;

        switch (switcher)
        {
            case 0: RunBefore(2f); break;
            case 1: FlameState(false); Vehicle.Gas = 0.1f; break;
            case 2: RunBefore(1.9f); break;
            case 3: FlameState(true); break;
            case 4: RunBefore(1.6f); break;
            case 5: FlameState(false); break;
            case 6: RunBefore(1.5f); break;
            case 7: FlameState(true); break;
            case 8: RunBefore(1.2f); break;
            case 9: FlameState(false); break;
            case 10: RunBefore(1.0f); break;
            case 11: FlameState(true); break;
            case 12: RunBefore(0.8f); break;
            case 13: FlameState(false); break;
            case 14: RunBefore(0.6f); break;
            case 15: FlameState(true); break;
            case 16: RunBefore(0.4f); break;
            case 17: FlameState(false); break;
            case 18: RunBefore(0.3f); SoundController.instance.Play(SFXList.Flame_Burst); break;
            case 19: FlameState(true); break;
            case 20: RunBefore(0.2f); break;
            case 21: FlameState(false); break;
            case 22: RunBefore(0.1f); break;
            case 23: FlameState(true); break;
            case 24: gameObject.SetActive(false); switcher++; break;
        }
    }
    private void FlameState(bool state)
    {
        if(state)
        {
            aud.volume = 0.2f;
            Flame.Play();
            Vehicle.BoosterGas = true;
        }
        else
        {
            aud.volume = 0f;
            Flame.Stop();
            Vehicle.BoosterGas = false;
        }
        switcher++;
    }
    private void RunBefore(float time)
    {
        if (Timer >= LifeTime - time) switcher++;
    }

    private void Invoke_Disable()
    {
        gameObject.SetActive(false);
    }

    public void ResetBooster()
    {
        switcher = -1;
        Timer = 0f;

        if(Vehicle != null) Vehicle.BoosterHelperEnabled = false;
        AwardMixer.Instance.BoosterEnabled = false;
        gameObject.SetActive(false);
    }
}
