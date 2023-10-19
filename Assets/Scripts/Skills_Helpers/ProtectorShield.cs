using UnityEngine;

public class ProtectorShield : MonoBehaviour
{
    [SerializeField] private ParticleSystem DestroyParticle;

    [HideInInspector] public AwardMixer awardMixer;
    [HideInInspector] public AutoPilot Vehicle;
    [HideInInspector] public float LifeTime = 0f;

    private ParticleSystem ps;

    private float timer = 0f;

    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(gameObject, LifeTime);
        
    }

    void Update()
    {
        transform.position = Vehicle.transform.position + Vehicle.transform.forward * .2f + Vector3.up * .1f;

        timer += Time.deltaTime;

        if(timer >= LifeTime - 3f)
        {
            ParticleSystem.MainModule psm = ps.main;

            psm.startColor = Color.Lerp(psm.startColor.color, Color.red, Time.deltaTime * 1.5f);
        }

        if(timer >= LifeTime - 0.5f)
        {
            SoundController.instance.Play(SFXList.electronic_02, .5f);
        }
    }

    private void OnDestroy()
    {
        AwardMixer.Instance.ProtectorEnabled = false;
        DestroyParticle.transform.SetParent(Vehicle.transform);
        DestroyParticle.Play();
        Destroy(DestroyParticle.gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<I_Destroy>(out I_Destroy d))
        {
            other.isTrigger = true;
            d.KillWithGun(10, GunType.Any);
        }
    }
}
