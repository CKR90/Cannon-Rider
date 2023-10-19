using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonGun : MonoBehaviour
{
    public AutoPilot pilot;
    public Rigidbody VehicleRB;
    public GameObject Bullet;
    public ParticleSystem BulletFireEffect;
    public Transform BulletInstantiatePosition;
    [Range(1, 20)] public int level = 1;
    [Range(20f, 100f)] public float BulletImpactDistance = 50f;
    [Range(20f, 100f)] public float BulletSpeed = 7f;
    [Range(10f, 60f)] public float GunLerpSpeed = 7f;
    [Range(10f, 70f)] public float GunAngleLimit = 50f;
    [Range(.1f, 1f)] public float GunAngleMultiplier = 1.5f;

    private Camera cam;
    private float screenWidth = 0f;
    private AudioSource audioSource;

    private bool isClicked = false;

    private Vector2 anchor = Vector2.zero;
    [HideInInspector] public bool controllerType = false;

    private float mouseTriangleHipLength = 0f;
    private float xRatio = 0f;

    private bool finishGame = false;

    void Awake()
    {
        xRatio = 600f / 1080f;
        mouseTriangleHipLength = Screen.width * xRatio;
        cam = Camera.main;
        screenWidth = Screen.width;
        audioSource = GetComponent<AudioSource>();

        SetGunProperty();
    }

    private void SetGunProperty()
    {
        level = Mathf.Clamp(User.info.cannonlevel, 1, 20);

        switch (level)
        {
            case 1:  BulletImpactDistance = 25; BulletSpeed = 50; break;
            case 2:  BulletImpactDistance = 28; BulletSpeed = 51; break;
            case 3:  BulletImpactDistance = 30; BulletSpeed = 52; break;
            case 4:  BulletImpactDistance = 32; BulletSpeed = 53; break;
            case 5:  BulletImpactDistance = 34; BulletSpeed = 54; break;
            case 6:  BulletImpactDistance = 35; BulletSpeed = 55; break;
            case 7:  BulletImpactDistance = 36; BulletSpeed = 56; break;
            case 8:  BulletImpactDistance = 37; BulletSpeed = 57; break;
            case 9:  BulletImpactDistance = 38; BulletSpeed = 58; break;
            case 10: BulletImpactDistance = 40; BulletSpeed = 59; break;
            case 11: BulletImpactDistance = 42; BulletSpeed = 60; break;
            case 12: BulletImpactDistance = 44; BulletSpeed = 61; break;
            case 13: BulletImpactDistance = 46; BulletSpeed = 62; break;
            case 14: BulletImpactDistance = 48; BulletSpeed = 63; break;
            case 15: BulletImpactDistance = 50; BulletSpeed = 64; break;
            case 16: BulletImpactDistance = 52; BulletSpeed = 65; break;
            case 17: BulletImpactDistance = 54; BulletSpeed = 66; break;
            case 18: BulletImpactDistance = 56; BulletSpeed = 67; break;
            case 19: BulletImpactDistance = 58; BulletSpeed = 68; break;
            case 20: BulletImpactDistance = 60; BulletSpeed = 70; break;

        }
    }
    void Update()
    {
        if(finishGame)
        {
            ReturnDefaultPosition();
            KeepHorizontalDirection();
            return;
        }

        if (Input.GetMouseButton(0) && GameAreaActivator.Instance.ControlEnabled)
        {
            if (isClicked == false)
            {
                if (cam.WorldToScreenPoint(transform.position).y + 10f> Input.mousePosition.y) controllerType = true;
                else controllerType = false;
                isClicked = true;
            }
            if(controllerType) SetGunPosition();
            else ReturnDefaultPosition();
        }
        else
        {
            if (isClicked)
            {
                isClicked = false;
                controllerType = false;
            }
            ReturnDefaultPosition();
        }

        KeepHorizontalDirection();
    }

    private void SetGunPosition()
    {
        Vector2 t = GetScreenDirection();
        float magnitude = t.magnitude;
        float angle = Vector2.SignedAngle(t, Vector2.up) * GunAngleMultiplier;
        angle = Mathf.Clamp(angle, -GunAngleLimit, GunAngleLimit);

        float currentY = transform.localEulerAngles.y;
        if (currentY > 180f) currentY -= 360f;

        float value = Mathf.LerpAngle(currentY, angle, Time.deltaTime * GunLerpSpeed);

        transform.localEulerAngles = new Vector3(0f, value, 0);
    }
    
    public void Fire(RaycastHit hit)
    {
        GameObject g = Instantiate(Bullet, null);
        g.GetComponent<CannonBullet>().SetTarget(hit);
        g.GetComponent<CannonBullet>().SetBulletData(BulletSpeed, BulletImpactDistance);
        g.transform.position = BulletInstantiatePosition.position;
        g.transform.forward = BulletInstantiatePosition.forward;
        Destroy(g, 3f);

        pilot.SetVirtualAcceleration();
        BulletFireEffect.Play();
        audioSource.Play();
    }
    public void Fire()
    {
        GameObject g = Instantiate(Bullet, null);
        g.GetComponent<CannonBullet>().SetBulletData(BulletSpeed, BulletImpactDistance);
        g.transform.position = BulletInstantiatePosition.position;
        g.transform.forward = BulletInstantiatePosition.forward;
        Destroy(g, 3f);

        pilot.SetVirtualAcceleration();
        BulletFireEffect.Play();
        audioSource.Play();
    }
    private Vector2 GetScreenDirection()
    {
        Vector2 point = Input.mousePosition;
        point.x -= screenWidth / 2f;
        anchor.y = point.y + mouseTriangleHipLength;
        return anchor - point;
    }

    private void ReturnDefaultPosition()
    {
        float currentY = transform.localEulerAngles.y;
        if (currentY > 180f) currentY -= 360f;

        float value = Mathf.LerpAngle(currentY, 0f, Time.deltaTime);

        transform.localEulerAngles = new Vector3(0f, value, 0);
    }

    private void KeepHorizontalDirection()
    {
        Vector3 horizontalDir = transform.forward;
        horizontalDir.y = 0f;

        float angle = Vector3.SignedAngle(transform.forward, horizontalDir, transform.right);

        Vector3 euler = transform.localEulerAngles;
        euler.x = angle;

        transform.localEulerAngles = euler;
    }

    public void FinishGame()
    {
        finishGame = true;
    }
}
